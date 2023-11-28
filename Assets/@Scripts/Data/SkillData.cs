using System;
using System.Collections.Generic;

namespace Data
{
  [Serializable]
  public class SkillData
  {
    public int dataId;
    public string name;
    public string description;
    public string prefabLabel; //프리팹 경로
    public string iconLabel;//아이콘 경로
    public string soundLabel;// 발동사운드 경로
    public string category;//스킬 카테고리
    public float coolTime; // 쿨타임
    public float damageMultiplier; //스킬데미지 (곱하기)
    public float projectileSpacing;// 발사체 사이 간격
    public float duration; //스킬 지속시간
    public float recognitionRange;//인식범위
    public int numProjectiles;// 회당 공격횟수
    public string castingSound; // 시전사운드
    public float angleBetweenProj;// 발사체 사이 각도
    public float attackInterval; //공격간격
    public int numBounce;//바운스 횟수
    public float bounceSpeed;// 바운스 속도
    public float bounceDist;//바운스 거리
    public int numPenetrations; //관통 횟수
    public int castingEffect; // 스킬 발동시 효과
    public string hitSoundLabel; // 히트사운드
    public float probCastingEffect; // 스킬 발동 효과 확률
    public int hitEffect;// 적중시 이펙트
    public float probHitEffect; // 스킬 발동 효과 확률
    public float projRange; //투사체 사거리
    public float minCoverage; //최소 효과 적용 범위
    public float maxCoverage; // 최대 효과 적용 범위
    public float rotateSpeed; // 회전 속도
    public float projSpeed; //발사체 속도
    public float scaleMultiplier;
  }
  
  [Serializable]
  public class SkillDataLoader : ILoader<int, SkillData>
  {
    public List<SkillData> skills = new List<SkillData>();

    public Dictionary<int, SkillData> MakeDict()
    {
      Dictionary<int, SkillData> dict = new Dictionary<int, SkillData>();
      foreach (SkillData skill in skills)
        dict.Add(skill.dataId, skill);
      return dict;
    }
  }
}

