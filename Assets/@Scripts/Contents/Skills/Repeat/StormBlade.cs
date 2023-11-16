using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class StormBlade : RepeatSkill
{
  private float _radian;
  
  private void Awake()
  {
    SkillType = ESkillType.StormBlade;
  }
  
  private IEnumerator CoSetStormBlade()
  {
    if (Managers.Game.Player != null)
    {
      Vector3 dir = Managers.Game.Player.PlayerDirection;
      Shoot(dir);
            
      for (int i = 0; i < 7; i++)
      {
        dir = Quaternion.AngleAxis((45 + 45 * i) * -1, Vector3.forward) * dir;
        Shoot(dir);
        yield return new WaitForSeconds(SkillData.attackInterval);
      }
    }
    yield return null;
  }
  private void Shoot(Vector3 dir)
  {
    string prefabName = SkillData.prefabLabel;
    Vector3 startPos = Managers.Game.Player.PlayerCenterPos;

    for (int i = 0; i < SkillData.numProjectiles; i++)
    {
      float angle = SkillData.angleBetweenProj * (i - (SkillData.numProjectiles - 1) / 2f);
      Vector3 res = Quaternion.AngleAxis(angle, Vector3.forward) * dir;
      GenerateProjectile(Managers.Game.Player, prefabName, startPos, res.normalized, Vector3.zero, this);
    }
  }
  
  private void OnTriggerEnter2D(Collider2D collision)
  {
    if (IsLearnedSkill == false) return;
   
    CreatureController creature = collision.transform.GetComponent<CreatureController>();
    if (creature != null && creature.IsMonster())
      creature.OnDamaged(Managers.Game.Player, this);
  }
  
  protected override void DoSkillJob()
  {
    StartCoroutine(CoSetStormBlade());
  }
}
