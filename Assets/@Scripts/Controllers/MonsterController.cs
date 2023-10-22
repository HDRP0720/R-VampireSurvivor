using System.Collections;
using UnityEngine;

public class MonsterController : CreatureController
{
  protected Animator animator;
  
  private Coroutine _coDotDamage;
  private Define.ECreatureState _creatureState = Define.ECreatureState.Moving;
  
  // Property
  public virtual Define.ECreatureState CreatureState
  {
    get => _creatureState;
    set
    {
      _creatureState = value;
      UpdateAnimation();
    }
  }
  
  private void FixedUpdate()
  {
    if (CreatureState != Define.ECreatureState.Moving) return;
    
    PlayerController pc = Managers.Object.Player;
    if (pc == null) return;

    Vector3 dir = pc.transform.position - transform.position;
    Vector3 newPos = transform.position + dir.normalized * (Time.deltaTime * _speed);
    GetComponent<Rigidbody2D>().MovePosition(newPos);

    GetComponent<SpriteRenderer>().flipX = dir.x > 0;
  }

  public override bool Init()
  {
    if (base.Init()) return false;

    animator = GetComponent<Animator>();
    ObjectType = Define.EObjectType.Monster;
    CreatureState = Define.ECreatureState.Moving;
    
    return true;
  }

  private void OnCollisionEnter2D(Collision2D other)
  {
    PlayerController target = other.gameObject.GetComponent<PlayerController>();
    if (target.IsValid() == false) return;

    if (this.IsValid() == false) return;
    
    if(_coDotDamage != null)
      StopCoroutine(_coDotDamage);
    
    _coDotDamage = StartCoroutine(CoStartDotDamage(target));
  }
  private void OnCollisionExit2D(Collision2D other)
  {
    PlayerController target = other.gameObject.GetComponent<PlayerController>();
    if (target.IsValid() == false) return;

    if (this.IsValid() == false) return;
    
    if(_coDotDamage != null)
      StopCoroutine(_coDotDamage);

    _coDotDamage = null;
  }
  private IEnumerator CoStartDotDamage(PlayerController target)
  {
    while (true)
    {
      target.OnDamaged(this, 2);
      yield return new WaitForSeconds(0.1f);
    }
  }

  protected override void OnDead()
  {
    base.OnDead();

    Managers.Game.KillCount++;
    
    if(_coDotDamage != null)
      StopCoroutine(_coDotDamage);
    _coDotDamage = null;
    
    // Whenever monster died, spawn exp gem
    GemController gc = Managers.Object.Spawn<GemController>(transform.position);

    Managers.Object.Despawn(this);
  }

  #region Boss State pattern
  protected virtual void UpdateAnimation() { }
  public override void HandleUpdate()
  {
    base.HandleUpdate();
    switch (CreatureState)
    {
      case Define.ECreatureState.Idle:
        UpdateIdle();
        break;
      case Define.ECreatureState.Skill:
        UpdateSkill();
        break;
      case Define.ECreatureState.Moving:
        UpdateMoving();
        break;
      case Define.ECreatureState.Dead:
        UpdateDead();
        break;
    }
  }
  protected virtual void UpdateIdle(){}
  protected virtual void UpdateSkill(){}
  protected virtual void UpdateMoving(){}
  protected virtual void UpdateDead(){}
  #endregion
}
