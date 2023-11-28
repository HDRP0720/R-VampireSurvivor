using System;
using System.Collections.Generic;

namespace Data
{
  [Serializable]
  public class SupportSkillData
  {
    public int acquiredLevel;
    public int dataId;
    public Define.ESupportSkillType supportSkillType;
    public Define.ESupportSkillName supportSkillName;
    public Define.ESupportSkillGrade supportSkillGrade;
    public string name;
    public string description;
    public string iconLabel;
    public float hpRegen;
    public float healRate; // 회복량 (최대HP%)
    public float healBonusRate; // 회복량 증가
    public float magneticRange; // 아이템 습득 범위
    public int soulAmount; // 영혼획득
    public float hpRate;
    public float atkRate;
    public float defRate;
    public float moveSpeedRate;
    public float criRate;
    public float criDmg;
    public float damageReduction;
    public float expBonusRate;
    public float soulBonusRate;
    public float projectileSpacing;// 발사체 사이 간격
    public float duration; //스킬 지속시간
    public int numProjectiles;// 회당 공격횟수
    public float attackInterval; //공격간격
    public int numBounce;//바운스 횟수
    public int numPenetrations; //관통 횟수
    public float projRange; //투사체 사거리
    public float rotateSpeed; // 회전 속도
    public float scaleMultiplier;
    public float price;
    public bool isLocked = false;
    public bool isPurchased = false;
    
    public bool CheckRecommendationCondition()
    {
      if (isLocked || Managers.Game.SoulShopList.Contains(this)) return false;

      if (supportSkillType == Define.ESupportSkillType.Special)
      {
        if (Managers.Game.EquippedEquipments.TryGetValue(Define.EEquipmentType.Weapon, out Equipment myWeapon))
        {
          int skillId = myWeapon.equipmentData.basicSkill;
          Define.ESkillType type = Utils.GetSkillTypeFromInt(skillId);

          switch (supportSkillName)
          {
            case Define.ESupportSkillName.ArrowShot:
            case Define.ESupportSkillName.SavageSmash:
            case Define.ESupportSkillName.PhotonStrike:
            case Define.ESupportSkillName.Shuriken:
            case Define.ESupportSkillName.EgoSword:
              if (supportSkillName.ToString() != type.ToString())
                return false;
              break;
          }

        }
      }
            
      #region 서포트 스킬 중복 방지 모드 보류
      //if (Managers.Game.Player.Skills.SupportSkills.TryGetValue(SupportSkillName, out var existingSkill))
      //{
      //    if (existingSkill == null)
      //        return true;

      //    if (DataId <= existingSkill.DataId)
      //    {
      //        return false;
      //    }
      //}
      #endregion

      return true;
    }
  }
  
  [Serializable]
  public class SupportSkillDataLoader : ILoader<int, SupportSkillData>
  {
    public List<SupportSkillData> supportSkills = new List<SupportSkillData>();

    public Dictionary<int, SupportSkillData> MakeDict()
    {
      Dictionary<int, SupportSkillData> dict = new Dictionary<int, SupportSkillData>();
      foreach (SupportSkillData skill in supportSkills)
        dict.Add(skill.dataId, skill);
      return dict;
    }
  }
}

