using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Data;
using static Define;

public class SkillBook : MonoBehaviour
{
  public EObjectType ownerType;
  public List<SupportSkillData> supportSkills = new List<SupportSkillData>();
  public Dictionary<ESkillType, int> savedBattleSkills = new Dictionary<ESkillType, int>();
    
  [SerializeField] private List<SkillBase> _skillList = new List<SkillBase>();
  
  private int _sequenceIndex = 0;
  private bool _stopped = false;

  #region Properties
  public List<SkillBase> SkillList => _skillList;
  public List<SequenceSkill> SequenceSkills { get; } = new List<SequenceSkill>();
  public List<SupportSkillData> LockedSupportSkills { get; } = new List<SupportSkillData>();
  public List<SkillBase> ActivatedSkills => SkillList.Where(skill => skill.IsLearnedSkill).ToList();
  #endregion
  
  // Action
  public event Action OnUpdateSkillUI;

  public void Awake()
  {
    ownerType = GetComponent<CreatureController>().ObjectType;
  }
  
  public void LoadSkill(ESkillType skillType, int level)
  {
    AddSkill(skillType);
    
    for (int i = 0; i < level; i++)
    {
      LevelUpSkill(skillType);
    }
  }
  
  public void AddSkill(ESkillType skillType, int skillId = 0)
  {
    string className = skillType.ToString();

    if (skillType == ESkillType.FrozenHeart || skillType == ESkillType.SavageSmash || skillType == ESkillType.EletronicField)
    {
      GameObject go = Managers.Resource.Instantiate(skillType.ToString(), gameObject.transform);
      if (go != null)
      {
        SkillBase skill = go.GetOrAddComponent<SkillBase>();
        SkillList.Add(skill);
        if(savedBattleSkills.ContainsKey(skillType))
          savedBattleSkills[skillType] = skill.Level;
        else
          savedBattleSkills.Add(skillType, skill.Level);
      }
    }
    else
    {
      SequenceSkill skill = gameObject.AddComponent(Type.GetType(className)) as SequenceSkill;
      if (skill != null)
      {
        skill.ActivateSkill();
        skill.Owner = GetComponent<CreatureController>();
        skill.dataId = skillId;
        SkillList.Add(skill);
        SequenceSkills.Add(skill);
      }
      else
      {
        RepeatSkill skillbase = gameObject.GetComponent(Type.GetType(className)) as RepeatSkill;
        SkillList.Add(skillbase);
        if (savedBattleSkills.ContainsKey(skillType))
          savedBattleSkills[skillType] = skillbase.Level;
        else
          savedBattleSkills.Add(skillType, skillbase.Level);
      }
    }
  }
  
  public void StartNextSequenceSkill()
  {
    if (_stopped) return;
     
    if (SequenceSkills.Count == 0) return;

    SequenceSkills[_sequenceIndex].DoSkill(OnFinishedSequenceSkill);
  }
  private void OnFinishedSequenceSkill()
  {
    _sequenceIndex = (_sequenceIndex + 1) % SequenceSkills.Count;
    StartNextSequenceSkill();
  }
  
  public void StopSkills()
  {
    _stopped = true;
    foreach (var skill in ActivatedSkills)
    {
      skill.StopAllCoroutines();
    }
  }
  
  public void AddSupportSkill(SupportSkillData skill, bool isLoadSkill = false)
  {
    // 서포트스킬 중복가능
    skill.isPurchased = true;

    //1. 스킬 등록 없이 바로 끝내는 것들
    if (skill.supportSkillName == ESupportSkillName.Healing)
    {
      Managers.Game.Player.Healing(skill.healRate);
      return;
    }

    supportSkills.Add(skill);
    OnUpdateSkillUI?.Invoke();

    // 이미 적용된 값을 가지고 있으니 스킬데이타를 업데이트 하지않고 Add 시킨후 UI에만 추가한다.
    if (isLoadSkill == true) return;

    if (skill.supportSkillType == ESupportSkillType.General)
    {
      GeneralSupportSkillBonus(skill);
    }
    else if (skill.supportSkillType == ESupportSkillType.Special)
    {
      //배틀스킬에 영향을 미치는 스킬인경우 UpdateskilLData();
      foreach (SkillBase playerSkill in SkillList)
      {
        if (skill.supportSkillName.ToString() == playerSkill.SkillType.ToString())
          playerSkill.UpdateSkillData();
      }
    }
  }
  
  public void LevelUpSkill(ESkillType skillType)
  {
    for (int i = 0; i < SkillList.Count; i++)
    {
      if (SkillList[i].SkillType == skillType)
      {
        SkillList[i].OnLevelUp();
        if (savedBattleSkills.ContainsKey(skillType))
          savedBattleSkills[skillType] = SkillList[i].Level;
        
        OnUpdateSkillUI?.Invoke();
      }
    }
  }
  
  public void OnSkillBookChanged()
  {
    OnUpdateSkillUI?.Invoke();
  }

  #region Functions For Support Skill Bonus
  public void OnPlayerLevelUpBonus()
  {
    List<SupportSkillData> passiveSkills = supportSkills.Where(skill => skill.supportSkillType == ESupportSkillType.LevelUp).ToList();

    float moveRate = 0;
    float atkRate = 0;
    float criRate = 0;
    float cridmg = 0;
    float reduceDamage = 0;

    foreach (SupportSkillData passive in passiveSkills)
    {
      if (passive.supportSkillName == ESupportSkillName.Resurrection) continue;
      
      moveRate += passive.moveSpeedRate;
      atkRate += passive.atkRate;
      criRate += passive.criRate;
      cridmg += passive.criDmg;
      reduceDamage += passive.damageReduction;
    }

    PlayerController player = Managers.Game.Player;
    player.MoveSpeedRate += moveRate;
    player.AttackRate += atkRate;
    player.CriRate += criRate;
    player.CriDamage += cridmg;
    player.DamageReduction += reduceDamage;

    player.UpdatePlayerStat();
  }
  public void OnMonsterKillBonus()
  {
    List<SupportSkillData> passiveSkills = supportSkills.Where(skill => skill.supportSkillType == ESupportSkillType.MonsterKill).ToList();

    float dmgReduction = 0;
    float atkRate = 0;
    float healAmount = 0;
    foreach (SupportSkillData passive in passiveSkills)
    {
      if (passive.supportSkillName == ESupportSkillName.Resurrection) continue;
      
      dmgReduction += passive.damageReduction;
      atkRate += passive.atkRate;
      healAmount += passive.healRate;
    }

    PlayerController player = Managers.Game.Player;
    player.DamageReduction += dmgReduction;
    player.AttackRate += atkRate;

    player.UpdatePlayerStat();
    Managers.Game.Player.Healing(healAmount);
  }
  public void OnEliteDeadBonus()
  {
    List<SupportSkillData> passiveSkills = supportSkills.Where(skill => skill.supportSkillType == ESupportSkillType.EliteKill).ToList();

    float soulCount = 0;
    float expBonus = 0;

    foreach (SupportSkillData passive in passiveSkills)
    {
      if (passive.supportSkillName == ESupportSkillName.Resurrection)
        continue;
      soulCount += passive.soulAmount;
      expBonus += passive.expBonusRate;
    }

    PlayerController player = Managers.Game.Player;
    player.SoulCount += soulCount;
    player.ExpBonusRate += expBonus;
  }
  private void GeneralSupportSkillBonus(SupportSkillData skill)
  {
    List<SupportSkillData> generalList = supportSkills.Where(skill => skill.supportSkillType == ESupportSkillType.General).ToList();

    PlayerController player = Managers.Game.Player;
    player.CriRate += skill.criRate;
    player.MaxHpBonusRate += skill.hpRate;
    player.ExpBonusRate += skill.expBonusRate;
    player.AttackRate += skill.atkRate;
    player.DefRate += skill.defRate;
    player.DamageReduction += skill.damageReduction;
    player.SoulBonusRate += skill.soulBonusRate;
    player.HealBonusRate += skill.healBonusRate;
    player.MoveSpeedRate += skill.moveSpeedRate;
    player.HpRegen += skill.hpRegen;
    player.CriDamage += skill.criDmg;
    player.CollectDistBonus += skill.magneticRange;

    player.UpdatePlayerStat();
  }
  #endregion
  
  
  
}
