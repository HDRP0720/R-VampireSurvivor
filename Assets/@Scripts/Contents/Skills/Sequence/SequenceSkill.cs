using System;

public abstract class SequenceSkill : SkillBase
{
  public int dataId;
  public string animationName;

  public abstract void DoSkill(Action callback = null);
}
