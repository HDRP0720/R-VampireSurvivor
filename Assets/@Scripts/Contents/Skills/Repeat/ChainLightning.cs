using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class ChainLightning : RepeatSkill
{
  private void Awake()
  {
    SkillType = ESkillType.ChainLightning;
  }
  
  protected override void DoSkillJob()
  {
    StartCoroutine(CoChainLightning());
  }
  private IEnumerator CoChainLightning()
  {
    string prefabName = SkillData.prefabLabel;
    if (Managers.Game.Player != null)
    {
      for (int i = 0; i < SkillData.numProjectiles; i++)
      {
        Vector3 startPos = Managers.Game.Player.PlayerCenterPos;
        int minDist = (int)SkillData.bounceDist - 1;
        int maxDist = (int)SkillData.bounceDist + 1;
        List<MonsterController> targets = GetChainMonsters(SkillData.numBounce, minDist, maxDist, index : i);
        if (targets == null) continue;
      
        for (int j = 0; j < targets.Count; j++)
        {
          if (j > 0)
            startPos = targets[j - 1].CenterPosition;
          
          Vector3 dir = (targets[j].CenterPosition - startPos).normalized;
          GenerateProjectile(Managers.Game.Player, prefabName, startPos, dir, targets[j].CenterPosition, this);
        }
        yield return null;
      }
    }
  }

  private List<MonsterController> GetChainMonsters(int numTargets, float minDistance, float maxDistance, float angleRange = 180, int index = 0)
  {
    List<MonsterController> chainMonsters = new List<MonsterController>();
    // projRange 이상의 몬스터만 검색
    List<MonsterController> nearestMonster = Managers.Object.GetNearestMonsters(SkillData.numProjectiles, (int)SkillData.projRange);
    if (nearestMonster != null)
    {
      int idx = Mathf.Min(index, nearestMonster.Count-1);
      chainMonsters.Add(nearestMonster[idx]);

      for (int i = 1; i < numTargets; i++)
      {
        MonsterController chainMonster = GetChainMonster(chainMonsters[i - 1].transform.position, minDistance, maxDistance, angleRange, chainMonsters);
        if (chainMonster != null)
        {
          chainMonsters.Add(chainMonster);
        }
        else
        {
          break;
        }
      }
    }

    return chainMonsters;
  }

  private MonsterController GetChainMonster(Vector3 origin, float minDistance, float maxDistance, float angleRange, List<MonsterController> ignoreMonsters)
  {
    LayerMask targetLayer = LayerMask.GetMask("Monster", "Boss");
    Collider2D[] targets = Physics2D.OverlapCircleAll(origin, maxDistance, targetLayer);

    float closestDistance = Mathf.Infinity;
    MonsterController closestMonster = null;
    foreach (Collider2D target in targets)
    {
      if (ignoreMonsters.Contains(target.GetComponent<MonsterController>())) continue;

      Vector3 targetPosition = target.transform.position;
      float distance = Vector3.Distance(origin, targetPosition);
      if (distance >= minDistance && distance <= maxDistance)
      {
        Vector3 direction = (targetPosition - origin).normalized;
        float angle = Vector3.Angle(direction, Vector3.up);
        {
          closestDistance = distance;
          closestMonster = target.GetComponent<MonsterController>();
        }
      }
    }

    return closestMonster;
  }
}
