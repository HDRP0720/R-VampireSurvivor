using System;
using System.Collections;
using UnityEngine;
using static Define;
using Random = UnityEngine.Random;

public class Dash : SequenceSkill
{
  private Rigidbody2D _rb;
  private Coroutine _coroutine;
  
  private void Awake()
  {
    SkillType = ESkillType.Dash;
    animationName = "Dash";
  }
  
  private IEnumerator CoDash(Action callback = null)
  {
    _rb = GetComponent<Rigidbody2D>();
    float elapsed = 0;
    Vector2 targetPos = Managers.Game.Player.CenterPosition;
    
    GameObject obj = Managers.Resource.Instantiate("SkillRange", pooling : true);
    obj.transform.SetParent(transform);
    obj.transform.localPosition = Vector3.zero;
    SkillRange skillRange = obj.GetOrAddComponent<SkillRange>();
    
    while (true)
    {
      elapsed += Time.deltaTime;
      if(elapsed > SkillData.duration) break;
      
      Vector3 dir = (Vector2)Managers.Game.Player.CenterPosition - _rb.position;
      targetPos = Managers.Game.Player.CenterPosition + dir.normalized * SkillData.maxCoverage;
      skillRange.SetInfo(dir, targetPos, Vector3.Distance(_rb.position, targetPos));
      yield return null;
    }
    
    Managers.Resource.Destroy(obj);
    transform.GetChild(0).GetComponent<Animator>().Play(animationName);
    while (Vector3.Distance(_rb.position, targetPos) > 0.3f)
    {
      Vector2 dirVec = targetPos - _rb.position;
      Vector2 nextVec = dirVec.normalized * SkillData.projSpeed * Time.fixedDeltaTime;
      _rb.MovePosition(_rb.position + nextVec);
            
      yield return null;
    }
    
    yield return new WaitForSeconds(SkillData.attackInterval);
    callback?.Invoke();
  }
  
  public override void DoSkill(Action callback = null)
  {
    UpdateSkillData(dataId);
    
    if(_coroutine != null)
      StopCoroutine(_coroutine);

    _coroutine = StartCoroutine(CoDash(callback));
  }
}
