using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class EnergyBolt : RepeatSkill
{
  private void Awake()
  {
    SkillType = ESkillType.EnergyBolt;
  }
  
  private IEnumerator CoSetEnergyBolt()
  {
    string prefabName = SkillData.prefabLabel;

    if (Managers.Game.Player != null)
    {
      List<MonsterController> targets = Managers.Object.GetNearestMonsters(SkillData.numProjectiles);
      if(targets == null) yield break;

      for (int i = 0; i < targets.Count; i++)
      {
        Vector3 dir = (targets[i].CenterPosition - Managers.Game.Player.CenterPosition).normalized;
        Vector3 startPos = Managers.Game.Player.CenterPosition;
        GenerateProjectile(Managers.Game.Player, prefabName, startPos, dir, Vector3.zero, this);
        yield return new WaitForSeconds(SkillData.projectileSpacing);
      }
    }
  }
  protected override void DoSkillJob()
  {
    StartCoroutine(CoSetEnergyBolt());
  }
}
