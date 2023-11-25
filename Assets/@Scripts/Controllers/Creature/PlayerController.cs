using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Data;
using static Define;

public class PlayerController : CreatureController
{
  public PlayerStat statViewer = new PlayerStat();
  public Transform indicator;
  
  [SerializeField] private Transform _indicatorSprite;
  
  private Vector2 _moveDir = Vector2.zero;

  #region Properties
  // For Save
  public override int DataId
  {
    get => Managers.Game.ContinueInfo.playerDataId;
    set => Managers.Game.ContinueInfo.playerDataId = statViewer.dataId = value;
  }
  public override float Hp
  {
    get => Managers.Game.ContinueInfo.hp;
    set 
    { 
      if(value > MaxHp)
        Managers.Game.ContinueInfo.hp = statViewer.hp = MaxHp;
      else
        Managers.Game.ContinueInfo.hp = statViewer.hp = value; 
    }
  }
  public override float MaxHp
  {
    get => Managers.Game.ContinueInfo.maxHp;
    set => Managers.Game.ContinueInfo.maxHp = statViewer.maxHp = value;
  }
  public override float MaxHpBonusRate
  {
    get => Managers.Game.ContinueInfo.maxHpBonusRate;
    set => Managers.Game.ContinueInfo.maxHpBonusRate = statViewer.maxHpBonusRate = value;
  }
  public override float HealBonusRate
  {
    get => Managers.Game.ContinueInfo.healBonusRate;
    set => Managers.Game.ContinueInfo.healBonusRate = statViewer.healBonusRate = value;
  }
  public override float HpRegen
  {
    get => Managers.Game.ContinueInfo.hpRegen;
    set => Managers.Game.ContinueInfo.hpRegen = statViewer.hpRegen = value;
  }
  public override float Atk
  {
    get => Managers.Game.ContinueInfo.atk;
    set => Managers.Game.ContinueInfo.atk = statViewer.atk = value;
  }
  public override float AttackRate
  {
    get => Managers.Game.ContinueInfo.attackRate;
    set => Managers.Game.ContinueInfo.attackRate = statViewer.attackRate = value;
  }
  public override float Def
  {
    get => Managers.Game.ContinueInfo.def;
    set => Managers.Game.ContinueInfo.def = statViewer.def = value;
  }
  public override float DefRate
  {
    get => Managers.Game.ContinueInfo.defRate;
    set => Managers.Game.ContinueInfo.defRate = statViewer.defRate = value;
  }
  public override float CriRate
  {
    get => Managers.Game.ContinueInfo.criRate;
    set => Managers.Game.ContinueInfo.criRate = statViewer.criRate = value;
  }
  public override float CriDamage
  {
    get => Managers.Game.ContinueInfo.criDamage;
    set => Managers.Game.ContinueInfo.criDamage = statViewer.criDamage = value;
  }
  public override float DamageReduction
  {
    get => Managers.Game.ContinueInfo.damageReduction;
    set => Managers.Game.ContinueInfo.damageReduction = statViewer.damageReduction = value;
  }
  public override float MoveSpeedRate
  {
    get => Managers.Game.ContinueInfo.moveSpeedRate;
    set => Managers.Game.ContinueInfo.moveSpeedRate = statViewer.moveSpeedRate = value;
  }
  public override float MoveSpeed
  {
    get => Managers.Game.ContinueInfo.moveSpeed;
    set => Managers.Game.ContinueInfo.moveSpeed = statViewer.moveSpeed = value;
  }
  public int Level
  {
    get => Managers.Game.ContinueInfo.level;
    set => Managers.Game.ContinueInfo.level = value;
  }
  public float Exp
  {
    get => Managers.Game.ContinueInfo.exp;
    set
    {
      Managers.Game.ContinueInfo.exp = value;
      int level = Level;
      while (true)
      {
        LevelData nextLevel;
        if (Managers.Data.LevelDataDic.TryGetValue(level + 1, out nextLevel) == false)
          break;

        LevelData currentLevel;
        Managers.Data.LevelDataDic.TryGetValue(level, out currentLevel);
        if (Managers.Game.ContinueInfo.exp < currentLevel.totalExp)
          break;
        level++;
      }

      if (level != Level)
      {
        Level = level;
        LevelData currentLevel;
        Managers.Data.LevelDataDic.TryGetValue(level, out currentLevel);
        TotalExp = currentLevel.totalExp;
        LevelUp(Level);

      }

      OnPlayerDataUpdated();
    }
  }
  public float TotalExp
  {
    get => Managers.Game.ContinueInfo.totalExp;
    set => Managers.Game.ContinueInfo.totalExp = value;
  }
  public float ExpBonusRate
  {
    get { return Managers.Game.ContinueInfo.expBonusRate; }
    set { Managers.Game.ContinueInfo.expBonusRate = value; }
  }
  public float SoulBonusRate
  {
    get => Managers.Game.ContinueInfo.soulBonusRate;
    set => Managers.Game.ContinueInfo.soulBonusRate = value;
  }
  public float CollectDistBonus
  {
    get => Managers.Game.ContinueInfo.collectDistBonus;
    set => Managers.Game.ContinueInfo.collectDistBonus = value;
  }
  public int SkillRefreshCount
  {
    get => Managers.Game.ContinueInfo.skillRefreshCount;
    set => Managers.Game.ContinueInfo.skillRefreshCount = value;
  }
  public int KillCount
  {
    get => Managers.Game.ContinueInfo.killCount;
    set
    {
      Managers.Game.ContinueInfo.killCount = value;
      if (Managers.Game.DicMission.TryGetValue(EMissionTarget.MonsterKill, out MissionInfo mission))
        mission.progress = value;
      if (Managers.Game.ContinueInfo.killCount % 500 == 0)
      {
        Skills.OnMonsterKillBonus();
      }
      OnPlayerDataUpdated?.Invoke();
    }
  }
  public float SoulCount
  {
    get { return Managers.Game.ContinueInfo.soulCount; }

    set
    {
      Managers.Game.ContinueInfo.soulCount = Mathf.Round(value);

      OnPlayerDataUpdated?.Invoke();
    }
  }
  public float ExpRatio
  {
    get
    {
      LevelData currentLevelData;
      if (Managers.Data.LevelDataDic.TryGetValue(Level, out currentLevelData))
      {
        int currentLevelExp = currentLevelData.totalExp;
        int nextLevelExp = currentLevelExp;
        int previousLevelExp = 0;

        LevelData prevLevelData;
        if (Managers.Data.LevelDataDic.TryGetValue(Level - 1, out prevLevelData))
        {
          previousLevelExp = prevLevelData.totalExp;
        }
        
        // If the player isn't max level
        LevelData nextLevelData;
        if (Managers.Data.LevelDataDic.TryGetValue(Level + 1, out nextLevelData))
          nextLevelExp = nextLevelData.totalExp;

        return (float)(Exp - previousLevelExp) / (currentLevelExp - previousLevelExp);
      }

      return 0f;
    }
  }
  public float ItemCollectDist { get; } = 4.0f;
  
  public Vector2 MoveDir
  {
    get => _moveDir;
    set => _moveDir = value.normalized;
  }
  public Vector3 PlayerCenterPos => indicator.transform.position;
  public Vector3 PlayerDirection => (_indicatorSprite.transform.position - PlayerCenterPos).normalized;
  #endregion
  
  #region Action
  public Action OnPlayerDataUpdated;
  public Action OnPlayerLevelUp;
  public Action OnPlayerDead;
  public Action OnPlayerDamaged;
  public Action OnPlayerMove;
  #endregion

  private void Start()
  {
    StartCoroutine(CoSelfRecovery());
    if (Managers.Game.ContinueInfo.IsContinue == true)
      LoadSkill();
    else
      InitSkill();
  }
  private void Update()
  {
    HandlePlayerDirection();
    MovePlayer();
    CollectEnv();
  }
  private void OnDestroy()
  {
    if(Managers.Game != null)
      Managers.Game.OnMoveDirChanged -= HandleOnMoveDirChanged;
  }

  public override bool Init()
  {
    base.Init();

    ObjectType = EObjectType.Player;

    //event
    Managers.Game.OnMoveDirChanged += HandleOnMoveDirChanged;

    //camera
    FindObjectOfType<CameraController>()._playerTransform = gameObject.transform;
    transform.localScale = Vector3.one;
    return true;
  }
  public override void InitSkill()
  {
    base.InitSkill();
    
    Equipment item;
    Managers.Game.EquippedEquipments.TryGetValue(EEquipmentType.Weapon, out item);
    if (item != null)
    {
      // 베이스 스킬
      ESkillType type = Utils.GetSkillTypeFromInt(item.equipmentData.basicSkill);
      if (type != ESkillType.None)
      {
        Skills.AddSkill(type, item.equipmentData.basicSkill);
        Skills.LevelUpSkill(type);
      }

      SupportSkillData uncommonSkill;
      SupportSkillData rareSkill;
      SupportSkillData epicSkill;
      SupportSkillData legendSkill;
      
      //등급별 서포트 스킬
      foreach (Equipment equip in Managers.Game.EquippedEquipments.Values)
      {
        switch (equip.equipmentData.equipmentGrade)
        {
          case EEquipmentGrade.Uncommon:
            if (Managers.Data.SupportSkillDic.TryGetValue(equip.equipmentData.uncommonGradeSkill, out uncommonSkill))
              Skills.AddSupportSkill(uncommonSkill);
            break;
          case EEquipmentGrade.Rare:
            if (Managers.Data.SupportSkillDic.TryGetValue(equip.equipmentData.uncommonGradeSkill, out uncommonSkill))
              Skills.AddSupportSkill(uncommonSkill);
            if (Managers.Data.SupportSkillDic.TryGetValue(equip.equipmentData.rareGradeSkill, out rareSkill))
              Skills.AddSupportSkill(rareSkill);
            break;
          case EEquipmentGrade.Epic:
          case EEquipmentGrade.Epic1:
          case EEquipmentGrade.Epic2:
            if (Managers.Data.SupportSkillDic.TryGetValue(equip.equipmentData.uncommonGradeSkill, out uncommonSkill))
              Skills.AddSupportSkill(uncommonSkill);
            if (Managers.Data.SupportSkillDic.TryGetValue(equip.equipmentData.rareGradeSkill, out rareSkill))
              Skills.AddSupportSkill(rareSkill);
            if (Managers.Data.SupportSkillDic.TryGetValue(equip.equipmentData.epicGradeSkill, out epicSkill))
              Skills.AddSupportSkill(epicSkill);
            break;
          case EEquipmentGrade.Legendary:
          case EEquipmentGrade.Legendary1:
          case EEquipmentGrade.Legendary2:
          case EEquipmentGrade.Legendary3:
            if (Managers.Data.SupportSkillDic.TryGetValue(equip.equipmentData.uncommonGradeSkill, out uncommonSkill))
              Skills.AddSupportSkill(uncommonSkill);
            if (Managers.Data.SupportSkillDic.TryGetValue(equip.equipmentData.rareGradeSkill, out rareSkill))
              Skills.AddSupportSkill(rareSkill);
            if (Managers.Data.SupportSkillDic.TryGetValue(equip.equipmentData.epicGradeSkill, out epicSkill))
              Skills.AddSupportSkill(epicSkill);
            if (Managers.Data.SupportSkillDic.TryGetValue(equip.equipmentData.legendaryGradeSkill, out legendSkill))
              Skills.AddSupportSkill(legendSkill);
            break;
        }
      }
    }
  }
  public override void InitCreatureStat(bool isFullHp = true)
  {
    // 현재 케릭터의 Stat 가져오기
    MaxHp = Managers.Game.CurrentCharacter.MaxHp;
    Atk = Managers.Game.CurrentCharacter.Atk;
    MoveSpeed = creatureData.moveSpeed * creatureData.moveSpeedRate;

    //장비 합산 데이터 다 가져오기
    var (equip_hp, equip_attack) = Managers.Game.GetCurrentChracterStat();
    MaxHp += equip_hp;
    Atk += equip_attack;

    MaxHp *= MaxHpBonusRate;
    Atk *= AttackRate;
    Def *= DefRate;
    MoveSpeed *= MoveSpeedRate;

    if (isFullHp == true)
      Hp = MaxHp;
  }
  public override void UpdatePlayerStat()
  {
    InitCreatureStat(false);

    MaxHp *= MaxHpBonusRate;
    Hp *= MaxHpBonusRate;// 최대체력이 늘어난 비율 만큼 hp도 같이 늘어난다.
    Atk *= AttackRate;
    Def *= DefRate;
    MoveSpeed *= MoveSpeedRate;
  }

  private void HandlePlayerDirection()
  {
    if (_moveDir.x < 0)
      creatureSprite.flipX = false;
    else
      creatureSprite.flipX = true;
  }
  private void MovePlayer()
  {
    if (CreatureState == ECreatureState.OnDamaged) return;
     
    Rb.velocity = Vector2.zero;

    Vector3 dir = _moveDir * MoveSpeed * Time.deltaTime;
    transform.position += dir;

    if (dir != Vector3.zero)
    {
      indicator.transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2(-dir.x, dir.y) * 180 / Mathf.PI);
      OnPlayerMove?.Invoke();
    }
    else
      Rb.velocity = Vector2.zero;
  }
  private void CollectEnv()
  {
    List<DropItemController> items = Managers.Game.CurrentMap.grid.GatherObjects(transform.position, ItemCollectDist + 0.5f);

    foreach (DropItemController item in items)
    {
      Vector3 dir = item.transform.position - transform.position;
      switch (item.itemType)
      {
        case EObjectType.DropBox:
        case EObjectType.Potion:
        case EObjectType.Magnet:
        case EObjectType.Bomb:
          if (dir.sqrMagnitude <= item.CollectDist * item.CollectDist)
          {
            item.GetItem();
          }
          break;
        default:
          float cd = item.CollectDist * CollectDistBonus;
          if (dir.sqrMagnitude <= cd * cd)
          {
            item.GetItem();
          }
          break;
      }
    }
  }
  private void LevelUp(int level = 0)
  {
    if (Level > 1)
      OnPlayerLevelUp?.Invoke();

    Skills.OnPlayerLevelUpBonus();
  }
  
  public override void Healing(float amount, bool isEffect = true)
  {
    if(amount == 0) return;
    float res = ((MaxHp * amount) * HealBonusRate);
    if (res == 0) return;
    Hp = Hp + res;
    Managers.Object.ShowDamageFont(CenterPosition, 0, res, transform);
    if (isEffect)
      Managers.Resource.Instantiate("HealEffect", transform);
  }
  
  public void OnSafetyZoneEnter(BaseController attacker)
  {
    creatureSprite.color = new Color(1, 1, 1, 1f);
  }
  public void OnSafetyZoneExit(BaseController attacker)
  {
    float damage = MaxHp * 0.1f;
    OnDamaged(attacker, null, damage);
    creatureSprite.color = new Color(1, 1, 1, 0.5f);
    OnPlayerDamaged?.Invoke();
  }

  public override void OnDamaged(BaseController attacker, SkillBase skill = null, float damage = 0)
  {
    float totalDamage = 0;
    CreatureController creatureController = attacker as CreatureController;
    if (creatureController != null)
    {
      if (skill == null)
        totalDamage = creatureController.Atk;
      else
        totalDamage = creatureController.Atk + (creatureController.Atk * skill.SkillData.damageMultiplier);
    }
    else
    {
      totalDamage = damage;
    }
    
    totalDamage *= 1 - DamageReduction;
    
    Managers.Game.CameraController.ShakeCamera();
    base.OnDamaged(attacker, null, totalDamage);
  }
  public override void OnDead()
  {
    OnPlayerDead?.Invoke();
  }

  private void HandleOnMoveDirChanged(Vector2 dir)
  {
    _moveDir = dir;
  }
  
  public override void OnDeathAnimationEnd() { }
  private IEnumerator CoSelfRecovery()
  {
    while (true)
    {
      yield return new WaitForSeconds(1f);
      Healing(HpRegen, false);
    }
  }
}

// This is for only observing data in the inspector
[Serializable]
public class PlayerStat
{
  public int dataId;
  public float hp;
  public float maxHp;
  public float maxHpBonusRate = 1;
  public float healBonusRate = 1;
  public float hpRegen;
  public float atk;
  public float attackRate = 1;
  public float def;
  public float defRate = 1;
  public float criRate;
  public float criDamage = 1.5f;
  public float damageReduction;
  public float moveSpeedRate = 1;
  public float moveSpeed;
}
