using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class ComboShot : SequenceSkill
{
  private CreatureController _owner;
  private Vector3 _dir;
  private Coroutine _coroutine;
  
  private void Awake()
  {
    SkillType = ESkillType.ComboShot;
    animationName = "Attack";
    _owner = GetComponent<CreatureController>();
  }
  
  private IEnumerator CoSkill(Action callback = null)
  {
    float angleIncrement = 360f / SkillData.numProjectiles;
    transform.GetChild(0).GetComponent<Animator>().Play(animationName);

    for (int i = 0; i < SkillData.numProjectiles; i++)
    {
      float angle = i * angleIncrement;
      Vector3 dir = Quaternion.Euler(0, 0, angle) * Vector3.up;
      Vector3 startPos = _owner.CenterPosition + dir;
      GenerateProjectile(_owner, SkillData.prefabLabel, startPos, dir.normalized, Vector3.zero, this);
    }
    yield return new WaitForSeconds(SkillData.attackInterval);

    callback?.Invoke();
  }
  
  public override void DoSkill(Action callback = null)
  {
    CreatureController owner = GetComponent<CreatureController>();
    if (owner.CreatureState != ECreatureState.Skill)
      return;

    UpdateSkillData(dataId);

    _dir = Managers.Game.Player.CenterPosition - _owner.CenterPosition;
    GetComponent<Rigidbody2D>().velocity = Vector2.zero;

    _coroutine = null;
    _coroutine = StartCoroutine(CoSkill(callback));
  }
}
