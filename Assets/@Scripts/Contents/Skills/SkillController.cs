using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillController : BaseController
{
  public Define.ESkillType SkillType { get; set; } = Define.ESkillType.None;
  public Data.SkillData SkillData { get; protected set; }

  private Coroutine _coDestroy;

  public void StartDestroy(float delaySeconds)
  {
    StopDestroy();
    _coDestroy = StartCoroutine(CoDestroy(delaySeconds));
  }

  public void StopDestroy()
  {
    if (_coDestroy != null)
    {
      StopCoroutine(_coDestroy);
      _coDestroy = null;
    }
  }

  private IEnumerator CoDestroy(float delaySeconds)
  {
    yield return new WaitForSeconds(delaySeconds);
    
    if(this.IsValid())
      Managers.Object.Despawn(this);
  }
}
