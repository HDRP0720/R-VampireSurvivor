using System.Collections;
using UnityEngine;

using static Define;

public abstract class RepeatSkill : SkillBase
{
  private Coroutine _coroutine;
  
  // Property
  public float CoolTime { get; set; } = 1.0f;
  
  public override bool Init()
  {
    base.Init();
    return true;
  }

  public override void ActivateSkill()
  {
    base.ActivateSkill();
    
    if(_coroutine != null) StopCoroutine(_coroutine);
    
    gameObject.SetActive(true);
    _coroutine = StartCoroutine(CoStartSkill());
  }
  protected virtual IEnumerator CoStartSkill()
  {
    WaitForSeconds wait = new WaitForSeconds(SkillData.coolTime);
    yield return wait;
    
    while (true)
    {
      if(SkillData.coolTime != 0)
        Managers.Sound.Play(ESound.Effect, SkillData.castingSound);
      DoSkillJob();
      yield return wait;
    }
  }
  
  protected abstract void DoSkillJob();
}
