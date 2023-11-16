using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Shuriken : RepeatSkill
{
  private void Awake()
  {
    SkillType = ESkillType.Shuriken;
  }
  
  private IEnumerator CoSetShuriken(ESkillType type)
  {
    string prefabName = SkillData.prefabLabel;

    if (Managers.Game.Player != null)
    {
      List<MonsterController> targets = Managers.Object.GetMonsterWithinCamera(SkillData.numProjectiles);
      if (targets == null) yield break;
      
      foreach (var target in targets)
      {
        if (target.IsValid() == false) continue;
     
        Vector3 dir = target.CenterPosition - Managers.Game.Player.CenterPosition;
        Vector3 startPos = Managers.Game.Player.CenterPosition;
        GenerateProjectile(Managers.Game.Player, prefabName, startPos, dir.normalized, Vector3.zero, this);

        yield return new WaitForSeconds(SkillData.projectileSpacing);
      }
    }
  }
  
  protected override void DoSkillJob()
  {
    StartCoroutine(CoSetShuriken(SkillType));
  }
}
