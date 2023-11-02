using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Data;
using static Define;

public class PlayerController : CreatureController
{
  public PlayerStat statViewer = new PlayerStat();
  
  [SerializeField] private Transform _indicator;
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
    get { return Managers.Game.ContinueInfo.SoulCount; }

    set
    {
      Managers.Game.ContinueInfo.SoulCount = Mathf.Round(value);

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
        int currentLevelExp = currentLevelData.TotalExp;
        int nextLevelExp = currentLevelExp;
        int previousLevelExp = 0;

        LevelData prevLevelData;
        if (Managers.Data.LevelDataDic.TryGetValue(Level - 1, out prevLevelData))
        {
          previousLevelExp = prevLevelData.TotalExp;
        }

        // 만렙이 아닌 경우
        LevelData nextLevelData;
        if (Managers.Data.LevelDataDic.TryGetValue(Level + 1, out nextLevelData))
        {
          nextLevelExp = nextLevelData.TotalExp;
        }

        return (float)(Exp - previousLevelExp) / (currentLevelExp - previousLevelExp);
      }

      return 0f;
    }
  }
  
  
  
  
  

  #endregion
  
  #region Action
  public Action OnPlayerDataUpdated;
  public Action OnPlayerLevelUp;
  public Action OnPlayerDead;
  public Action OnPlayerDamaged;
  public Action OnPlayerMove;
  #endregion

  private void Update()
  {
    MovePlayer();
    CollectGems();
  }
  private void OnDestroy()
  {
    if(Managers.Game != null)
      Managers.Game.OnMoveDirChanged -= HandleOnMoveDirChanged;
  }

  public override bool Init()
  {
    if (base.Init() == false) return false;

    _speed = 5.0f;
    Managers.Game.OnMoveDirChanged += HandleOnMoveDirChanged;
    
    // TODO:
    Skills.AddSkill<FireballSkill>(transform.position);
    Skills.AddSkill<EgoSword>(indicator.position);
    
    return true;
  }

  private void MovePlayer()
  {
    Vector2 dir = _moveDir * (_speed * Time.deltaTime);
    transform.position += new Vector3(dir.x, dir.y, 0);

    if (_moveDir != Vector2.zero)
      indicator.eulerAngles = new Vector3(0, 0, Mathf.Atan2(-dir.x, dir.y) * 180 / Mathf.PI);

    GetComponent<Rigidbody2D>().velocity = Vector2.zero;
  }

  private void CollectGems()
  {
    float sqrCollectDist = EnvCollectDist * EnvCollectDist;
    
    var findGems = GameObject.Find("Grid").GetComponent<GridController>()
      .GatherObjects(transform.position, EnvCollectDist + 0.5f);

    foreach (GameObject go in findGems)
    {
      GemController gem = go.GetComponent<GemController>();
      Vector3 dir = gem.transform.position - transform.position;
      if (dir.sqrMagnitude <= sqrCollectDist)
      {
        Managers.Game.Gem += 1;
        Managers.Object.Despawn(gem);
      }
    }
  }
  
  private void LevelUp(int level = 0)
  {
    if (Level > 1)
      OnPlayerLevelUp?.Invoke();

    Skills.OnPlayerLevelUpBonus();
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
  
  private void OnCollisionEnter2D(Collision2D other)
  {
    MonsterController target = other.gameObject.GetComponent<MonsterController>();
    if (target == null) return;
    
    
  }

  public override void OnDamaged(BaseController attacker, int damage)
  {
    base.OnDamaged(attacker, damage);
    
    // TODO: This is temp code
    CreatureController cc = attacker as CreatureController;
    cc?.OnDamaged(this, 10000);
  }

  private void HandleOnMoveDirChanged(Vector2 dir)
  {
    _moveDir = dir;
  }
}

// This is for viewing the data in the inspector
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
