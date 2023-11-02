using System;
using System.Collections.Generic;
using System.IO;
using Data;
using Newtonsoft.Json;
using UnityEngine;
using static Define;

[Serializable]
public class StageClearInfo
{
  public int stageIndex = 1;
  public int maxWaveIndex = 0;
  public bool isOpenFirstBox = false;
  public bool isOpenSecondBox = false;
  public bool isOpenThirdBox = false;
  public bool isClear = false;
}

[Serializable]
public class MissionInfo
{
  public int progress;
  public bool isRewarded;
}

[Serializable]
public class GameData
{
  public int userLevel = 1;
  public string userName = "Player";

  public int stamina = Define.MAX_STAMINA;
  public int gold = 0;
  public int dia = 0;

  #region Achievements
  public int commonGachaOpenCount = 0;
  public int advancedGachaOpenCount = 0;
  public int fastRewardCount = 0;
  public int offlineRewardCount = 0;
  public int totalMonsterKillCount = 0;
  public int totalEliteKillCount = 0;
  public int totalBossKillCount = 0;
  public List<AchievementData> achievements = new List<AchievementData>(); // 업적 목록
  #endregion
  
  #region Refresh Per Day
  public int gachaCountAdsAnvanced = 1;
  public int gachaCountAdsCommon = 1;
  public int goldCountAds = 1;
  public int rebirthCountAds = 3;
  public int diaCountAds = 3;
  public int staminaCountAds = 1;
  public int fastRewardCountAds = 1;
  public int fastRewardCountStamina = 3;
  public int skillRefreshCountAds = 3;
  public int remainsStaminaByDia = 3;
  public int bronzeKeyCountAds = 1;
  #endregion
  
  public bool[] attendanceReceived = new bool[30];
  public bool BGMOn = true;
  public bool effectSoundOn = true;
  public Define.EJoystickType joystickType = Define.EJoystickType.Flexible;
  public List<Character> characters = new List<Character>();
  public List<Equipment> ownedEquipments = new List<Equipment>();
  public ContinueData continueInfo = new ContinueData();
  public StageData currentStage = new StageData();
  public Dictionary<int, int> itemDictionary = new Dictionary<int, int>();//<ID, 갯수>
  public Dictionary<EEquipmentType, Equipment> equippedEquipments = new Dictionary<EEquipmentType, Equipment>();
  public Dictionary <int, StageClearInfo> dicStageClearInfo = new Dictionary<int, StageClearInfo>();
  public Dictionary<EMissionTarget, MissionInfo> dicMission = new Dictionary<EMissionTarget, MissionInfo>()
  {
    {EMissionTarget.StageEnter, new MissionInfo() { progress = 0, isRewarded = false }},
    {EMissionTarget.StageClear, new MissionInfo() { progress = 0, isRewarded = false }},
    {EMissionTarget.EquipmentLevelUp, new MissionInfo() { progress = 0, isRewarded = false }},
    {EMissionTarget.OfflineRewardGet, new MissionInfo() { progress = 0, isRewarded = false }},
    {EMissionTarget.EquipmentMerge, new MissionInfo() { progress = 0, isRewarded = false }},
    {EMissionTarget.MonsterKill, new MissionInfo() { progress = 0, isRewarded = false }},
    {EMissionTarget.EliteMonsterKill, new MissionInfo() { progress = 0, isRewarded = false }},
    {EMissionTarget.GachaOpen, new MissionInfo() { progress = 0, isRewarded = false }},
    {EMissionTarget.ADWatchIng, new MissionInfo() { progress = 0, isRewarded = false }},
  };
}

[Serializable]
public class ContinueData
{
  public int playerDataId;
  public float hp;
  public float maxHp;
  public float maxHpBonusRate = 1;
  public float healBonusRate = 1;
  public float hpRegen;
  public float atk;
  public float attackRate = 1;
  public float def;
  public float defRate;
  public float moveSpeed;
  public float moveSpeedRate = 1;
  public float totalExp;
  public int level = 1;
  public float exp;
  public float criRate;
  public float criDamage = 1.5f;
  public float damageReduction;
  public float expBonusRate = 1;
  public float soulBonusRate = 1;
  public float collectDistBonus = 1;
  public int killCount;
  public int skillRefreshCount = 3;
  public float soulCount;

  public List<SupportSkillData> soulShopList = new List<SupportSkillData>();
  public List<SupportSkillData> savedSupportSkill = new List<SupportSkillData>();
  public Dictionary<ESkillType, int> savedBattleSkill = new Dictionary<ESkillType, int>();

  public int waveIndex;
  
  // Property
  public bool IsContinue { get { return savedBattleSkill.Count > 0; } }
  
  public void Clear()
  {
    // 각 변수의 초기값 설정
    playerDataId = 0;
    hp = 0f;
    maxHp = 0f;
    maxHpBonusRate = 1f;
    healBonusRate = 1f;
    hpRegen = 0f;
    atk = 0f;
    attackRate = 1f;
    def = 0f;
    defRate = 0f;
    moveSpeed = 0f;
    moveSpeedRate = 1f;
    totalExp = 0f;
    level = 1;
    exp = 0f;
    criRate = 0f;
    criDamage = 1.5f;
    damageReduction = 0f;
    expBonusRate = 1f;
    soulBonusRate = 1f;
    collectDistBonus = 1f;
        
    killCount = 0;
    soulCount = 0f;
    skillRefreshCount = 3;

    soulShopList.Clear();
    savedSupportSkill.Clear();
    savedBattleSkill.Clear();
  }
}

public class GameManager
{
  public bool isLoaded = false;
  
  private GameData _gameData = new GameData();
  private int _gem;
  private int _killCount;
  private Vector2 _moveDir;
  
  // Delegate
  public event Action<Vector2> OnMoveDirChanged;
  public event Action<int> OnGemCountChanged;
  public event Action<int> OnKillCountChanged;
  public event Action OnEquipInfoChanged;
  public event Action OnResourcesChanged;
  
  // Property
  public int Gem
  {
    get => _gem;
    set
    {
      _gem = value;
      OnGemCountChanged?.Invoke(value);
    }
  }
  public int KillCount
  {
    get => _killCount;
    set
    {
      _killCount = value;
      OnKillCountChanged?.Invoke(value);
    }
  }
  public Vector2 MoveDir
  {
    get => _moveDir;
    set
    {
      _moveDir = value;
      OnMoveDirChanged?.Invoke(_moveDir);
    }
  }
  public PlayerController Player => Managers.Object?.Player;

  #region GameData Properties
  public Map CurrentMap { get; set; }
  public List<Equipment> OwnedEquipments
  {
    get => _gameData.ownedEquipments;
    set => _gameData.ownedEquipments = value;
    // TODO : 갱신이 빈번하게 발생하여 렉 발생, Sorting시 무한루프 발생으로 인하여 주석처리
  }
  public List<SupportSkillData> SoulShopList
  {
    get => _gameData.continueInfo.soulShopList;
    set
    {
      _gameData.continueInfo.soulShopList = value;
      SaveGame();
    }
  }
  public Dictionary<int, int> ItemDictionary
  {
    get => _gameData.itemDictionary;
    set => _gameData.itemDictionary = value;
  }
  public Dictionary<EMissionTarget, MissionInfo> DicMission
  {
    get => _gameData.dicMission;
    set => _gameData.dicMission = value;
  }
  public List<Character> Characters
  {
    get => _gameData.characters;
    set
    {
      _gameData.characters = value;
      OnEquipInfoChanged?.Invoke();
    }
  }
  public Dictionary<EEquipmentType, Equipment> EquippedEquipments
  {
    get => _gameData.equippedEquipments;
    set
    {
      _gameData.equippedEquipments = value;
      OnEquipInfoChanged?.Invoke();
    }
  }
  public Dictionary<int, StageClearInfo> DicStageClearInfo
  {
    get => _gameData.dicStageClearInfo;
    set
    {
      _gameData.dicStageClearInfo = value;
      Managers.Achievement.StageClear();
      SaveGame();
    }
  }
  public int UserLevel
  {
    get => _gameData.userLevel;
    set => _gameData.userLevel = value;
  }
  public string UserName
  {
    get => _gameData.userName;
    set => _gameData.userName = value;
  }
  public int Stamina
  {
    get => _gameData.stamina;
    set 
    { 
      _gameData.stamina = value;
      SaveGame();
      OnResourcesChanged?.Invoke();
    }
  }
  public int Gold
  {
    get => _gameData.gold;
    set 
    {
      _gameData.gold = value;
      SaveGame();
      OnResourcesChanged?.Invoke();
    }
  }
  public int Dia
  {
    get => _gameData.dia;
    set 
    { 
      _gameData.dia = value;
      SaveGame();
      OnResourcesChanged?.Invoke();
    }
  }
  public int CommonGachaOpenCount
  {
    get => _gameData.commonGachaOpenCount;
    set { 
      _gameData.commonGachaOpenCount = value;
      Managers.Achievement.CommonOpen();
    }
  }
  public int AdvancedGachaOpenCount
  {
    get => _gameData.advancedGachaOpenCount;
    set { 
      _gameData.advancedGachaOpenCount = value;
      Managers.Achievement.AdvancedOpen();
    }
  }
  public int FastRewardCount
  {
    get => _gameData.fastRewardCount;
    set { 
      _gameData.fastRewardCount = value;
      Managers.Achievement.FastReward();
    }
  }
  public int OfflineRewardCount
  {
    get => _gameData.offlineRewardCount;
    set { 
      _gameData.offlineRewardCount = value;
      Managers.Achievement.OfflineReward();
    }
  }
  public int TotalMonsterKillCount
  {
    get => _gameData.totalMonsterKillCount;
    set 
    { 
      _gameData.totalMonsterKillCount = value;
      if (value % 100 == 0)
        Managers.Achievement.MonsterKill();
    }
  }
  public int TotalEliteKillCount
  {
    get => _gameData.totalEliteKillCount;
    set { 
      _gameData.totalEliteKillCount = value;
      Managers.Achievement.EliteKill();
    }
  }
  public int TotalBossKillCount
  {
    get => _gameData.totalBossKillCount;
    set { 
      _gameData.totalBossKillCount = value;
      Managers.Achievement.BossKill();
    }
  }
  public List<Data.AchievementData> Achievements
  {
    get => _gameData.achievements;
    set => _gameData.achievements = value;
  }
  public ContinueData ContinueInfo
  {
    get => _gameData.continueInfo;
    set => _gameData.continueInfo = value;
  }
  public StageData CurrentStageData
  {
    get => _gameData.currentStage;
    set => _gameData.currentStage = value;
  }
  public WaveData CurrentWaveData => CurrentStageData.WaveArray[CurrentWaveIndex];
  public int CurrentWaveIndex
  {
    get => _gameData.continueInfo.waveIndex;
    set => _gameData.continueInfo.waveIndex = value;
  }
  public int GachaCountAdsAdvanced
  {
    get => _gameData.gachaCountAdsAnvanced;
    set => _gameData.gachaCountAdsAnvanced = value;
  }
  public int GachaCountAdsCommon
  {
    get => _gameData.gachaCountAdsCommon;
    set => _gameData.gachaCountAdsCommon = value;
  }
  public int GoldCountAds 
  {
    get => _gameData.goldCountAds;
    set => _gameData.goldCountAds = value;
  }
  public int RebirthCountAds
  {
    get => _gameData.rebirthCountAds;
    set => _gameData.rebirthCountAds = value;
  }
  public int DiaCountAds
  {
    get => _gameData.diaCountAds;
    set => _gameData.diaCountAds = value;
  }
  public int StaminaCountAds
  {
    get => _gameData.staminaCountAds;
    set => _gameData.staminaCountAds = value;
  }
  public int FastRewardCountAds
  {
    get => _gameData.fastRewardCountAds;
    set => _gameData.fastRewardCountAds = value;
  }
  public int SkillRefreshCountAds
  {
    get => _gameData.skillRefreshCountAds;
    set => _gameData.skillRefreshCountAds = value;
  }
  public int RemainsStaminaByDia
  {
    get => _gameData.remainsStaminaByDia;
    set => _gameData.remainsStaminaByDia = value;
  }
  public int BronzeKeyCountAds
  {
    get => _gameData.bronzeKeyCountAds;
    set => _gameData.bronzeKeyCountAds = value;
  }
  public int FastRewardCountStamina
  {
    get => _gameData.fastRewardCountStamina;
    set => _gameData.fastRewardCountStamina = value;
  }
  public bool[] AttendanceReceived 
  {
    get => _gameData.attendanceReceived;
    set => _gameData.attendanceReceived = value;
  }
  #endregion

  #region Option Properties
  public bool BGMOn
  {
    get => _gameData.BGMOn;
    set
    {
      if(_gameData.BGMOn == value) return;

      _gameData.BGMOn = value;
      if (_gameData.BGMOn == false)
      {
        Managers.Sound.Stop(ESound.BGM);
      }
      else
      {
        string name = "BGM_Lobby";
        if (Managers.Scene.CurrentScene.SceneType == Define.EScene.GameScene)
          name = "BGM_Game";
        
        Managers.Sound.Play(Define.ESound.BGM, name);
      }
    }
  }
  public bool EffectSoundOn
  {
    get => _gameData.effectSoundOn;
    set => _gameData.effectSoundOn = value;
  }
  public EJoystickType JoystickType
  {
    get => _gameData.joystickType;
    set => _gameData.joystickType = value;
  }
  #endregion
  
  public int GetMaxStageClearIndex()
  {
    int MaxStageClearIndex = 0;

    foreach (StageClearInfo stageClearInfo in Managers.Game.DicStageClearInfo.Values)
    {
      if (stageClearInfo.isClear == true)
        MaxStageClearIndex = Mathf.Max(MaxStageClearIndex, stageClearInfo.stageIndex);
    }
    return MaxStageClearIndex;
  }
  
  public void EquipItem(EEquipmentType type, Equipment equipment)
  {
    if (EquippedEquipments.ContainsKey(type))
    {
      EquippedEquipments[type].IsEquipped = false;
      EquippedEquipments.Remove(type);
    }
  
    EquippedEquipments.Add(type, equipment);
    equipment.IsEquipped = true;
    equipment.IsConfirmed = true;
 
    OnEquipInfoChanged?.Invoke();
  }
  

  #region Save & Load
  private string _path;
  public void SaveGame()
  {
    if (Player != null )
    {
      // _gameData.continueInfo.savedBattleSkill = Player.Skills?.savedBattleSkill;
      // _gameData.continueInfo.savedSupportSkill = Player.Skills?.supportSkills;
    }
    string jsonStr = JsonConvert.SerializeObject(_gameData);
    File.WriteAllText(_path, jsonStr);
  }
  public bool LoadGame()
  {
    if (PlayerPrefs.GetInt("ISFIRST", 1) == 1)
    {
      string path = Application.persistentDataPath + "/SaveData.json";
      if(File.Exists(path)) File.Delete(path);

      return false;
    }

    if (File.Exists(_path) == false) return false;

    string fileStr = File.ReadAllText(_path);
    GameData data = JsonConvert.DeserializeObject<GameData>(fileStr);
    if(data != null) _gameData = data;
    
    EquippedEquipments = new Dictionary<EEquipmentType, Equipment>();
    for (int i = 0; i < OwnedEquipments.Count; i++)
    {
      if (OwnedEquipments[i].IsEquipped)
        EquipItem(OwnedEquipments[i].equipmentData.equipmentType, OwnedEquipments[i]);
    }
    
    isLoaded = true;
    return true;
  }
  public void ClearContinueData()
  {
    Managers.Game.SoulShopList.Clear();
    ContinueInfo.Clear();
    CurrentWaveIndex = 0;
    SaveGame();
  }
  public float GetTotalDamage()
  {
    float result = 0;
    foreach (SkillBase skill in Player.Skills.SkillList)
    {
      result += skill.TotalDamage;
    }

    return result;
  }
  #endregion
}
