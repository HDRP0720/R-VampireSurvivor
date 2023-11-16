using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Charging : SequenceSkill
{
  private Coroutine _coroutine;
  
  private void Awake()
  {
    SkillType = ESkillType.Charging;
    animationName = "Charge";
  }
  
  private IEnumerator CoSkill(Action callback = null)
  {
    GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    transform.GetChild(0).GetComponent<Animator>().Play(animationName);
    yield return new WaitForSeconds(SkillData.attackInterval);
    callback?.Invoke();
  }
  
  public override void DoSkill(Action callback = null)
  {
    CreatureController owner = GetComponent<CreatureController>();
    if (owner.CreatureState != ECreatureState.Skill) return;

    UpdateSkillData(dataId);

    _coroutine = null;
    _coroutine = StartCoroutine(CoSkill(callback));
  }
}
