using System;

public abstract class SequenceSkill : SkillBase
{
  // Constructor
  public SequenceSkill() : base(Define.ESkillType.Sequence){ }

  public abstract void DoSkill(Action callback = null);

}
