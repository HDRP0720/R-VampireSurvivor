using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System;
using System.Collections.Generic;
using UnityEngine.Serialization;
using static Define;

namespace Data
{
    #region LevelData
    [Serializable]
    public class LevelData
    {
        public int level;
        public int totalExp;
        public int requiredExp;
    }

    [Serializable]
    public class LevelDataLoader : ILoader<int, LevelData>
    {
        public List<LevelData> levels = new List<LevelData>();
        public Dictionary<int, LevelData> MakeDict()
        {
            Dictionary<int, LevelData> dict = new Dictionary<int, LevelData>();
            foreach (LevelData levelData in levels)
                dict.Add(levelData.level, levelData);
            return dict;
        }
    }
    #endregion

    #region CreatureData
    [Serializable]
    public class CreatureData
    {
        public int dataId;
        public string descriptionTextID;
        public string prefabLabel;
        public float maxHp;
        public float maxHpBonus;
        public float atk;
        public float atkBonus;
        public float def;
        public float moveSpeed;
        public float totalExp;
        public float hpRate;
        public float atkRate;
        public float defRate;
        public float moveSpeedRate;
        public string iconLabel;
        public List<int> skillTypeList;//InGameSkills를 제외한 추가스킬들
    }

    [Serializable]
    public class CreatureDataLoader : ILoader<int, CreatureData>
    {
        public List<CreatureData> creatures = new List<CreatureData>();
        public Dictionary<int, CreatureData> MakeDict()
        {
            Dictionary<int, CreatureData> dict = new Dictionary<int, CreatureData>();
            foreach (CreatureData creature in creatures)
                dict.Add(creature.dataId, creature);
            return dict;
        }
    }
    #endregion

    #region SkillData
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
    #endregion

    #region SupportSkilllData
    [Serializable]
    public class SupportSkillData
    {
        public int acquiredLevel;
        public int dataId;
        public ESupportSkillType supportSkillType;
        public ESupportSkillName supportSkillName;
        public ESupportSkillGrade supportSkillGrade;
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

            if (supportSkillType == ESupportSkillType.Special)
            {
                if (Managers.Game.EquippedEquipments.TryGetValue(Define.EEquipmentType.Weapon, out Equipment myWeapon))
                {
                    int skillId = myWeapon.equipmentData.basicSkill;
                    ESkillType type = Utils.GetSkillTypeFromInt(skillId);

                    switch (supportSkillName)
                    {
                        case ESupportSkillName.ArrowShot:
                        case ESupportSkillName.SavageSmash:
                        case ESupportSkillName.PhotonStrike:
                        case ESupportSkillName.Shuriken:
                        case ESupportSkillName.EgoSword:
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
    #endregion

    #region StageData
    [Serializable]
    public class StageData
    {
        public int stageIndex = 1;
        public string stageName;
        public int stageLevel = 1;
        public string mapName;
        public int stageSkill;

        public int firstWaveCountValue;
        public int firstWaveClearRewardItemId;
        public int firstWaveClearRewardItemValue;

        public int secondWaveCountValue;
        public int secondWaveClearRewardItemId;
        public int secondWaveClearRewardItemValue;

        public int thirdWaveCountValue;
        public int thirdWaveClearRewardItemId;
        public int thirdWaveClearRewardItemValue;

        public int clearRewardGold;
        public int clearRewardExp;
        public string stageImage;
        public List<int> appearingMonsters;
        public List<WaveData> waveArray;
    }
    public class StageDataLoader : ILoader<int, StageData>
    {
        public List<StageData> stages = new List<StageData>();

        public Dictionary<int, StageData> MakeDict()
        {
            Dictionary<int, StageData> dict = new Dictionary<int, StageData>();
            foreach (StageData stage in stages)
                dict.Add(stage.stageIndex, stage);
            return dict;
        }
    }
    #endregion

    #region WaveData
    [System.Serializable]
    public class WaveData
    {
        public int stageIndex = 1;
        public int waveIndex = 1;
        public float spawnInterval = 0.5f;
        public int onceSpawnCount;
        public List<int> monsterId;
        public List<int> eliteId;
        public List<int> bossId;
        public float remainsTime;
        public EWaveType waveType;
        public float firstMonsterSpawnRate;
        public float hpIncreaseRate;
        public float nonDropRate;
        [FormerlySerializedAs("SmallGemDropRate")] public float smallGemDropRate;
        [FormerlySerializedAs("GreenGemDropRate")] public float greenGemDropRate;
        [FormerlySerializedAs("BlueGemDropRate")] public float blueGemDropRate;
        [FormerlySerializedAs("YellowGemDropRate")] public float yellowGemDropRate;
        [FormerlySerializedAs("EliteDropItemId")] public List<int> eliteDropItemId;
    }

    public class WaveDataLoader : ILoader<int, WaveData>
    {
        public List<WaveData> waves = new List<WaveData>();

        public Dictionary<int, WaveData> MakeDict()
        {
            Dictionary<int, WaveData> dict = new Dictionary<int, WaveData>();
            foreach (WaveData wave in waves)
                dict.Add(wave.waveIndex, wave);
            return dict;
        }
    }
    #endregion

    #region EquipmentData
    [Serializable]
    public class EquipmentData
    {
        public string dataId;
        public EGachaRarity gachaRarity;
        public EEquipmentType equipmentType;
        public EEquipmentGrade equipmentGrade;
        public string nameTextID;
        public string descriptionTextID;
        public string spriteName;
        public string hpRegen;
        public int maxHpBonus;
        public int maxHpBonusPerUpgrade;
        public int atkDmgBonus;
        public int atkDmgBonusPerUpgrade;
        public int maxLevel;
        public int uncommonGradeSkill;
        public int rareGradeSkill;
        public int epicGradeSkill;
        public int legendaryGradeSkill;
        public int basicSkill;
        public EMergeEquipmentType mergeEquipmentType1;
        public string mergeEquipment1;
        public EMergeEquipmentType mergeEquipmentType2;
        public string mergeEquipment2;
        public string mergedItemCode;
        public int levelupMaterialID;
        public string downgradeEquipmentCode;
        public string downgradeMaterialCode;
        public int downgradeMaterialCount;
    }

    [Serializable]
    public class EquipmentDataLoader : ILoader<string, EquipmentData>
    {
        public List<EquipmentData> equipments = new List<EquipmentData>();
        public Dictionary<string, EquipmentData> MakeDict()
        {
            Dictionary<string, EquipmentData> dict = new Dictionary<string, EquipmentData>();
            foreach (EquipmentData equip in equipments)
                dict.Add(equip.dataId, equip);
            return dict;
        }
    }
    #endregion

    #region MaterialtData
    [Serializable]
    public class MaterialData
    {
        public int dataId;
        public EMaterialType materialType;
        public EMaterialGrade materialGrade;
        public string nameTextID;
        public string descriptionTextID;
        public string spriteName;
    }

    [Serializable]
    public class MaterialDataLoader : ILoader<int, MaterialData>
    {
        public List<MaterialData> materials = new List<MaterialData>();
        public Dictionary<int, MaterialData> MakeDict()
        {
            Dictionary<int, MaterialData> dict = new Dictionary<int, MaterialData>();
            foreach (MaterialData mat in materials)
                dict.Add(mat.dataId, mat);
            return dict;
        }
    }
    #endregion

    #region LevelData
    [Serializable]
    public class EquipmentLevelData
    {
        public int level;
        public int upgradeCost;
        public int upgradeRequiredItems;
    }

    [Serializable]
    public class EquipmentLevelDataLoader : ILoader<int, EquipmentLevelData>
    {
        public List<EquipmentLevelData> levels = new List<EquipmentLevelData>();
        public Dictionary<int, EquipmentLevelData> MakeDict()
        {
            Dictionary<int, EquipmentLevelData> dict = new Dictionary<int, EquipmentLevelData>();

            foreach (EquipmentLevelData levelData in levels)
                dict.Add(levelData.level, levelData);
            return dict;
        }
    }
    #endregion

    #region DropItemData
    public class DropItemData
    {
        public int dataId;
        public EDropItemType dropItemType;
        public string nameTextID;
        public string descriptionTextID;
        public string spriteName;
    }
    [Serializable]
    public class DropItemDataLoader : ILoader<int, DropItemData>
    {
        public List<DropItemData> dropItems = new List<DropItemData>();
        public Dictionary<int, DropItemData> MakeDict()
        {
            Dictionary<int, DropItemData> dict = new Dictionary<int, DropItemData>();
            foreach (DropItemData dropItem in dropItems)
                dict.Add(dropItem.dataId, dropItem);
            return dict;
        }
    }

    #endregion

    #region GachaData
    public class GachaTableData
    {
        public EGachaType type;
        public List<GachaRateData> gachaRateTable = new List<GachaRateData>();
    }

    [Serializable]
    public class GachaDataLoader : ILoader<Define.EGachaType, GachaTableData>
    {
        public List<GachaTableData> gachaTable = new List<GachaTableData>();
        public Dictionary<Define.EGachaType, GachaTableData> MakeDict()
        {
            Dictionary<Define.EGachaType, GachaTableData> dict = new Dictionary<Define.EGachaType, GachaTableData>();
            foreach (GachaTableData gacha in gachaTable)
                dict.Add(gacha.type, gacha);
            return dict;
        }
    }
    #endregion

    #region GachaRateData
    public class GachaRateData
    {
        public string equipmentID;
        public float gachaRate;
        public EEquipmentGrade equipGrade;
    }
    #endregion

    #region StagePackageData
    public class StagePackageData
    {
        public int stageIndex;
        public int diaValue;
        public int goldValue;
        public int randomScrollValue;
        public int goldKeyValue;
        public int productCostValue;
    }

    [Serializable]
    public class StagePackageDataLoader : ILoader<int, StagePackageData>
    {
        public List<StagePackageData> stagePackages = new List<StagePackageData>();
        public Dictionary<int, StagePackageData> MakeDict()
        {
            Dictionary<int, StagePackageData> dict = new Dictionary<int, StagePackageData>();
            foreach (StagePackageData stp in stagePackages)
                dict.Add(stp.stageIndex, stp);
            return dict;
        }
    }
    #endregion

    #region MissionData
    public class MissionData
    {
        public int missionId;
        public EMissionType missionType;
        public string descriptionTextID;
        public EMissionTarget missionTarget;
        public int missionTargetValue;
        public int clearRewardItmeId;
        public int rewardValue;
    }

    [Serializable]
    public class MissionDataLoader : ILoader<int, MissionData>
    {
        public List<MissionData> missions = new List<MissionData>();
        public Dictionary<int, MissionData> MakeDict()
        {
            Dictionary<int, MissionData> dict = new Dictionary<int, MissionData>();
            foreach (MissionData mis in missions)
                dict.Add(mis.missionId, mis);
            return dict;
        }
    }
    #endregion

    #region AchievementData
    [Serializable]
    public class AchievementData
    {
        public int achievementID;
        public string descriptionTextID;
        public EMissionTarget missionTarget;
        public int missionTargetValue;
        public int clearRewardItemId;
        public int rewardValue;
        public bool isCompleted;
        public bool isRewarded;
        public int progressValue;
    }
    [Serializable]
    public class AchievementDataLoader : ILoader<int, AchievementData>
    {
        public List<AchievementData> achievements = new List<AchievementData>();
        public Dictionary<int, AchievementData> MakeDict()
        {
            Dictionary<int, AchievementData> dict = new Dictionary<int, AchievementData>();
            foreach (AchievementData ach in achievements)
                dict.Add(ach.achievementID, ach);
            return dict;
        }
    }
    #endregion

    #region CheckOutData
    public class CheckOutData
    {
        public int day;
        public int rewardItemId;
        public int missionTargetRewardItemValue;
    }

    [Serializable]
    public class CheckOutDataLoader : ILoader<int, CheckOutData>
    {
        public List<CheckOutData> checkouts = new List<CheckOutData>();
        public Dictionary<int, CheckOutData> MakeDict()
        {
            Dictionary<int, CheckOutData> dict = new Dictionary<int, CheckOutData>();
            foreach (CheckOutData chk in checkouts)
                dict.Add(chk.day, chk);
            return dict;
        }
    }
    #endregion

    #region OfflineRewardData
    public class OfflineRewardData
    {
        public int stageIndex;
        public int reward_Gold;
        public int reward_Exp;
        public int fastReward_Scroll;
        public int fastReward_Box;
    }

    [Serializable]
    public class OfflineRewardDataLoader : ILoader<int, OfflineRewardData>
    {
        public List<OfflineRewardData> offlines = new List<OfflineRewardData>();
        public Dictionary<int, OfflineRewardData> MakeDict()
        {
            Dictionary<int, OfflineRewardData> dict = new Dictionary<int, OfflineRewardData>();
            foreach (OfflineRewardData ofr in offlines)
                dict.Add(ofr.stageIndex, ofr);
            return dict;
        }
    }
    #endregion

    #region BattlePassData
    public class BattlePassData
    {
        public int passLevel;
        public int freeRewardItemId;
        public int freeRewardItemValue;
        public int rareRewardItemId;
        public int rareRewardItemValue;
        public int epicRewardItemId;
        public int epicRewardItemValue;
    }

    [Serializable]
    public class BattlePassDataLoader : ILoader<int, BattlePassData>
    {
        public List<BattlePassData> battles = new List<BattlePassData>();
        public Dictionary<int, BattlePassData> MakeDict()
        {
            Dictionary<int, BattlePassData> dict = new Dictionary<int, BattlePassData>();
            foreach (BattlePassData bts in battles)
                dict.Add(bts.passLevel, bts);
            return dict;
        }
    }
    #endregion

    #region DailyShopData
    public class DailyShopData
    {
        public int index;
        public int buyItemId;
        public int costItemId;
        public int costValue;
        public float discountValue;
    }

    [Serializable]
    public class DailyShopDataLoader : ILoader<int, DailyShopData>
    {
        public List<DailyShopData> dailys = new List<DailyShopData>();
        public Dictionary<int, DailyShopData> MakeDict()
        {
            Dictionary<int, DailyShopData> dict = new Dictionary<int, DailyShopData>();
            foreach (DailyShopData dai in dailys)
                dict.Add(dai.index, dai);
            return dict;
        }
    }
    #endregion

    #region AccountPassData
    public class AccountPassData
    {
        public int accountLevel;
        public int freeRewardItemId;
        public int freeRewardItemValue;
        public int rareRewardItemId;
        public int rareRewardItemValue;
        public int epicRewardItemId;
        public int epicRewardItemValue;
    }

    [Serializable]
    public class AccountPassDataLoader : ILoader<int, AccountPassData>
    {
        public List<AccountPassData> accounts = new List<AccountPassData>();
        public Dictionary<int, AccountPassData> MakeDict()
        {
            Dictionary<int, AccountPassData> dict = new Dictionary<int, AccountPassData>();
            foreach (AccountPassData aps in accounts)
                dict.Add(aps.accountLevel, aps);
            return dict;
        }
    }
    #endregion
}
