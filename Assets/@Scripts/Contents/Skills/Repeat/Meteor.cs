using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Meteor : RepeatSkill
{
  private void Awake()
  {
    SkillType = ESkillType.Meteor;
  }
  
  private IEnumerator GenerateMeteor()
  {
    List<MonsterController> targets = Managers.Object.GetMonsterWithinCamera(SkillData.numProjectiles);
    if (targets == null) yield break;
   
    foreach (var target in targets)
    {
      if (target.IsValid())
      { 
        Vector2 startPos = GetMeteorPosition(target.CenterPosition);
        GenerateProjectile(Managers.Game.Player, "MeteorProjectile", startPos, Vector3.zero, target.CenterPosition, this);
        yield return new WaitForSeconds(SkillData.attackInterval);
      }
    }
  }
  private Vector2 GetMeteorPosition(Vector3 target)
  {
    float angleInRadians = 60f * Mathf.Deg2Rad;
    float spawnMargin = 1f;
    float halfHeight = Camera.main.orthographicSize;  // 화면의 높이 절반
    float halfWidth = Camera.main.aspect * halfHeight;     // 화면의 너비 절반

    float spawnX = target.x + (halfWidth + spawnMargin) * Mathf.Cos(angleInRadians);
    float spawnY = target.y + (halfHeight + spawnMargin) * Mathf.Sin(angleInRadians);
    Vector2 spawnPosition = new Vector2(spawnX, spawnY);

    return spawnPosition;
  }
  
  protected override void DoSkillJob()
  {
    throw new System.NotImplementedException();
  }
}
