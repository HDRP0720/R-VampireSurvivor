using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossController : MonsterController
{
  public override bool Init()
  {
    base.Init();
    animator = GetComponent<Animator>();
    CreatureState = Define.ECreatureState.Moving;
    HP = 10000000;
    
    CreatureState = Define.ECreatureState.Skill;
    Skills.AddSkill<Move>(transform.position);
    Skills.AddSkill<Dash>(transform.position);
    Skills.AddSkill<Dash>(transform.position);
    Skills.AddSkill<Dash>(transform.position);
    Skills.StartNextSequenceSkill();

    return true;
  }

  protected override void UpdateAnimation()
  {
    switch (CreatureState)
    {
      case Define.ECreatureState.Idle:
        animator.Play("Idle");
        break;
      case Define.ECreatureState.Moving:
        animator.Play("Moving");
        break;
      case Define.ECreatureState.Skill:
        break;
      case Define.ECreatureState.Dead:
        animator.Play("Death");
        break;
    }
  }

  protected override void UpdateDead()
  {
    Skills.StopSkills();
    
    if(_coWait == null)
      Managers.Object.Despawn(this);
  }

  public override void OnDamaged(BaseController attacker, int damage)
  {
    base.OnDamaged(attacker, damage);
  }

  protected override void OnDead()
  {
    CreatureState = Define.ECreatureState.Dead;
    Wait(2.0f);
  }

  #region  Wait Coroutine
  private Coroutine _coWait;
  private void Wait(float waitSeconds)
  {
    if(_coWait != null) 
      StopCoroutine(_coWait);
 
    _coWait = StartCoroutine(CoStartWait(waitSeconds));
  }
  private IEnumerator CoStartWait(float waitSeconds)
  {
    yield return new WaitForSeconds(waitSeconds);
    _coWait = null;
  }
  #endregion
}
