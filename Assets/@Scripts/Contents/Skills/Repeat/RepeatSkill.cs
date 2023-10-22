using System.Collections;
using UnityEngine;

public abstract class RepeatSkill : SkillBase
{
  private Coroutine _coSkill;
  
  // Property
  public float CoolTime { get; set; } = 1.0f;
  
  // Constructor
  public RepeatSkill() : base(Define.ESkillType.Repeat) { }

  public override void ActivateSkill()
  {
    if(_coSkill != null)
      StopCoroutine(_coSkill);

    _coSkill = StartCoroutine(CoStartSkill());
  }
  protected virtual IEnumerator CoStartSkill()
  {
    WaitForSeconds wait = new WaitForSeconds(CoolTime);
    while (true)
    {
      DoSkillJob();
      yield return wait;
    }
  }
  protected abstract void DoSkillJob();
}
