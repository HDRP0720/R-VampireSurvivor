using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class PhotonStrike : RepeatSkill
{
  private void Awake()
  {
    SkillType = ESkillType.PhotonStrike;
  }
  
  private IEnumerator CoSetPhotonStrike()
  {
    string prefabName = SkillData.prefabLabel;
    if (Managers.Game.Player != null)
    {
      for (int i = 0; i < SkillData.numProjectiles; i++)
      {
        Vector3 dir = Vector3.one;
        Vector3 startPos = Managers.Game.Player.CenterPosition;
        GenerateProjectile(Managers.Game.Player, prefabName, startPos, dir, Vector3.zero, this);

        yield return new WaitForSeconds(SkillData.projectileSpacing);
      }
    }
  }
  
  protected override void DoSkillJob()
  {
    StartCoroutine(CoSetPhotonStrike());
  }
}
