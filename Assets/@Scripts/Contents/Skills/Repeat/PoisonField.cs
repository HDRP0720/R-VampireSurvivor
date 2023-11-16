using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class PoisonField : RepeatSkill
{
  public void Awake()
  {
    SkillType = ESkillType.PoisonField;
  }
  
  private IEnumerator CoGeneratePoisonField()
  {
    Vector2 dir = Managers.Game.Player.PlayerDirection;
    for (int i = 0; i < SkillData.numProjectiles; i++)
    {
      Vector3 pos = Quaternion.AngleAxis(SkillData.angleBetweenProj * i, Vector3.forward) * dir * SkillData.projRange;

      Vector3 spawnPos = Managers.Game.Player.PlayerCenterPos;

      string prefabName = Level == 6 ? "PoisonFieldProjectile_Final" : "PoisonFieldProjectile";
      if (SkillData.numProjectiles == 1)
        GenerateProjectile(Managers.Game.Player, prefabName, spawnPos, dir, spawnPos, this);
      else
        GenerateProjectile(Managers.Game.Player, prefabName, spawnPos, dir, spawnPos + pos, this);

      yield return new WaitForSeconds(SkillData.attackInterval);
    }
  }
  
  protected override void DoSkillJob()
  {
    StartCoroutine(CoGeneratePoisonField());
  }
}
