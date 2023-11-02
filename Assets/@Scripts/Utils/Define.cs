using System;
using System.Collections.Generic;
using UnityEngine;

using static Utils;

public static class Define
{
  public enum UIEvent
  {
    Click,
    Pressed,
    PointerDown,
    PointerUp,
    BeginDrag,
    Drag,
    EndDrag,
  }
  
  public enum EScene { Unknown, TitleScene, LobbyScene, GameScene }
  
  public enum EJoystickType { Fixed, Flexible }
  
  public enum ESound { BGM, Effect, Max }

  public enum EObjectType
  {
    Player,
    Monster,
    EliteMonster,
    Boss,
    Projectile,
    Gem,
    Soul,
    Potion,
    DropBox,
    Magnet,
    Bomb
  }
  
  public enum ESkillType
  {
    None = 0,
    EnergyBolt = 10001,       //100001 ~ 100005 
    IcicleArrow = 10011,          //100011 ~ 100015 
    PoisonField = 10021,      //100021 ~ 100025 
    EletronicField = 10031,   //100031 ~ 100035 
    Meteor = 10041,           //100041 ~ 100045 
    FrozenHeart = 10051,      //100051 ~ 100055 
    WindCutter = 10061,       //100061 ~ 100065 
    EgoSword = 10071,         //100071 ~ 100075 
    ChainLightning = 10081,
    Shuriken = 10091,
    ArrowShot = 10101,
    SavageSmash = 10111,
    PhotonStrike = 10121,
    StormBlade = 10131,
    MonsterSkill_01 = 20091,
    BasicAttack = 100101,
    Move = 100201,
    Charging = 100301,
    Dash = 100401,
    SpinShot = 100501,
    CircleShot = 100601,
    ComboShot = 100701,
  }
  
  public enum EStageType { Normal, Boss }
  
  public enum ECreatureState { Idle, Moving, Skill, Dead }
  public enum EEquipmentSortType { Level, Grade } // TODO: 획득 순 정렬 추가 필요
  public enum EMergeEquipmentType{ None, ItemCode, Grade }
  public enum EEquipmentGrade
  {
    None,
    Common,
    Uncommon,
    Rare,
    Epic,
    Epic1,
    Epic2,
    Legendary,
    Legendary1,
    Legendary2,
    Legendary3,
    Myth,
    Myth1,
    Myth2,
    Myth3
  }
  public enum EMaterialGrade
  {
    Common,
    Uncommon,
    Rare,
    Epic,
    Epic1,
    Epic2,
    Legendary,
    Legendary1,
    Legendary2,
    Legendary3,
  }
  
  public enum EGachaRarity { Normal, Special }
  public enum EGachaType { None, CommonGacha, AdvancedGacha, PickupGacha, }
  


  
  // 미션 조건
  public enum EMissionTarget 
  {
    DailyComplete,            // 데일리 완료
    WeeklyComplete,           // 위클리 완료
    StageEnter,               // 스테이지 입장
    StageClear,               // 스테이지 클리어
    EquipmentLevelUp,         // 장비 레벨업
    CommonGachaOpen,          // 일반 가챠 오픈 (광고 유도목적)
    AdvancedGachaOpen,        // 고급 가챠 오픈 (광고 유도목적)
    OfflineRewardGet,         // 오프라인 보상 
    FastOfflineRewardGet,     // 빠른 오프라인 보상
    ShopProductBuy,           // 상점 상품 구매
    Login,                    // 로그인
    EquipmentMerge,           // 장비 합성
    MonsterAttack,            // 몬스터 어택
    MonsterKill,              // 몬스터 킬
    EliteMonsterAttack,       // 엘리트 어택
    EliteMonsterKill,         // 엘리트 킬
    BossKill,                 // 보스 킬
    DailyShopBuy,             // 데일리 상점 상품 구매
    GachaOpen,                // 가챠 오픈 (일반, 고급가챠 포함)
    ADWatchIng,               // 광고 시청
  }
  
  public enum ESupportSkillType
  {
    General,
    Passive,
    LevelUp,
    MonsterKill,
    EliteKill,
    Special
  }
  public enum ESupportSkillGrade
  {
    Common,
    Uncommon,
    Rare,
    Epic,
    Legend
  }
  public enum ESupportSkillName
  {
    Critical,
    MaxHpBonus,
    ExpBonus,
    SoulBonus,
    DamageReduction,
    AtkBonusRate,
    MoveBonusRate,
    Healing, // 체력 회복 
    HealBonusRate,//회복량 증가
    HpRegen,
    CriticalDamage,
    MagneticRange,
    Resurrection,
    LevelupMoveSpeed,
    LevelupReduction,
    LevelupAtk,
    LevelupCri,
    LevelupCriDmg,
    MonsterKillAtk,
    MonsterKillMaxHP,
    MonsterKillReduction,
    EliteKillExp,
    EliteKillSoul,
    EnergyBolt,
    IcicleArrow,
    PoisonField,
    EletronicField,
    Meteor,
    FrozenHeart,
    WindCutter,
    EgoSword,
    ChainLightning,
    Shuriken,
    ArrowShot,
    SavageSmash,
    PhotonStrike,
    StormBlade,
  }
  
  public enum EEquipmentType
  {
    Weapon,
    Gloves,
    Ring,
    Belt,
    Armor,
    Boots,
  }
  
  public static int MAX_STAMINA = 50;
  public static int GAME_PER_STAMINA = 3;
  
  public const int GOBLIN_ID = 1;
  public const int SNAKE_ID = 2;
  public const int BOSS_ID = 3;
 

  public const int EGO_SWORD_ID = 10;
  public const int FIRE_BALL_ID = 11;

  public const string EXP_GEM_PREFAB = "EXPGem.prefab";
  public const string FIRE_PROJECTILE = "FireProjectile.prefab";
  
  public static float POTION_COLLECT_DISTANCE = 2.6F;
  public static float BOX_COLLECT_DISTANCE = 2.6F;
  public static int STAMINA_RECHARGE_INTERVAL = 300;
  public static int MAX_SKILL_LEVEL = 6;
  public static int MAX_SKILL_COUNT = 6;
  
  #region DataID
  public static int ID_GOLD = 50001;
  public static int ID_DIA = 50002;
  public static int ID_STAMINA = 50003;
  public static int ID_BRONZE_KEY = 50201;
  public static int ID_SILVER_KEY = 50202;
  public static int ID_GOLD_KEY = 50203;
  public static int ID_RANDOM_SCROLL = 50301;
  public static int ID_POTION = 60001;
  public static int ID_MAGNET = 60004;
  public static int ID_BOMB = 60008;

  public static int ID_WEAPON_SCROLL = 50101;
  public static int ID_GLOVES_SCROLL = 50102;
  public static int ID_RING_SCROLL = 50103;
  public static int ID_BELT_SCROLL = 50104;
  public static int ID_ARMOR_SCROLL = 50105;
  public static int ID_BOOTS_SCROLL = 50106;

  public static string GOLD_SPRITE_NAME = "Gold_Icon";
  #endregion
}

public static class EquipmentUIColors
{
  #region 장비 이름 색상
  public static readonly Color CommonNameColor = HexToColor("A2A2A2");
  public static readonly Color UncommonNameColor = HexToColor("57FF0B");
  public static readonly Color RareNameColor = HexToColor("2471E0");
  public static readonly Color EpicNameColor = HexToColor("9F37F2");
  public static readonly Color LegendaryNameColor = HexToColor("F67B09");
  public static readonly Color MythNameColor = HexToColor("F1331A");
  #endregion
  #region 테두리 색상
  public static readonly Color Common = HexToColor("AC9B83");
  public static readonly Color Uncommon = HexToColor("73EC4E");
  public static readonly Color Rare = HexToColor("0F84FF");
  public static readonly Color Epic = HexToColor("B740EA");
  public static readonly Color Legendary = HexToColor("F19B02");
  public static readonly Color Myth = HexToColor("FC2302");
  #endregion
  #region 배경색상
  public static readonly Color EpicBg = HexToColor("D094FF");
  public static readonly Color LegendaryBg = HexToColor("F8BE56");
  public static readonly Color MythBg = HexToColor("FF7F6E");
  #endregion
}

