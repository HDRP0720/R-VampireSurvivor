using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static Define;

public class ArrowShot : RepeatSkill
{
  private void Awake()
  {
    SkillType = ESkillType.ArrowShot;
  }
  
  protected override void DoSkillJob()
  {
    StartCoroutine(CoSetArrowShot());
  }
  private IEnumerator CoSetArrowShot()
  { 
    string prefabName = SkillData.prefabLabel;

    if (Managers.Game.Player != null)
    {
      List<MonsterController> target = Managers.Object.GetNearestMonsters(SkillData.numProjectiles);
      if (target == null) yield break;
   
      for (int i = 0; i < target.Count; i++)
      {
        Vector3 dir = Managers.Game.Player.PlayerDirection;
        Vector3 startPos = Managers.Game.Player.CenterPosition;
        GenerateProjectile(Managers.Game.Player, prefabName, startPos, dir, Vector3.zero, this);
        yield return new WaitForSeconds(SkillData.projectileSpacing);
      }
    }
  }
}
