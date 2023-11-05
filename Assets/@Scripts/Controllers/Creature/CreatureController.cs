using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

using Data;
using static Define;

public abstract class CreatureController : BaseController
{
  public CreatureData creatureData;
  public Material defaultMat;
  public Material hitEffectmat;
    
  [SerializeField] protected SpriteRenderer creatureSprite;
  [SerializeField] protected bool isPlayDamagedAnim = false;
    
  protected string spriteName;
    
  private Collider2D _offset;
  private ECreatureState _creatureState = ECreatureState.Moving;

  #region Properties
  public Rigidbody2D Rb { get; set; }
  public Animator Anim { get; set; }
  public Vector3 CenterPosition => _offset.bounds.center;
  public float ColliderRadius { get; set; }
  public virtual int DataId { get; set; }
  public virtual float Hp { get; set; }
  public virtual float MaxHp { get; set; }
  public virtual float MaxHpBonusRate { get; set; } = 1;
  public virtual float HealBonusRate { get; set; } = 1;
  public virtual float HpRegen { get; set; }
  public virtual float Atk { get; set; }
  public virtual float AttackRate { get; set; } = 1;
  public virtual float Def { get; set; }
  public virtual float DefRate { get; set; }
  public virtual float CriRate { get; set; }
  public virtual float CriDamage { get; set; } = 1.5f;
  public virtual float DamageReduction { get; set; }
  public virtual float MoveSpeedRate { get; set; } = 1;
  public virtual float MoveSpeed { get; set; }
  public virtual SkillBook Skills { get; set; }
  public virtual ECreatureState CreatureState
  {
    get => _creatureState;
    set
    {
      _creatureState = value;
      UpdateAnimation();
    }
  }
  #endregion
  
  private void Awake()
  {
    Init();
  }
  
  public override bool Init()
  {
    base.Init();
    Skills = gameObject.GetOrAddComponent<SkillBook>();
    Rb = GetComponent<Rigidbody2D>();
    _offset = GetComponent<Collider2D>();
    ColliderRadius = GetComponent<CircleCollider2D>().radius;

    creatureSprite = GetComponent<SpriteRenderer>();
    if (creatureSprite == null)
      creatureSprite = Utils.FindChild<SpriteRenderer>(gameObject);

    Anim = GetComponent<Animator>();
    if (Anim == null)
      Anim = Utils.FindChild<Animator>(gameObject);

    return true;
  }

  protected virtual void UpdateAnimation() { }
  protected virtual void UpdateIdle() { }
  protected virtual void UpdateSkill() { }
  protected virtual void UpdateMoving() { }
  protected virtual void UpdateDead() { }
  

  public virtual void OnDamaged(BaseController attacker, SkillBase skill = null, float damage = 0)
  {
    bool isCritical = false;
    PlayerController player = attacker as PlayerController;
    if (player != null)
    {
      //크리티컬 적용
      if (Random.value <= player.CriRate)
      {
        damage = damage * player.CriDamage;
        isCritical = true;
      }
    }

    if (skill) skill.TotalDamage += damage;
      
    Hp -= damage;
    Managers.Object.ShowDamageFont(CenterPosition, damage, 0, transform, isCritical);

    if (gameObject.IsValid() || this.IsValid())
      StartCoroutine(PlayDamageAnimation());
  }
  
  public virtual void OnDead()
  {
    Rb.simulated = false;
    transform.localScale = new Vector3(1, 1, 1);
    CreatureState = ECreatureState.Dead;
  }
  
  public abstract void OnDeathAnimationEnd();
  
  public virtual void InitCreatureStat(bool isFullHp = true)
  {
    float waveRate = Managers.Game.CurrentWaveData.hpIncreaseRate;
    
    // Only for Elite monster, monsters except boss, player
    MaxHp = (creatureData.maxHp + (creatureData.maxHpBonus * Managers.Game.CurrentStageData.stageLevel)) * (creatureData.hpRate + waveRate);
    Atk = (creatureData.atk + (creatureData.atkBonus * Managers.Game.CurrentStageData.stageLevel)) * creatureData.atkRate;
    Hp = MaxHp;
    MoveSpeed = creatureData.moveSpeed * creatureData.moveSpeedRate;
  }
  public virtual void UpdatePlayerStat() { }
  public virtual void Healing(float amount, bool isEffect = true) { }
  
  public void SetInfo(int creatureId)
  {
    DataId = creatureId;
    Dictionary<int, CreatureData> dict = Managers.Data.CreatureDic;
    creatureData = dict[creatureId];
    InitCreatureStat();
    Sprite sprite = Managers.Resource.Load<Sprite>(creatureData.iconLabel);
    creatureSprite.sprite = Managers.Resource.Load<Sprite>(creatureData.iconLabel);

    Init();
  }
  
  public void LoadSkill()
  {
    foreach (KeyValuePair<ESkillType, int> pair in Managers.Game.ContinueInfo.savedBattleSkill.ToList())
    {
      Skills.LoadSkill(pair.Key, pair.Value);
    }
    foreach (SupportSkillData supportSkill in Managers.Game.ContinueInfo.savedSupportSkill.ToList())
    {
      Skills.AddSupportSkill(supportSkill, true);
    }
  }
  
  public virtual void InitSkill()
  {
    foreach (int skillId in creatureData.skillTypeList)
    {
      ESkillType type = Utils.GetSkillTypeFromInt(skillId);
      if (type != ESkillType.None)
        Skills.AddSkill(type, skillId);
    }
  }
  
  public bool IsMonster()
  {
    switch (ObjectType)
    {
      case EObjectType.Boss:
      case EObjectType.Monster:
      case EObjectType.EliteMonster:
        return true;
      case EObjectType.Player:
      case EObjectType.Projectile:
        return false; ;
      default:
        return false;
    }
  }
  
  private IEnumerator PlayDamageAnimation()
  {
    if (isPlayDamagedAnim == false)
    {
      isPlayDamagedAnim = true;
      defaultMat = Managers.Resource.Load<Material>("CreatureDefaultMat");
      hitEffectmat = Managers.Resource.Load<Material>("PaintWhite");
      
      // Damaged Animation
      creatureSprite.material = hitEffectmat;
      yield return new WaitForSeconds(0.1f);
      creatureSprite.material = defaultMat;

      if (Hp <= 0)
      {
        transform.localScale = new Vector3(1, 1, 1);
        switch (ObjectType)
        {
          case EObjectType.Player:
            // Check Resurrection count
            SupportSkillData resurrection = Skills.supportSkills
              .FirstOrDefault(skill => skill.supportSkillName == ESupportSkillName.Resurrection);

            if (resurrection == null)
            {
              OnDead();
            }
            else
            {
              Resurrection(resurrection.healRate, resurrection.moveSpeedRate, resurrection.atkRate);
              //
              Skills.supportSkills.Remove(resurrection);
              Skills.OnSkillBookChanged();
            }
            break;
          
          default:
            OnDead();
            break;
        }
      }
      isPlayDamagedAnim = false;
    }
  }
  
  public void Resurrection(float healRate, float moveSpeed = 0, float atkRate = 0)
  {
    Healing(healRate, false);
    Managers.Resource.Instantiate("Revival", transform);
    MoveSpeedRate += moveSpeed;
    AttackRate += atkRate;
    UpdatePlayerStat();
    Managers.Object.KillAllMonsters();
  }
}
