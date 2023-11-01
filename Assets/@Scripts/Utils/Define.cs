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
  
  public enum ESound { BGM, SubBGM, Effect, Max }
  
  public enum EObjectType { Player, Monster, Projectile, Env }
  
  public enum ESkillType { None, Sequence, Repeat, Etc }
  
  public enum EStageType { Normal, Boss }
  
  public enum ECreatureState { Idle, Moving, Skill, Dead }
  
  public enum EMissionTarget // 미션 조건
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
}

