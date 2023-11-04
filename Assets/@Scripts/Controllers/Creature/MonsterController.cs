using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

using Data;
using DG.Tweening;
using static Define;

public class MonsterController : CreatureController
{
  private float _timer;
  private Vector3 _moveDir;
  private SkillData _skillData;
  private Coroutine _coMoving;
  private Coroutine _coDotDamage;
  private Coroutine _coroutineKnockBack;
  
  public event Action OnBossDead;
  public event Action<MonsterController> OnMonsterInfoUpdate;
  
  private void OnEnable()
  {
    if(DataId != 0) SetInfo(DataId);
  }
  private void FixedUpdate()
  {
    PlayerController pc = Managers.Object.Player;
    if (pc.IsValid() == false) return;
   
    _moveDir = pc.transform.position - transform.position;
    creatureSprite.flipX = _moveDir.x > 0;

    if (CreatureState != ECreatureState.Moving) return;

    Vector3 newPos = transform.position + _moveDir.normalized * (Time.deltaTime * MoveSpeed);
    Rb.MovePosition(newPos);
    _timer += Time.deltaTime;
  }
  public virtual void OnCollisionEnter2D(Collision2D other)
  {
    PlayerController target = other.gameObject.GetComponent<PlayerController>();
    if (target.IsValid() == false) return;

    if (this.IsValid() == false) return;
    
    if(_coDotDamage != null)
      StopCoroutine(_coDotDamage);
    
    _coDotDamage = StartCoroutine(CoStartDotDamage(target));
  }
  public virtual void OnCollisionExit2D(Collision2D other)
  {
    PlayerController target = other.gameObject.GetComponent<PlayerController>();
    if (target.IsValid() == false) return;

    if (this.IsValid() == false) return;
    
    if(_coDotDamage != null)
      StopCoroutine(_coDotDamage);

    _coDotDamage = null;
  }

  public override bool Init()
  {
    base.Init();
    
    ObjectType = EObjectType.Monster;
    CreatureState = ECreatureState.Moving;
    
    Rb.simulated = true;
    transform.localScale = new Vector3(1f, 1f, 1f);
    
    // Initialize turn off and on for pooling
    SetMonsterPosition();

    if (Skills)
    {
      foreach (SkillBase skill in Skills.SkillList)
      {
        skill.Level = 0;
        skill.UpdateSkillData();
      }
    }

    if (creatureData != null)
    {
      if (creatureData.skillTypeList.Count != 0)
      {
        InitSkill();
        Skills.LevelUpSkill((ESkillType) creatureData.skillTypeList[0]);
      }
    }
    
    return true;
  }
  private void SetMonsterPosition()
  {
    Vector2 randCirclePos = Utils.GenerateMonsterSpawnPosition(Managers.Game.Player.PlayerCenterPos);
    transform.position = (randCirclePos);
  }

  protected override void UpdateAnimation()
  {
    base.UpdateAnimation();
    switch (CreatureState)
    {
      case ECreatureState.Idle:
        UpdateIdle();
        break;
      case ECreatureState.Skill:
        UpdateSkill();
        break;
      case ECreatureState.Moving:
        UpdateMoving();
        break;
      case ECreatureState.Dead:
        UpdateDead();
        break;
    }
  }
  
  public override void OnDamaged(BaseController attacker, SkillBase skill = null, float damage = 0)
  {
    if (skill != null)
      Managers.Sound.Play(ESound.Effect, skill.SkillData.hitSoundLabel);
    
    float totalDmg = Managers.Game.Player.Atk * skill.SkillData.damageMultiplier;
    base.OnDamaged(attacker, skill, totalDmg);
    InvokeMonsterData();
    if (ObjectType == EObjectType.Monster)
    {
      if (_coroutineKnockBack == null)
      {
        _coroutineKnockBack = StartCoroutine(CoKnockBack());
      }
    }
  }
  private IEnumerator CoKnockBack()
  {
    float elapsed = 0;
    CreatureState = ECreatureState.OnDamaged;
    while (true)
    {
      elapsed += Time.deltaTime;
      if (elapsed > KNOCKBACK_TIME)
        break;

      Vector3 dir = _moveDir * -1f;
      Vector2 nextVec = dir.normalized * KNOCKBACK_SPEED * Time.fixedDeltaTime;
      Rb.MovePosition(Rb.position + nextVec);

      yield return null;
    }
    CreatureState = ECreatureState.Moving;

    yield return new WaitForSeconds(KNOCKBACK_COOLTIME);
    _coroutineKnockBack = null;
    yield break;
  }
  
  public override void OnDead()
  {
    base.OnDead();
    InvokeMonsterData();
    
    Managers.Game.Player.KillCount++;
    Managers.Game.TotalMonsterKillCount++;
    
    // Drop gem randomly
    if (Random.value >= Managers.Game.CurrentWaveData.nonDropRate)
    {
      GemController gem = Managers.Object.Spawn<GemController>(transform.position);
      gem.SetInfo(Managers.Game.GetGemInfo());
    }
    
    // Drop soul randomly
    if (Random.value < Define.STAGE_SOULDROP_RATE)
    {
      SoulController soul = Managers.Object.Spawn<SoulController>(transform.position);
    }
    
    Sequence seq = DOTween.Sequence();
    seq.Append(transform.DOScale(0f, 0.2f).SetEase(Ease.InOutBounce)).OnComplete(() =>
    {
      StopAllCoroutines();
      _coroutineKnockBack = null;
      Rb.velocity = Vector2.zero;
      OnBossDead?.Invoke();
      Managers.Object.Despawn(this);
    });
  }
  protected void InvokeMonsterData()
  {
    if (this.IsValid() && gameObject.IsValid() && ObjectType != EObjectType.Monster)
      OnMonsterInfoUpdate?.Invoke(this);
  }
  public override void OnDeathAnimationEnd() { }
  
  private IEnumerator CoStartDotDamage(PlayerController target)
  {
    while (true)
    {
      target.OnDamaged(this,null, Atk);
      yield return new WaitForSeconds(0.1f);
    }
  }
}
