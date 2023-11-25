using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class BossController : MonsterController
{
  private Queue<SkillBase> _skillQueue;
  
  private void Start()
  {
    Init();
    CreatureState = ECreatureState.Skill;
    Skills.StartNextSequenceSkill();
    InvokeMonsterData();
  }

  public override bool Init()
  {
    base.Init();
    
    ObjectType = EObjectType.Boss;
    transform.localScale = new Vector3(2f, 2f, 2f);
    CreatureState = ECreatureState.Skill;
    
    return true;
  }

  protected override void UpdateAnimation()
  {
    switch (CreatureState)
    {
      case ECreatureState.Idle:
        Anim.Play("Idle");
        break;
      case ECreatureState.Moving:
        Anim.Play("Moving");
        break;
      case ECreatureState.Skill:
        break;
      case ECreatureState.Dead:
        Skills.StopSkills();
        break;
    }
  }

  public override void InitCreatureStat(bool isFullHp = true)
  {
    MaxHp = (creatureData.maxHp + (creatureData.maxHpBonus * Managers.Game.CurrentStageData.stageLevel)) * creatureData.hpRate;
    Atk = (creatureData.atk + (creatureData.atkBonus * Managers.Game.CurrentStageData.stageLevel)) * creatureData.atkRate;
    Hp = MaxHp;
    MoveSpeed = creatureData.moveSpeed * creatureData.moveSpeedRate;
  }
  
  public override void OnCollisionEnter2D(Collision2D other)
  {
    base.OnCollisionEnter2D(other);
    
    PlayerController target = other.gameObject.GetComponent<PlayerController>();
    if (target.IsValid() == false) return;
  
    if (this.IsValid() == false) return;
  }

  public override void OnCollisionExit2D(Collision2D other)
  {
    base.OnCollisionExit2D(other);
    
    PlayerController target = other.gameObject.GetComponent<PlayerController>();
    if (target.IsValid() == false) return;
  
    if (this.IsValid() == false) return;
  }
}
