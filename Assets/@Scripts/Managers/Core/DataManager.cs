using System.Collections.Generic;

using UnityEngine;
using Newtonsoft.Json;

using Data;

public interface ILoader<Key, Value>
{
  public Dictionary<Key, Value> MakeDict();
}

public class DataManager
{
  public Dictionary<int, MaterialData> MaterialDic { get; private set; } = new Dictionary<int, MaterialData>();
  public Dictionary<int, SupportSkillData> SupportSkillDic { get; private set; } = new Dictionary<int, SupportSkillData>();
  public Dictionary<int, StageData> StageDic { get; private set; } = new Dictionary<int, StageData>();
  public Dictionary<int, SkillData> SkillDic { get; private set; } = new Dictionary<int, SkillData>();
  public Dictionary<int, CreatureData> CreatureDic { get; private set; } = new Dictionary<int, CreatureData>();
  public Dictionary<int, LevelData> LevelDataDic { get; private set; } = new Dictionary<int, LevelData>();
  public Dictionary<string, EquipmentData> EquipDataDic { get; private set; } = new Dictionary<string, EquipmentData>();
  public Dictionary<int, EquipmentLevelData> EquipLevelDataDic { get; private set; } = new Dictionary<int, EquipmentLevelData>();
  public Dictionary<Define.EGachaType, GachaTableData> GachaTableDataDic { get; private set; } = new Dictionary<Define.EGachaType, GachaTableData>();
  public Dictionary<int, MissionData> MissionDataDic { get; private set; } = new Dictionary<int, MissionData>();
  public Dictionary<int, AchievementData> AchievementDataDic { get; private set; } = new Dictionary<int, AchievementData>();
  public Dictionary<int, DropItemData> DropItemDataDic { get; private set; } = new Dictionary<int, DropItemData>();
  public Dictionary<int, CheckOutData> CheckOutDataDic { get; private set; } = new Dictionary<int, CheckOutData>();
  public Dictionary<int, OfflineRewardData> OfflineRewardDataDic { get; private set; } = new Dictionary<int, OfflineRewardData>();

  public void Init()
  {
    MaterialDic = LoadJson<MaterialDataLoader, int, MaterialData>("MaterialData").MakeDict();
    SupportSkillDic = LoadJson<SupportSkillDataLoader, int, SupportSkillData>("SupportSkillData").MakeDict();
    StageDic = LoadJson<StageDataLoader, int, StageData>("StageData").MakeDict();
    CreatureDic = LoadJson<CreatureDataLoader, int, CreatureData>("CreatureData").MakeDict();
    SkillDic = LoadJson<SkillDataLoader, int, SkillData>("SkillData").MakeDict();
    LevelDataDic = LoadJson<LevelDataLoader, int, LevelData>("LevelData").MakeDict();
    EquipDataDic = LoadJson<EquipmentDataLoader, string, EquipmentData>("EquipmentData").MakeDict();
    EquipLevelDataDic = LoadJson<EquipmentLevelDataLoader, int, EquipmentLevelData>("EquipmentLevelData").MakeDict();
    GachaTableDataDic = LoadJson<GachaDataLoader, Define.EGachaType, GachaTableData>("GachaTableData").MakeDict();
    MissionDataDic = LoadJson<MissionDataLoader, int, MissionData>("MissionData").MakeDict();
    AchievementDataDic = LoadJson<AchievementDataLoader, int, AchievementData>("AchievementData").MakeDict();
    DropItemDataDic = LoadJson<DropItemDataLoader, int, DropItemData>("DropItemData").MakeDict();
    CheckOutDataDic = LoadJson<CheckOutDataLoader, int, CheckOutData>("CheckOutData").MakeDict();
    OfflineRewardDataDic = LoadJson<OfflineRewardDataLoader, int, OfflineRewardData>("OfflineRewardData").MakeDict();
  }

  Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
  {
    TextAsset textAsset = Managers.Resource.Load<TextAsset>($"{path}");
    
    return JsonConvert.DeserializeObject<Loader>(textAsset.text);
  }
}