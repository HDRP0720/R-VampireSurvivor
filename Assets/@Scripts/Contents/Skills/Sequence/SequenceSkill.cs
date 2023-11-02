using System;

public abstract class SequenceSkill : SkillBase
{
  public int dataId;
  public string animagtionName;

  public abstract void DoSkill(Action callback = null);
}
