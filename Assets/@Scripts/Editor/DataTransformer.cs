using System;
using System.IO;
using System.Linq;
using System.ComponentModel;
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;
using Unity.Plastic.Newtonsoft.Json;

using Data;
using static Define;

public class DataTransformer : EditorWindow
{
  private static T ConvertValue<T>(string value)
  {
    if (string.IsNullOrEmpty(value)) return default(T);

    TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));
    return (T)converter.ConvertFromString(value);
  }
  private static List<T> ConvertList<T>(string value)
  {
    if (string.IsNullOrEmpty(value)) return new List<T>();

    return value.Split('&').Select(x => ConvertValue<T>(x)).ToList();
  }
  
  [MenuItem("Tools/DeleteGameData ")]
  public static void DeleteGameData()
  {
    PlayerPrefs.DeleteAll();
    
    string path = Application.persistentDataPath + "/SaveData.json";
    if (File.Exists(path))
      File.Delete(path);
  }
  
  [MenuItem("Tools/ParseExcel %#K")]
  public static void ParseExcel()
  {
    ParseSkillData("Skill");
    ParseStageData("Stage");
    ParseCreatureData("Creature");
    ParseLevelData("Level");
    ParseEquipmentLevelData("EquipmentLevel");
    ParseEquipmentData("Equipment");
    ParseMaterialData("Material");
    ParseSupportSkillData("SupportSkill");
    ParseDropItemData("DropItem");
    ParseGachaTableData("GachaTable");
    ParseStagePackageData("StagePackage");
    ParseMissionData("Mission");
    ParseAchievementData("Achievement");
    ParseCheckOutData("CheckOut");
    ParseOfflineRewardData("OfflineReward");
    ParseBattlePassData("BattlePass");
    ParseDailyShopData("DailyShop");
    ParseAccountPassDataData("AccountPass");
    Debug.Log("Complete DataTransformer");
  }
  
  private static void ParseSkillData(string filename)
  {
    SkillDataLoader loader = new SkillDataLoader();

    #region Change Excel CSV Data To Skill Data
    string[] lines = File.ReadAllText($"{Application.dataPath}/@Resources/Data/Excel/{filename}Data.csv").Split("\n");
    for (int y = 1; y < lines.Length; y++)
    {
      string[] row = lines[y].Replace("\r", "").Split(',');
      if (row.Length == 0) continue;
       
      if (string.IsNullOrEmpty(row[0])) continue;

      int i = 0;
      SkillData skillData = new SkillData();
      skillData.dataId = ConvertValue<int>(row[i++]);
      skillData.name = ConvertValue<string>(row[i++]);
      skillData.description = ConvertValue<string>(row[i++]);
      skillData.prefabLabel = ConvertValue<string>(row[i++]);
      skillData.iconLabel = ConvertValue<string>(row[i++]);
      skillData.soundLabel = ConvertValue<string>(row[i++]);
      skillData.category = ConvertValue<string>(row[i++]);
      skillData.coolTime = ConvertValue<float>(row[i++]);
      skillData.damageMultiplier = ConvertValue<float>(row[i++]);
      skillData.projectileSpacing = ConvertValue<float>(row[i++]);
      skillData.duration = ConvertValue<float>(row[i++]);
      skillData.recognitionRange = ConvertValue<float>(row[i++]);
      skillData.numProjectiles = ConvertValue<int>(row[i++]);
      skillData.castingSound = ConvertValue<string>(row[i++]);
      skillData.angleBetweenProj = ConvertValue<float>(row[i++]);
      skillData.attackInterval = ConvertValue<float>(row[i++]);
      skillData.numBounce = ConvertValue<int>(row[i++]);
      skillData.bounceSpeed = ConvertValue<float>(row[i++]);
      skillData.bounceDist = ConvertValue<float>(row[i++]);
      skillData.numPenetrations = ConvertValue<int>(row[i++]);
      skillData.castingEffect = ConvertValue<int>(row[i++]);
      skillData.hitSoundLabel = ConvertValue<string>(row[i++]);
      skillData.probCastingEffect = ConvertValue<float>(row[i++]);
      skillData.hitEffect = ConvertValue<int>(row[i++]);
      skillData.probHitEffect = ConvertValue<float>(row[i++]);
      skillData.projRange = ConvertValue<float>(row[i++]);
      skillData.minCoverage = ConvertValue<float>(row[i++]);
      skillData.maxCoverage = ConvertValue<float>(row[i++]);
      skillData.rotateSpeed = ConvertValue<float>(row[i++]);
      skillData.projSpeed = ConvertValue<float>(row[i++]);
      skillData.scaleMultiplier = ConvertValue<float>(row[i++]);
      loader.skills.Add(skillData);
    }
    #endregion

    string jsonStr = JsonConvert.SerializeObject(loader, Formatting.Indented);
    File.WriteAllText($"{Application.dataPath}/@Resources/Data/JsonData/{filename}Data.json", jsonStr);
    AssetDatabase.Refresh();
  }
  private static void ParseSupportSkillData(string filename)
  {
    SupportSkillDataLoader loader = new SupportSkillDataLoader();

    #region Change Excel CSV Data To Support Skill Data
    string[] lines = File.ReadAllText($"{Application.dataPath}/@Resources/Data/Excel/{filename}Data.csv").Split("\n");
    for (int y = 1; y < lines.Length; y++)
    {
      string[] row = lines[y].Replace("\r", "").Split(',');
      if (row.Length == 0) continue;
          
      if (string.IsNullOrEmpty(row[0])) continue;
      
      int i = 0;
      SupportSkillData skillData = new SupportSkillData();
      skillData.dataId = ConvertValue<int>(row[i++]);
      skillData.supportSkillType = ConvertValue<ESupportSkillType>(row[i++]);
      skillData.supportSkillName = ConvertValue<ESupportSkillName>(row[i++]);
      skillData.supportSkillGrade = ConvertValue<ESupportSkillGrade>(row[i++]);
      skillData.name = ConvertValue<string>(row[i++]);
      skillData.description = ConvertValue<string>(row[i++]);
      skillData.iconLabel = ConvertValue<string>(row[i++]);
      skillData.hpRegen = ConvertValue<float>(row[i++]);
      skillData.healRate = ConvertValue<float>(row[i++]);
      skillData.healBonusRate = ConvertValue<float>(row[i++]);
      skillData.magneticRange = ConvertValue<float>(row[i++]);
      skillData.soulAmount = ConvertValue<int>(row[i++]);
      skillData.hpRate = ConvertValue<float>(row[i++]);
      skillData.atkRate = ConvertValue<float>(row[i++]);
      skillData.defRate = ConvertValue<float>(row[i++]);
      skillData.moveSpeedRate = ConvertValue<float>(row[i++]);
      skillData.criRate = ConvertValue<float>(row[i++]);
      skillData.criDmg = ConvertValue<float>(row[i++]);
      skillData.damageReduction = ConvertValue<float>(row[i++]);
      skillData.expBonusRate = ConvertValue<float>(row[i++]);
      skillData.soulBonusRate = ConvertValue<float>(row[i++]);
      skillData.projectileSpacing = ConvertValue<float>(row[i++]);
      skillData.duration = ConvertValue<float>(row[i++]);
      skillData.numProjectiles = ConvertValue<int>(row[i++]);
      skillData.attackInterval = ConvertValue<float>(row[i++]);
      skillData.numBounce = ConvertValue<int>(row[i++]);
      skillData.numPenetrations = ConvertValue<int>(row[i++]);
      skillData.projRange = ConvertValue<float>(row[i++]);
      skillData.rotateSpeed = ConvertValue<float>(row[i++]);
      skillData.scaleMultiplier = ConvertValue<float>(row[i++]);
      skillData.price = ConvertValue<float>(row[i++]);
      loader.supportSkills.Add(skillData);
    }
    #endregion

    string jsonStr = JsonConvert.SerializeObject(loader, Formatting.Indented);
    File.WriteAllText($"{Application.dataPath}/@Resources/Data/JsonData/{filename}Data.json", jsonStr);
    AssetDatabase.Refresh();
  }
  private static void ParseStageData(string filename)
  {
    Dictionary<int, List<WaveData>> waveTable = ParseWaveData("Wave");
    StageDataLoader loader = new StageDataLoader();

    #region Change Excel CSV Data To Stage Data
    string[] lines = File.ReadAllText($"{Application.dataPath}/@Resources/Data/Excel/{filename}Data.csv").Split("\n");
    for (int y = 1; y < lines.Length; y++)
    {
      string[] row = lines[y].Replace("\r", "").Split(',');
      if (row.Length == 0) continue;
         
      if (string.IsNullOrEmpty(row[0])) continue;

      int i = 0;
      StageData stageData = new StageData();
      stageData.stageIndex = ConvertValue<int>(row[i++]);
      stageData.stageName = ConvertValue<string>(row[i++]);
      stageData.stageLevel = ConvertValue<int>(row[i++]);
      stageData.mapName = ConvertValue<string>(row[i++]);
      stageData.stageSkill = ConvertValue<int>(row[i++]);
      stageData.firstWaveCountValue = ConvertValue<int>(row[i++]);
      stageData.firstWaveClearRewardItemId = ConvertValue<int>(row[i++]);
      stageData.firstWaveClearRewardItemValue = ConvertValue<int>(row[i++]);

      stageData.secondWaveCountValue = ConvertValue<int>(row[i++]);
      stageData.secondWaveClearRewardItemId = ConvertValue<int>(row[i++]);
      stageData.secondWaveClearRewardItemValue = ConvertValue<int>(row[i++]);

      stageData.thirdWaveCountValue = ConvertValue<int>(row[i++]);
      stageData.thirdWaveClearRewardItemId = ConvertValue<int>(row[i++]);
      stageData.thirdWaveClearRewardItemValue = ConvertValue<int>(row[i++]);

      stageData.clearRewardGold = ConvertValue<int>(row[i++]);
      stageData.clearRewardExp = ConvertValue<int>(row[i++]);
      stageData.stageImage = ConvertValue<string>(row[i++]);
      stageData.appearingMonsters = ConvertList<int>(row[i++]);
      waveTable.TryGetValue(stageData.stageIndex, out stageData.waveArray);

      loader.stages.Add(stageData);
    }
    #endregion

    string jsonStr = JsonConvert.SerializeObject(loader, Formatting.Indented);
    File.WriteAllText($"{Application.dataPath}/@Resources/Data/JsonData/{filename}Data.json", jsonStr);
    AssetDatabase.Refresh();
  }
  private static Dictionary<int, List<WaveData>> ParseWaveData(string filename)
  {
    Dictionary<int, List<WaveData>> waveTable = new Dictionary<int, List<WaveData>>();

    #region Change Excel CSV Data To Wave Data
    string[] lines = File.ReadAllText($"{Application.dataPath}/@Resources/Data/Excel/{filename}Data.csv").Split("\n");
    for (int y = 1; y < lines.Length; y++)
    {
      string[] row = lines[y].Replace("\r", "").Split(',');
      if (row.Length == 0) continue;

      if (string.IsNullOrEmpty(row[0])) continue;

      int i = 0;
      WaveData waveData = new WaveData();
      waveData.stageIndex = ConvertValue<int>(row[i++]);
      waveData.waveIndex = ConvertValue<int>(row[i++]);
      waveData.spawnInterval = ConvertValue<float>(row[i++]);
      waveData.onceSpawnCount = ConvertValue<int>(row[i++]);
      waveData.monsterId = ConvertList<int>(row[i++]);
      waveData.eliteId = ConvertList<int>(row[i++]);
      waveData.bossId = ConvertList<int>(row[i++]);
      waveData.remainsTime = ConvertValue<float>(row[i++]);
      waveData.waveType = ConvertValue<EWaveType>(row[i++]);
      waveData.firstMonsterSpawnRate = ConvertValue<float>(row[i++]);
      waveData.hpIncreaseRate = ConvertValue<float>(row[i++]);
      waveData.nonDropRate = ConvertValue<float>(row[i++]);
      waveData.smallGemDropRate = ConvertValue<float>(row[i++]);
      waveData.greenGemDropRate = ConvertValue<float>(row[i++]);
      waveData.blueGemDropRate = ConvertValue<float>(row[i++]);
      waveData.yellowGemDropRate = ConvertValue<float>(row[i++]);
      waveData.eliteDropItemId = ConvertList<int>(row[i++]);

      if (waveTable.ContainsKey(waveData.stageIndex) == false)
          waveTable.Add(waveData.stageIndex, new List<WaveData>());

      waveTable[waveData.stageIndex].Add(waveData);
    }
    #endregion
    return waveTable;
  }
  private static void ParseCreatureData(string filename)
  {
    CreatureDataLoader loader = new CreatureDataLoader();

    #region Change Excel CSV Data To Creature Data
    string[] lines = File.ReadAllText($"{Application.dataPath}/@Resources/Data/Excel/{filename}Data.csv").Split("\n");
    for (int y = 1; y < lines.Length; y++)
    {
      string[] row = lines[y].Replace("\r", "").Split(',');

      if (row.Length == 0) continue;
           
      if (string.IsNullOrEmpty(row[0])) continue;

      int i = 0;
      CreatureData cd = new CreatureData();
      cd.dataId = ConvertValue<int>(row[i++]);
      cd.descriptionTextID = ConvertValue<string>(row[i++]);
      cd.prefabLabel = ConvertValue<string>(row[i++]);
      cd.maxHp = ConvertValue<float>(row[i++]);
      cd.maxHpBonus = ConvertValue<float>(row[i++]);
      cd.atk = ConvertValue<float>(row[i++]);
      cd.atkBonus = ConvertValue<float>(row[i++]);
      cd.def = ConvertValue<float>(row[i++]);
      cd.moveSpeed = ConvertValue<float>(row[i++]);
      cd.totalExp = ConvertValue<float>(row[i++]);
      cd.hpRate = ConvertValue<float>(row[i++]);
      cd.atkRate = ConvertValue<float>(row[i++]);
      cd.defRate = ConvertValue<float>(row[i++]);
      cd.moveSpeedRate = ConvertValue<float>(row[i++]);
      cd.iconLabel = ConvertValue<string>(row[i++]);
      cd.skillTypeList = ConvertList<int>(row[i++]);
      loader.creatures.Add(cd);
    }
    #endregion

    string jsonStr = JsonConvert.SerializeObject(loader, Formatting.Indented);
    File.WriteAllText($"{Application.dataPath}/@Resources/Data/JsonData/{filename}Data.json", jsonStr);
    AssetDatabase.Refresh();
  }
  private static void ParseLevelData(string filename)
  {
    LevelDataLoader loader = new LevelDataLoader();

    #region Change Excel CSV Data To Level Data
    string[] lines = File.ReadAllText($"{Application.dataPath}/@Resources/Data/Excel/{filename}Data.csv").Split("\n");
    for (int y = 1; y < lines.Length; y++)
    {
      string[] row = lines[y].Replace("\r", "").Split(',');
      if (row.Length == 0)  continue;
      
      if (string.IsNullOrEmpty(row[0])) continue;
       
      int i = 0;
      LevelData data = new LevelData();
      data.level = ConvertValue<int>(row[i++]);
      data.totalExp = ConvertValue<int>(row[i++]);
      loader.levels.Add(data);
    }
    #endregion

    string jsonStr = JsonConvert.SerializeObject(loader, Formatting.Indented);
    File.WriteAllText($"{Application.dataPath}/@Resources/Data/JsonData/{filename}Data.json", jsonStr);
    AssetDatabase.Refresh();
  }
  private static void ParseEquipmentLevelData(string filename)
  {
    EquipmentLevelDataLoader loader = new EquipmentLevelDataLoader();

    #region Change Excel CSV Data To Equipment Level Data
    string[] lines = File.ReadAllText($"{Application.dataPath}/@Resources/Data/Excel/{filename}Data.csv").Split("\n");
    for (int y = 1; y < lines.Length; y++)
    {
      string[] row = lines[y].Replace("\r", "").Split(',');
      if (row.Length == 0) continue;
   
      if (string.IsNullOrEmpty(row[0])) continue;

      int i = 0;
      EquipmentLevelData data = new EquipmentLevelData();
      data.level = ConvertValue<int>(row[i++]);
      data.upgradeCost = ConvertValue<int>(row[i++]);
      data.upgradeRequiredItems = ConvertValue<int>(row[i++]);
      loader.levels.Add(data);
    }
    #endregion

    string jsonStr = JsonConvert.SerializeObject(loader, Formatting.Indented);
    File.WriteAllText($"{Application.dataPath}/@Resources/Data/JsonData/{filename}Data.json", jsonStr);
    AssetDatabase.Refresh();
  }
  private static void ParseEquipmentData(string filename)
  {
    EquipmentDataLoader loader = new EquipmentDataLoader();

    #region Change Excel CSV Data To Equipment Data
    string[] lines = File.ReadAllText($"{Application.dataPath}/@Resources/Data/Excel/{filename}Data.csv").Split("\n");
    for (int y = 1; y < lines.Length; y++)
    {
      string[] row = lines[y].Replace("\r", "").Split(',');
      if (row.Length == 0) continue;
     
      if (string.IsNullOrEmpty(row[0])) continue;

      int i = 0;
      EquipmentData ed = new EquipmentData();
      ed.dataId = ConvertValue<string>(row[i++]);
      ed.gachaRarity = ConvertValue<EGachaRarity>(row[i++]);
      ed.equipmentType = ConvertValue<EEquipmentType>(row[i++]);
      ed.equipmentGrade = ConvertValue<EEquipmentGrade>(row[i++]);
      ed.nameTextID = ConvertValue<string>(row[i++]);
      ed.descriptionTextID = ConvertValue<string>(row[i++]);
      ed.spriteName = ConvertValue<string>(row[i++]);
      ed.maxHpBonus = ConvertValue<int>(row[i++]);
      ed.maxHpBonusPerUpgrade = ConvertValue<int>(row[i++]);
      ed.atkDmgBonus = ConvertValue<int>(row[i++]);
      ed.atkDmgBonusPerUpgrade = ConvertValue<int>(row[i++]);
      ed.maxLevel = ConvertValue<int>(row[i++]);
      ed.uncommonGradeSkill = ConvertValue<int>(row[i++]);
      ed.rareGradeSkill = ConvertValue<int>(row[i++]);
      ed.epicGradeSkill = ConvertValue<int>(row[i++]);
      ed.legendaryGradeSkill = ConvertValue<int>(row[i++]);
      ed.basicSkill = ConvertValue<int>(row[i++]);
      ed.mergeEquipmentType1 = ConvertValue<EMergeEquipmentType>(row[i++]);
      ed.mergeEquipment1 = row[i++];
      ed.mergeEquipmentType2 = ConvertValue<EMergeEquipmentType>(row[i++]);
      ed.mergeEquipment2 = row[i++];
      ed.mergedItemCode = row[i++];
      ed.levelupMaterialID = ConvertValue<int>(row[i++]);
      ed.downgradeEquipmentCode = ConvertValue<string>(row[i++]);
      ed.downgradeMaterialCode = ConvertValue<string>(row[i++]);
      ed.downgradeMaterialCount = ConvertValue<int>(row[i++]);
      loader.equipments.Add(ed);
    }
    #endregion

    string jsonStr = JsonConvert.SerializeObject(loader, Formatting.Indented);
    File.WriteAllText($"{Application.dataPath}/@Resources/Data/JsonData/{filename}Data.json", jsonStr);
    AssetDatabase.Refresh();
  }
  private static void ParseMaterialData(string filename)
  {
    MaterialDataLoader loader = new MaterialDataLoader();

    #region Change Excel CSV Data To Material Data
    string[] lines = File.ReadAllText($"{Application.dataPath}/@Resources/Data/Excel/{filename}Data.csv").Split("\n");
    for (int y = 1; y < lines.Length; y++)
    {
      string[] row = lines[y].Replace("\r", "").Split(',');
      if (row.Length == 0) continue;

      if (string.IsNullOrEmpty(row[0])) continue;

      int i = 0;
      MaterialData material = new MaterialData();
      material.dataId = ConvertValue<int>(row[i++]);
      material.materialType = ConvertValue<EMaterialType>(row[i++]);
      material.materialGrade = ConvertValue<EMaterialGrade>(row[i++]);
      material.nameTextID = ConvertValue<string>(row[i++]);
      material.descriptionTextID = ConvertValue<string>(row[i++]);
      material.spriteName = ConvertValue<string>(row[i++]);
      loader.materials.Add(material);
    }
    #endregion

    string jsonStr = JsonConvert.SerializeObject(loader, Formatting.Indented);
    File.WriteAllText($"{Application.dataPath}/@Resources/Data/JsonData/{filename}Data.json", jsonStr);
    AssetDatabase.Refresh();
  }
  private static void ParseDropItemData(string filename)
  {
    DropItemDataLoader loader = new DropItemDataLoader();

    #region Change Excel CSV Data To DropItem Data
    string[] lines = File.ReadAllText($"{Application.dataPath}/@Resources/Data/Excel/{filename}Data.csv").Split("\n");
    for (int y = 1; y < lines.Length; y++)
    {
      string[] row = lines[y].Replace("\r", "").Split(',');
      if (row.Length == 0) continue;
      
      if (string.IsNullOrEmpty(row[0])) continue;
  
      int i = 0;
      DropItemData dropItem = new DropItemData();
      dropItem.dataId = ConvertValue<int>(row[i++]);
      dropItem.dropItemType = ConvertValue<EDropItemType>(row[i++]);
      dropItem.nameTextID = ConvertValue<string>(row[i++]);
      dropItem.descriptionTextID = ConvertValue<string>(row[i++]);
      dropItem.spriteName = ConvertValue<string>(row[i++]);
      loader.dropItems.Add(dropItem);
    }
    #endregion

    string jsonStr = JsonConvert.SerializeObject(loader, Formatting.Indented);
    File.WriteAllText($"{Application.dataPath}/@Resources/Data/JsonData/{filename}Data.json", jsonStr);
    AssetDatabase.Refresh();
  }
  private static void ParseGachaTableData(string filename)
  {
    Dictionary<EGachaType, List<GachaRateData>> gachaTable = ParseGachaRateData("GachaTable");
    GachaTableDataLoader loader = new GachaTableDataLoader();

    #region Change Excel CSV Data To GachaTable Data
    // string[] lines = File.ReadAllText($"{Application.dataPath}/@Resources/Data/Excel/{filename}Data.csv").Split("\n");
    // for (int y = 1; y < lines.Length; y++)
    // {
    //   string[] row = lines[y].Replace("\r", "").Split(',');
    //   if (row.Length == 0) continue;
    //       
    //   if (string.IsNullOrEmpty(row[0])) continue;
    //
    //   int i = 0;
    //   GachaData gacha = new GachaData();
    //   gacha.DropItemType = ConvertValue<Define.DropItemType>(row[i++]);
    //
    //   loader.Gachas.Add(gacha);
    // }
    for (int i = 0; i < gachaTable.Count+1; i++)
    {
      GachaTableData gachaData = new GachaTableData()
      {
        type = (EGachaType)i,
      };
      if (gachaTable.TryGetValue(gachaData.type, out List<GachaRateData> gachaRate))
        gachaData.gachaRateTable.AddRange(gachaRate);

      loader.gachaTable.Add(gachaData);
    }
    #endregion

    string jsonStr = JsonConvert.SerializeObject(loader, Formatting.Indented);
    File.WriteAllText($"{Application.dataPath}/@Resources/Data/JsonData/{filename}Data.json", jsonStr);
    AssetDatabase.Refresh();
  }
  private static Dictionary<EGachaType, List<GachaRateData>> ParseGachaRateData(string filename)
  {
    Dictionary<EGachaType, List<GachaRateData>> gachaTable = new Dictionary<EGachaType, List<GachaRateData>>();

    #region Change Excel CSV Data To Gacha Rate Data
    string[] lines = File.ReadAllText($"{Application.dataPath}/@Resources/Data/Excel/{filename}Data.csv").Split("\n");
    for(int y=1; y<lines.Length; y++)
    {
      string[] row = lines[y].Replace("\r", "").Split(',');
      if (row.Length == 0) continue;

      if (string.IsNullOrEmpty(row[0])) continue;

      int i = 0;
      EGachaType dropType = (EGachaType)Enum.Parse(typeof(EGachaType), row[i++]);
      GachaRateData rateData = new GachaRateData()
      {
        equipmentID = row[i++],
        gachaRate = float.Parse(row[i++]),
        equipGrade = ConvertValue<EEquipmentGrade>(row[i++]),
      };

      if (gachaTable.ContainsKey(dropType) == false)
        gachaTable.Add(dropType, new List<GachaRateData>());

      gachaTable[dropType].Add(rateData);
    }
    #endregion

    return gachaTable;
  }
  private static void ParseStagePackageData(string filename)
  {
    StagePackageDataLoader loader = new StagePackageDataLoader();

    #region Change Excel CSV Data To Stage Package Data
    string[] lines = File.ReadAllText($"{Application.dataPath}/@Resources/Data/Excel/{filename}Data.csv").Split("\n");
    for (int y = 1; y < lines.Length; y++)
    {
      string[] row = lines[y].Replace("\r", "").Split(',');
      if (row.Length == 0) continue;
       
      if (string.IsNullOrEmpty(row[0])) continue;

      int i = 0;
      StagePackageData stp = new StagePackageData();
      stp.stageIndex = ConvertValue<int>(row[i++]);
      stp.diaValue = ConvertValue<int>(row[i++]);
      stp.goldValue = ConvertValue<int>(row[i++]);
      stp.randomScrollValue = ConvertValue<int>(row[i++]);
      stp.goldKeyValue = ConvertValue<int>(row[i++]);
      stp.productCostValue = ConvertValue<int>(row[i++]);
      loader.stagePackages.Add(stp);
    }
    #endregion

    string jsonStr = JsonConvert.SerializeObject(loader, Formatting.Indented);
    File.WriteAllText($"{Application.dataPath}/@Resources/Data/JsonData/{filename}Data.json", jsonStr);
    AssetDatabase.Refresh();
  }
  private static void ParseMissionData(string filename)
  {
    MissionDataLoader loader = new MissionDataLoader();

    #region Change Excel CSV Data To Mission Data
    string[] lines = File.ReadAllText($"{Application.dataPath}/@Resources/Data/Excel/{filename}Data.csv").Split("\n");
    for (int y = 1; y < lines.Length; y++)
    {
      string[] row = lines[y].Replace("\r", "").Split(',');
      if (row.Length == 0) continue;
      
      if (string.IsNullOrEmpty(row[0])) continue;

      int i = 0;
      MissionData missionData = new MissionData();
      missionData.missionId = ConvertValue<int>(row[i++]);
      missionData.missionType = ConvertValue<EMissionType>(row[i++]);
      missionData.descriptionTextID = ConvertValue<string>(row[i++]);
      missionData.missionTarget = ConvertValue<EMissionTarget>(row[i++]);
      missionData.missionTargetValue = ConvertValue<int>(row[i++]);
      missionData.clearRewardItmeId = ConvertValue<int>(row[i++]);
      missionData.rewardValue = ConvertValue<int>(row[i++]);
      loader.missions.Add(missionData);
    }
    #endregion

    string jsonStr = JsonConvert.SerializeObject(loader, Formatting.Indented);
    File.WriteAllText($"{Application.dataPath}/@Resources/Data/JsonData/{filename}Data.json", jsonStr);
    AssetDatabase.Refresh();
  }
  private static void ParseAchievementData(string filename)
  {
    AchievementDataLoader loader = new AchievementDataLoader();

    #region Change Excel CSV Data To Achievement Data
    string[] lines = File.ReadAllText($"{Application.dataPath}/@Resources/Data/Excel/{filename}Data.csv").Split("\n");
    for (int y = 1; y < lines.Length; y++)
    {
      string[] row = lines[y].Replace("\r", "").Split(',');
      if (row.Length == 0) continue;
      
      if (string.IsNullOrEmpty(row[0])) continue;

      int i = 0;
      AchievementData ach = new AchievementData();
      ach.achievementID = ConvertValue<int>(row[i++]);
      ach.descriptionTextID = ConvertValue<string>(row[i++]);
      ach.missionTarget = ConvertValue<EMissionTarget>(row[i++]);
      ach.missionTargetValue = ConvertValue<int>(row[i++]);
      ach.clearRewardItemId = ConvertValue<int>(row[i++]);
      ach.rewardValue = ConvertValue<int>(row[i++]);
      loader.achievements.Add(ach);
    }
    #endregion

    string jsonStr = JsonConvert.SerializeObject(loader, Formatting.Indented);
    File.WriteAllText($"{Application.dataPath}/@Resources/Data/JsonData/{filename}Data.json", jsonStr);
    AssetDatabase.Refresh();
  }
  private static void ParseCheckOutData(string filename)
  {
    CheckOutDataLoader loader = new CheckOutDataLoader();

    #region Change Excel CSV Data To CheckOut Data
    string[] lines = File.ReadAllText($"{Application.dataPath}/@Resources/Data/Excel/{filename}Data.csv").Split("\n");
    for (int y = 1; y < lines.Length; y++)
    {
      string[] row = lines[y].Replace("\r", "").Split(',');
      if (row.Length == 0) continue;
  
      if (string.IsNullOrEmpty(row[0])) continue;

      int i = 0;
      CheckOutData chk = new CheckOutData();
      chk.day = ConvertValue<int>(row[i++]);
      chk.rewardItemId = ConvertValue<int>(row[i++]);
      chk.missionTargetRewardItemValue = ConvertValue<int>(row[i++]);
      loader.checkouts.Add(chk);
    }
    #endregion

    string jsonStr = JsonConvert.SerializeObject(loader, Formatting.Indented);
    File.WriteAllText($"{Application.dataPath}/@Resources/Data/JsonData/{filename}Data.json", jsonStr);
    AssetDatabase.Refresh();
  }
  private static void ParseOfflineRewardData(string filename)
  {
    OfflineRewardDataLoader loader = new OfflineRewardDataLoader();

    #region Change Excel CSV Data To Offline Reward Data
    string[] lines = File.ReadAllText($"{Application.dataPath}/@Resources/Data/Excel/{filename}Data.csv").Split("\n");
    for (int y = 1; y < lines.Length; y++)
    {
      string[] row = lines[y].Replace("\r", "").Split(',');
      if (row.Length == 0) continue;
  
      if (string.IsNullOrEmpty(row[0])) continue;
    
      int i = 0;
      OfflineRewardData ofr = new OfflineRewardData();
      ofr.stageIndex = ConvertValue<int>(row[i++]);
      ofr.reward_Gold = ConvertValue<int>(row[i++]);
      ofr.reward_Exp = ConvertValue<int>(row[i++]);
      ofr.fastReward_Scroll = ConvertValue<int>(row[i++]);
      ofr.fastReward_Box = ConvertValue<int>(row[i++]);
      loader.offlines.Add(ofr);
    }
    #endregion

    string jsonStr = JsonConvert.SerializeObject(loader, Formatting.Indented);
    File.WriteAllText($"{Application.dataPath}/@Resources/Data/JsonData/{filename}Data.json", jsonStr);
    AssetDatabase.Refresh();
  }
  private static void ParseBattlePassData(string filename)
  {
    BattlePassDataLoader loader = new BattlePassDataLoader();

    #region Change Excel CSV Data To Battle Pass Data
    string[] lines = File.ReadAllText($"{Application.dataPath}/@Resources/Data/Excel/{filename}Data.csv").Split("\n");
    for (int y = 1; y < lines.Length; y++)
    {
      string[] row = lines[y].Replace("\r", "").Split(',');
      if (row.Length == 0) continue;
  
      if (string.IsNullOrEmpty(row[0])) continue;
    
      int i = 0;
      BattlePassData bts = new BattlePassData();
      bts.passLevel = ConvertValue<int>(row[i++]);
      bts.freeRewardItemId = ConvertValue<int>(row[i++]);
      bts.freeRewardItemValue = ConvertValue<int>(row[i++]);
      bts.rareRewardItemId = ConvertValue<int>(row[i++]);
      bts.rareRewardItemValue = ConvertValue<int>(row[i++]);
      bts.epicRewardItemId = ConvertValue<int>(row[i++]);
      bts.epicRewardItemValue = ConvertValue<int>(row[i++]);
      loader.battles.Add(bts);
    }
    #endregion

    string jsonStr = JsonConvert.SerializeObject(loader, Formatting.Indented);
    File.WriteAllText($"{Application.dataPath}/@Resources/Data/JsonData/{filename}Data.json", jsonStr);
    AssetDatabase.Refresh();
  }
  private static void ParseDailyShopData(string filename)
  {
    DailyShopDataLoader loader = new DailyShopDataLoader();

    #region Change Excel CSV Data To Daily Shop Data
    string[] lines = File.ReadAllText($"{Application.dataPath}/@Resources/Data/Excel/{filename}Data.csv").Split("\n");
    for (int y = 1; y < lines.Length; y++)
    {
      string[] row = lines[y].Replace("\r", "").Split(',');
      if (row.Length == 0) continue;
        
      if (string.IsNullOrEmpty(row[0])) continue;

      int i = 0;
      DailyShopData dai = new DailyShopData();
      dai.index = ConvertValue<int>(row[i++]);
      dai.buyItemId = ConvertValue<int>(row[i++]);
      dai.costItemId = ConvertValue<int>(row[i++]);
      dai.costValue = ConvertValue<int>(row[i++]);
      dai.discountValue = ConvertValue<float>(row[i++]);
      loader.dailys.Add(dai);
    }
    #endregion

    string jsonStr = JsonConvert.SerializeObject(loader, Formatting.Indented);
    File.WriteAllText($"{Application.dataPath}/@Resources/Data/JsonData/{filename}Data.json", jsonStr);
    AssetDatabase.Refresh();
  }
  private static void ParseAccountPassDataData(string filename)
  {
    AccountPassDataLoader loader = new AccountPassDataLoader();

    #region Change Excel CSV Data To Account Pass Data
    string[] lines = File.ReadAllText($"{Application.dataPath}/@Resources/Data/Excel/{filename}Data.csv").Split("\n");
    for (int y = 1; y < lines.Length; y++)
    {
      string[] row = lines[y].Replace("\r", "").Split(',');
      if (row.Length == 0) continue;
      
      if (string.IsNullOrEmpty(row[0])) continue;

      int i = 0;
      AccountPassData aps = new AccountPassData();
      aps.accountLevel = ConvertValue<int>(row[i++]);
      aps.freeRewardItemId = ConvertValue<int>(row[i++]);
      aps.freeRewardItemValue = ConvertValue<int>(row[i++]);
      aps.rareRewardItemId = ConvertValue<int>(row[i++]);
      aps.rareRewardItemValue = ConvertValue<int>(row[i++]);
      aps.epicRewardItemId = ConvertValue<int>(row[i++]);
      aps.epicRewardItemValue = ConvertValue<int>(row[i++]);
      loader.accounts.Add(aps);
    }
    #endregion

    string jsonStr = JsonConvert.SerializeObject(loader, Formatting.Indented);
    File.WriteAllText($"{Application.dataPath}/@Resources/Data/JsonData/{filename}Data.json", jsonStr);
    AssetDatabase.Refresh();
  }
}
