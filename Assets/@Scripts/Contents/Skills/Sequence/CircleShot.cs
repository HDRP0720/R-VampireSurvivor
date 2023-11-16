using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class CircleShot : SequenceSkill
{
  private CreatureController _owner;
  private Vector3 _dir;
  private Coroutine _coroutine;
  
  private void Awake()
  {
    SkillType = ESkillType.CircleShot;
    animationName = "Attack";
    _owner = GetComponent<CreatureController>();
  }
  
  private IEnumerator CoSkill(Action callback = null)
  {
    Vector3 playerPosition = Managers.Game.Player.CenterPosition;
    float angleIncrement = 360f / SkillData.numProjectiles;
    transform.GetChild(0).GetComponent<Animator>().Play(animationName);

    for (int i = 0; i < SkillData.numProjectiles; i++)
    {
      // 1. 프로젝타일 발사 위치 계산하기
      float angle = i * angleIncrement;
      Vector3 dir = Quaternion.Euler(0, 0, angle) * Vector3.up;

      // 2. 프로젝타일 발사하기
      Vector3 startPos = _owner.CenterPosition + dir;
      GenerateProjectile(_owner, SkillData.prefabLabel, startPos, dir.normalized, Vector3.zero, this);
    }
    yield return new WaitForSeconds(SkillData.attackInterval);

    callback?.Invoke();
  }
  
  public override void DoSkill(Action callback = null)
  {
    CreatureController owner = GetComponent<CreatureController>();
    if (owner.CreatureState != ECreatureState.Skill) return;

    UpdateSkillData(dataId);

    _dir = Managers.Game.Player.CenterPosition - _owner.CenterPosition;
    GetComponent<Rigidbody2D>().velocity = Vector2.zero;

    _coroutine = null;
    _coroutine = StartCoroutine(CoSkill(callback));
  }
}
