using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class IcicleArrow : RepeatSkill
{
  private void Awake()
  {
    SkillType = ESkillType.IcicleArrow;
  }
  
  protected override void DoSkillJob()
  {
    string prefabName = SkillData.prefabLabel;

    if (Managers.Game.Player != null)
    {
      Vector3 startPos = Managers.Game.Player.PlayerCenterPos;
      Vector3 dir = Managers.Game.Player.PlayerDirection;
      for (int i = 0; i < SkillData.numProjectiles; i++)
      {
        float angle = SkillData.angleBetweenProj * (i - (SkillData.numProjectiles - 1) / 2f);
        Vector3 res = Quaternion.AngleAxis(angle, Vector3.forward) * dir;
        GenerateProjectile(Managers.Game.Player, prefabName, startPos, res.normalized, Vector3.zero, this);
      }
    }
  }
}
