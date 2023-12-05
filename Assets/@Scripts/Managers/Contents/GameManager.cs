using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using Random = UnityEngine.Random;

using Newtonsoft.Json;
using Data;
using UnityEngine.Serialization;
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

  public int stamina = MAX_STAMINA;
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
  
  public int fastRewardCountAds = 1;
  public int fastRewardCountStamina = 3;
  
  public int skillRefreshCountAds = 3;
  
  public int gainStaminaByAds = 1;
  public int gainStaminaByDia = 3;
  
  public int cloverCountAds = 1;
  #endregion
  
  public bool[] attendanceReceived = new bool[30];
  public bool BGMOn = true;
  public bool effectSoundOn = true;
  public EJoystickType joystickType = EJoystickType.Flexible;
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
  public int skillRefreshCount = 1;
  public float soulCount;

  public List<SupportSkillData> soulShopList = new List<SupportSkillData>();
  public List<SupportSkillData> savedSupportSkill = new List<SupportSkillData>();
  public Dictionary<ESkillType, int> savedBattleSkill = new Dictionary<ESkillType, int>();

  public int waveIndex;
  
  // Property
  public bool IsContinue => savedBattleSkill.Count > 0;

  public void Clear()
  {
    // 각 변수들의 초기값을 재설정
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
    skillRefreshCount = 1;

    soulShopList.Clear();
    savedSupportSkill.Clear();
    savedBattleSkill.Clear();
  }
}

public class GameManager
{
  public bool isLoaded = false;
  public bool isGameEnd = false;
  public float timeRemaining = 60;
  
  private GameData _gameData = new GameData();
  private int _gem;
  private int _killCount;
  private Vector2 _moveDir;
  private string _path; // save & load를 위한 경로
  
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
  public PlayerController Player { get; set; }
  public Vector3 SoulDestination { get; set; }
  public CameraController CameraController { get; set; }
  public Character CurrentCharacter => _gameData.characters.Find(c => c.isCurrentCharacter == true);

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
  public WaveData CurrentWaveData => CurrentStageData.waveArray[CurrentWaveIndex];
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
  public int FastRewardCountAds
  {
    get => _gameData.fastRewardCountAds;
    set => _gameData.fastRewardCountAds = value;
  }
  public int FastRewardCountStamina
  {
    get => _gameData.fastRewardCountStamina;
    set => _gameData.fastRewardCountStamina = value;
  }
  public int SkillRefreshCountAds
  {
    get => _gameData.skillRefreshCountAds;
    set => _gameData.skillRefreshCountAds = value;
  }
  public int GainStaminaByAds
  {
    get => _gameData.gainStaminaByAds;
    set => _gameData.gainStaminaByAds = value;
  }
  public int GainStaminaByDia
  {
    get => _gameData.gainStaminaByDia;
    set => _gameData.gainStaminaByDia = value;
  }
  public int CloverCountAds
  {
    get => _gameData.cloverCountAds;
    set => _gameData.cloverCountAds = value;
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

  public void Init()
  {
    _path = Application.persistentDataPath + "/SaveData.json";
    if (LoadGame()) return;
    
    PlayerPrefs.SetInt("ISFIRST", 1);
    
    Character character = new Character();
    character.SetInfo(CHARACTER_DEFAULT_ID);
    character.isCurrentCharacter = true;

    Characters = new List<Character>();
    Characters.Add(character);

    CurrentStageData = Managers.Data.StageDic[1];
    
    foreach (StageData stage in Managers.Data.StageDic.Values)
    {
      StageClearInfo info = new StageClearInfo 
      {
        stageIndex = stage.stageIndex,
        maxWaveIndex = 0,
        isOpenFirstBox = false,
        isOpenSecondBox = false,
        isOpenThirdBox = false,
      };
      _gameData.dicStageClearInfo.Add(stage.stageIndex, info);
    }
    
    Managers.Time.LastRewardTime = DateTime.Now;
    Managers.Time.LastGeneratedStaminaTime = DateTime.Now;

    SetBaseEquipment();

    Managers.Achievement.Init();
    
    // 처음 접속할 때 주는 보상들
    ExchangeMaterial(Managers.Data.MaterialDic[Define.ID_CLOVER], 1);
    ExchangeMaterial(Managers.Data.MaterialDic[Define.ID_GOLD_KEY], 30);
    ExchangeMaterial(Managers.Data.MaterialDic[Define.ID_DIA], 1000);
    ExchangeMaterial(Managers.Data.MaterialDic[Define.ID_GOLD], 100000);
    ExchangeMaterial(Managers.Data.MaterialDic[Define.ID_WEAPON_SCROLL], 15);
    ExchangeMaterial(Managers.Data.MaterialDic[Define.ID_GLOVES_SCROLL], 15);
    ExchangeMaterial(Managers.Data.MaterialDic[Define.ID_RING_SCROLL], 15);
    ExchangeMaterial(Managers.Data.MaterialDic[Define.ID_BELT_SCROLL], 15);
    ExchangeMaterial(Managers.Data.MaterialDic[Define.ID_ARMOR_SCROLL], 15);
    ExchangeMaterial(Managers.Data.MaterialDic[Define.ID_BOOTS_SCROLL], 15);
    
    isLoaded = true;
    SaveGame();
  }

  public void ExchangeMaterial(MaterialData data, int count)
  {
    switch (data.materialType)
    {
      case EMaterialType.Dia:
        Dia += count;
        break;
      case EMaterialType.Gold:
        Gold += count;
        break;
      case EMaterialType.Stamina:
        Stamina += count;
        break;
      case EMaterialType.Clover:
      case EMaterialType.SilverKey:
      case EMaterialType.GoldKey:
        AddMaterialItem(data.dataId, count);
        break;
      case EMaterialType.RandomScroll:
        int randScroll = Random.Range(50101, 50106);
        AddMaterialItem(randScroll, count);
        break;
      case EMaterialType.WeaponScroll:
        AddMaterialItem(ID_WEAPON_SCROLL, count);
        break;
      case EMaterialType.GlovesScroll:
        AddMaterialItem(ID_GLOVES_SCROLL, count);
        break;
      case EMaterialType.RingScroll:
        AddMaterialItem(ID_RING_SCROLL, count);
        break;
      case EMaterialType.BeltScroll:
        AddMaterialItem(ID_BELT_SCROLL, count);
        break;
      case EMaterialType.ArmorScroll:
        AddMaterialItem(ID_ARMOR_SCROLL, count);
        break;
      case EMaterialType.BootsScroll:
        AddMaterialItem(ID_BOOTS_SCROLL, count);
        break;
      default: 
        // TODO:
        break;
    }
  }
  
  public void AddMaterialItem(int id, int quantity)
  {
    if (ItemDictionary.ContainsKey(id))
      ItemDictionary[id] += quantity;
    else
      ItemDictionary[id] = quantity;
    
    SaveGame();
  }
  public void RemoveMaterialItem(int id, int quantity)
  {
    if (ItemDictionary.ContainsKey(id))
    {
      ItemDictionary[id] -= quantity;
      SaveGame();
    }
  }

  #region Functions For In-Game
  public GemInfo GetGemInfo()
  {
    float smallGemChance = CurrentWaveData.smallGemDropRate;
    float greenGemChance = CurrentWaveData.greenGemDropRate + smallGemChance;
    float blueGemChance = CurrentWaveData.blueGemDropRate + greenGemChance;
    float yellowGemChance = CurrentWaveData.yellowGemDropRate + blueGemChance;
    float rand = Random.value;

    if (rand < smallGemChance)
      return new GemInfo(GemInfo.EGemType.Small, new Vector3(0.65f, 0.65f, 0.65f));
    else if (rand < greenGemChance)
      return new GemInfo(GemInfo.EGemType.Green, Vector3.one);
    else if (rand < blueGemChance)
      return new GemInfo(GemInfo.EGemType.Blue, Vector3.one);
    else if (rand < yellowGemChance)
      return new GemInfo(GemInfo.EGemType.Yellow, Vector3.one);

    return null;
  }
  public GemInfo GetGemInfo(GemInfo.EGemType type)
  {
    if(type == GemInfo.EGemType.Small)
      return new GemInfo(GemInfo.EGemType.Small, new Vector3(0.65f, 0.65f, 0.65f));

    return new GemInfo(type, Vector3.one);
  }
  public void GameOver()
  {
    isGameEnd = true;
    Player.StopAllCoroutines();
    Managers.UI.ShowPopupUI<UI_GameoverPopup>().SetInfo();
  }
  public (int hp, int atk) GetCurrentCharacterStat()
  {
    int hpBonus = 0;
    int atkBonus = 0;
    var (equipHpBonus, equipAtkBonus) = GetEquipmentBonus();

    Character ch = CurrentCharacter;

    hpBonus = (equipHpBonus);
    atkBonus = (equipAtkBonus);

    return (hpBonus, atkBonus);
  }
  #endregion

  #region Functions For Equipment
  public void SetBaseEquipment()
  {
    //초기아이템 설정
    Equipment weapon = new Equipment(WEAPON_DEFAULT_ID);
    Equipment gloves = new Equipment(GLOVES_DEFAULT_ID);
    Equipment ring = new Equipment(RING_DEFAULT_ID);
    Equipment belt = new Equipment(BELT_DEFAULT_ID);
    Equipment armor = new Equipment(ARMOR_DEFAULT_ID);
    Equipment boots = new Equipment(BOOTS_DEFAULT_ID);

    OwnedEquipments = new List<Equipment>
    {
      weapon,
      gloves,
      ring,
      belt,
      armor,
      boots
    };

    EquippedEquipments = new Dictionary<EEquipmentType, Equipment>();
    EquipItem(EEquipmentType.Weapon, weapon);
    EquipItem(EEquipmentType.Gloves, gloves);
    EquipItem(EEquipmentType.Ring, ring);
    EquipItem(EEquipmentType.Belt, belt);
    EquipItem(EEquipmentType.Armor, armor);
    EquipItem(EEquipmentType.Boots, boots);
  }
  public void EquipItem(EEquipmentType type, Equipment equipment)
  {
    if (EquippedEquipments.ContainsKey(type))
    {
      EquippedEquipments[type].IsEquipped = false;
      EquippedEquipments.Remove(type);
    }

    // 새로운 장비를 착용
    EquippedEquipments.Add(type, equipment);
    equipment.IsEquipped = true;
    equipment.IsConfirmed = true;

    // 장비변경 이벤트 호출
    OnEquipInfoChanged?.Invoke();
  }
  public void UnEquipItem(Equipment equipment)
  {
    // 착용중인 장비를 제거한다.
    if (EquippedEquipments.ContainsKey(equipment.equipmentData.equipmentType))
    {
      EquippedEquipments[equipment.equipmentData.equipmentType].IsEquipped = false;
      EquippedEquipments.Remove(equipment.equipmentData.equipmentType);
    }
    
    // 장비변경 이벤트 호출
    OnEquipInfoChanged?.Invoke();
  }
  public Equipment AddEquipment(string key)
  {
    if (key.Equals("None")) return null;

    Equipment equip = new Equipment(key);
    equip.IsConfirmed = false;
    OwnedEquipments.Add(equip);
    OnEquipInfoChanged?.Invoke();

    return equip;
  }
  public Equipment MergeEquipment(Equipment equipment, Equipment mergeEquipment1, Equipment mergeEquipment2, bool isAllMerge = false)
  {
    equipment = OwnedEquipments.Find(equip => equip == equipment);
    if (equipment == null) return null;
     
    mergeEquipment1 = OwnedEquipments.Find(equip => equip == mergeEquipment1);
    if (mergeEquipment1 == null) return null;

    if (mergeEquipment2 != null)
    {
      mergeEquipment2 = OwnedEquipments.Find(equip => equip == mergeEquipment2);
      if (mergeEquipment2 == null) return null;
    }

    int level = equipment.Level;
    bool isEquipped = equipment.IsEquipped;
    string mergedItemCode = equipment.equipmentData.mergedItemCode;
    Equipment newEquipment = AddEquipment(mergedItemCode);
    newEquipment.Level = level;
    newEquipment.IsEquipped = isEquipped;

    OwnedEquipments.Remove(equipment);
    OwnedEquipments.Remove(mergeEquipment1);
    OwnedEquipments.Remove(mergeEquipment2);

    if (Managers.Game.DicMission.TryGetValue(EMissionTarget.EquipmentMerge, out MissionInfo mission))
      mission.progress++;

    //자동합성인 경우는 SAVE게임 하지않고 다끝난후에 한번에 한다.
    if(isAllMerge == false)
      SaveGame();

    Debug.Log(newEquipment.equipmentData.equipmentGrade);
    return newEquipment;
  }
  public void SortEquipment(EEquipmentSortType sortType)
  {
    if (sortType == EEquipmentSortType.Grade)
    {
      //OwnedEquipments = OwnedEquipments.OrderBy(item => item.EquipmentGrade).ThenBy(item => item.Level).ThenBy(item => item.EquipmentType).ToList();
      OwnedEquipments = OwnedEquipments.OrderBy(item => item.equipmentData.equipmentGrade)
        .ThenBy(item => item.IsEquipped)
        .ThenBy(item => item.Level)
        .ThenBy(item => item.equipmentData.equipmentType).ToList();
    }
    else if (sortType == EEquipmentSortType.Level)
    {
      OwnedEquipments = OwnedEquipments.OrderBy(item => item.Level)
        .ThenBy(item => item.IsEquipped)
        .ThenBy(item => item.equipmentData.equipmentGrade)
        .ThenBy(item => item.equipmentData.equipmentType).ToList();
    }
  }
  private (int hp, int atk) GetEquipmentBonus()
  {
    int hpBonus = 0;
    int atkBonus = 0;

    foreach (KeyValuePair<EEquipmentType, Equipment> pair in EquippedEquipments)
    {
      hpBonus += pair.Value.MaxHpBonus;
      atkBonus += pair.Value.AttackBonus;
    }
    return (hpBonus, atkBonus);
  }
  #endregion

  #region Functions For Gacha
  public List<Equipment> DoGacha(EGachaType gachaType, int count = 1)
  {
    List<Equipment> ret = new List<Equipment>();

    for (int i = 0; i < count; i++)
    {
      EEquipmentGrade grade = GetRandomGrade(PICKUP_GACHA_GRADE_PROB);
      switch (gachaType)
      {
        case EGachaType.CommonGacha:
          grade = GetRandomGrade(COMMON_GACHA_GRADE_PROB);
          CommonGachaOpenCount++;
          break;
        case EGachaType.PickupGacha:
          grade = GetRandomGrade(PICKUP_GACHA_GRADE_PROB);
          break;
        case EGachaType.AdvancedGacha:
          grade = GetRandomGrade(ADVENCED_GACHA_GRADE_PROB);
          AdvancedGachaOpenCount++;
          break;
      }

      List<GachaRateData> list = Managers.Data.GachaTableDataDic[gachaType].gachaRateTable.Where(item => item.equipGrade == grade).ToList();

      int index = Random.Range(0, list.Count);
      string key = list[index].equipmentID;

      if (Managers.Data.EquipDataDic.ContainsKey(key))
        ret.Add(AddEquipment(key));
    }

    return ret;
  }
  private static EEquipmentGrade GetRandomGrade(float[] prob)
  {
    float randomValue = Random.value;
    if (randomValue < prob[(int)EEquipmentGrade.Common])
    {
      return EEquipmentGrade.Common;
    }
    else if (randomValue < prob[(int)EEquipmentGrade.Common] + prob[(int)EEquipmentGrade.Uncommon])
    {
      return EEquipmentGrade.Uncommon;
    }
    else if (randomValue < prob[(int)EEquipmentGrade.Common] + prob[(int)EEquipmentGrade.Uncommon] + prob[(int)EEquipmentGrade.Rare])
    {
      return EEquipmentGrade.Rare;
    }
    else if (randomValue < prob[(int)EEquipmentGrade.Common] + prob[(int)EEquipmentGrade.Uncommon] + prob[(int)EEquipmentGrade.Rare] + prob[(int)EEquipmentGrade.Epic])
    {
      return EEquipmentGrade.Epic;
    }

    return EEquipmentGrade.Common;
  }
  #endregion
  
  public void SetNextStage()
  { 
    CurrentStageData = Managers.Data.StageDic[CurrentStageData.stageIndex + 1];
  }
  public int GetMaxStageIndex()
  {
    foreach(StageClearInfo clearInfo in _gameData.dicStageClearInfo.Values) 
    {
      if (clearInfo.maxWaveIndex != 10)
        return clearInfo.stageIndex;
    }
    return 0;
  }
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
  

  #region Functions For Save & Load
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
