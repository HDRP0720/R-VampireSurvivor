using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class MonsterSkill_01 : RepeatSkill
{
  private CreatureController _owner;
  private Rigidbody2D _target;
  private Rigidbody2D _rigidBody;
  
  private void Awake()
  {
    SkillType = ESkillType.MonsterSkill_01;
  }
  private void OnEnable()
  {
    _owner = GetComponent<CreatureController>();
    _target = Managers.Game.Player.GetComponent<Rigidbody2D>();
    _rigidBody = GetComponent<Rigidbody2D>();
    StopAllCoroutines();
    if (IsLearnedSkill)
      StartCoroutine(CoSetProjectile());
  }
  
  public override void ActivateSkill()
  {
    base.ActivateSkill();
    StartCoroutine(CoSetProjectile());
  }
  
  private IEnumerator CoSetProjectile()
  {
    while (true)
    { 
      Vector3 dirVec = Managers.Game.Player.CenterPosition - _owner.CenterPosition;

      if (dirVec.magnitude > SkillData.projRange)
      {
        _owner.CreatureState = ECreatureState.Moving;
      }
      else
      {
        _owner.CreatureState = ECreatureState.Skill;
        Vector3 startPos = transform.position;
        GenerateProjectile(_owner, SkillData.prefabLabel, startPos, dirVec.normalized, Vector3.zero, this);
        Managers.Sound.Play(ESound.Effect, "MonsterProjectile_Start");
        yield return new WaitForSeconds(SkillData.coolTime);
      }
      yield return null;
    }
  }
  
  protected override void DoSkillJob(){ }
}
