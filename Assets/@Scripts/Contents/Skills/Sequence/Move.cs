using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Move : SequenceSkill
{
  private CreatureController _owner;
  private Rigidbody2D _rb;
  private Coroutine _coroutine;
  
  private void Awake()
  {
    _owner = GetComponent<CreatureController>();
  }
  
  private IEnumerator CoMove(Action callback = null)
  {
    _rb = GetComponent<Rigidbody2D>();
    transform.GetChild(0).GetComponent<Animator>().Play(animationName);
    float elapsedTime = 0;

    while (true)
    {
      elapsedTime += Time.deltaTime;
      if (elapsedTime > 3.0f) break;

      Vector3 dir = (Managers.Game.Player.CenterPosition - _owner.CenterPosition).normalized;
      Vector2 targetPos = Managers.Game.Player.CenterPosition + dir * Random.Range(SkillData.minCoverage, SkillData.maxCoverage);
      
      if(Vector3.Distance(_rb.position, targetPos) <= 0.1f) continue;

      Vector2 dirVec = targetPos - _rb.position;
      Vector2 nextVec = dirVec.normalized * SkillData.projSpeed * Time.fixedDeltaTime;
      _rb.MovePosition(_rb.position + nextVec);

      yield return null;
    }
    
    callback?.Invoke();
  }
  
  public override void DoSkill(Action callback = null)
  {
    UpdateSkillData(dataId);
    
    if (_coroutine != null)
      StopCoroutine(_coroutine);

    _coroutine = StartCoroutine(CoMove(callback));
  }
}
