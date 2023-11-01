using System;
using System.Collections.Generic;
using System.IO;
using Data;
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
  private GameData _gameData = new GameData();
  private int _gem;
  private int _killCount;
  private Vector2 _moveDir;
  
  // Delegate
  public event Action<Vector2> OnMoveDirChanged;
  public event Action<int> OnGemCountChanged;
  public event Action<int> OnKillCountChanged;
  
  // Property
  public Map CurrentMap { get; set; }
  public List<SupportSkillData> SoulShopList
  {
    get { return _gameData.continueInfo.soulShopList; }
    set
    {
      _gameData.continueInfo.soulShopList = value;
      SaveGame();
    }
  }

  public int CurrentWaveIndex
  {
    get{ return _gameData.ContinueInfo.WaveIndex; }
    set{ _gameData.ContinueInfo.WaveIndex = value; }
  }
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
  public int Gold { get; set; }


  #region Option
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
        if(Managers.Scene.)
      }
    }
  }
  

  #endregion
  private string _path;
  public void SaveGame()
  {
    // if (Player != null )
    // {
    //   _gameData.continueInfo.savedBattleSkill = Player.Skills?.savedBattleSkill;
    //   _gameData.continueInfo.savedSupportSkill = Player.Skills?.supportSkills;
    // }
    // string jsonStr = JsonConvert.SerializeObject(_gameData);
    // File.WriteAllText(_path, jsonStr);
  }
}
