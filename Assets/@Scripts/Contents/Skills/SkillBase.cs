using System;
using System.Collections;
using UnityEngine;

using Data;
using static Define;

public class SkillBase : BaseController
{
  private ESkillType _skillType;
  private SkillData _skillData;
  private int _level = 0;

  #region Properties
  public CreatureController Owner { get; set; }
  public ESkillType  SkillType
  {
    get => _skillType;
    protected set => _skillType = value;
  }
  public SkillData SkillData 
  {
    get => _skillData;
    private set => _skillData = value;
  }
  public int Level
  {
    get => _level;
    set => _level = value;
  }
  public float TotalDamage { get; set; } = 0;
  public bool IsLearnedSkill => Level > 0;
  public int Damage { get; set; } = 100;
  #endregion
  
  public SkillData UpdateSkillData(int dataId = 0)
  {
    int id = 0;
    if (dataId == 0)
      id = Level < 2 ? (int)SkillType : (int)SkillType + Level - 1;
    else
      id = dataId;

    SkillData skillData = new SkillData();
    if (Managers.Data.SkillDic.TryGetValue(id, out skillData) == false)
      return SkillData;

    foreach (SupportSkillData support in Managers.Game.Player.Skills.supportSkills)
    {
      if (SkillType.ToString() == support.supportSkillName.ToString())
      {
        skillData.projectileSpacing += support.projectileSpacing;
        skillData.duration += support.duration;
        skillData.numProjectiles += support.numProjectiles;
        skillData.attackInterval += support.attackInterval;
        skillData.numBounce += support.numBounce;
        skillData.projRange += support.projRange;
        skillData.rotateSpeed += support.rotateSpeed;
        skillData.scaleMultiplier += support.scaleMultiplier;
        skillData.numPenetrations += support.numPenetrations;
      }
    }
    SkillData = skillData;
    OnChangedSkillData();
    return SkillData;
  }
  protected virtual void OnChangedSkillData() { }
  public virtual void ActivateSkill()
  {
    UpdateSkillData();
  }
  public virtual void OnLevelUp()
  {
    if (Level == 0) ActivateSkill();
      
    Level++;
    
    UpdateSkillData();
  }
  protected virtual void GenerateProjectile(CreatureController owner, string prefabName, Vector3 startPos, Vector3 dir, Vector3 targetPos, SkillBase skill)
  {
    ProjectileController pc = Managers.Object.Spawn<ProjectileController>(startPos, prefabName: prefabName);
    pc.SetInfo(owner, startPos, dir, targetPos, skill);
  }
}

[Serializable]
public class SkillStat
{
  public ESkillType skillType;
  public int level;
  public float maxHp;
  public SkillData skillData;
}
