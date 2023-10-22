using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SequenceSkill : SkillBase
{
  // Constructor
  public SequenceSkill() : base(Define.ESkillType.Sequence){ }

  public abstract void DoSkill(Action callback = null);

}