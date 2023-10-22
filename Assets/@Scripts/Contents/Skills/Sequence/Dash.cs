using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Dash : SequenceSkill
{
  private Rigidbody2D _rb;
  private Coroutine _coroutine;
  
  // Properties
  private float WaitTime { get; } = 1.0f;
  private float Speed { get; } = 10.0f;
  private string AnimationName { get; } = "Charge";
  
  public override void DoSkill(Action callback = null)
  {
    if(_coroutine != null)
      StopCoroutine(_coroutine);

    _coroutine = StartCoroutine(CoDash(callback));
  }

  private IEnumerator CoDash(Action callback = null)
  {
    _rb = GetComponent<Rigidbody2D>();
    yield return new WaitForSeconds(WaitTime);
    this.GetComponent<Animator>().Play(AnimationName);
    Vector3 dir = ((Vector2)Managers.Game.Player.transform.position - _rb.position).normalized;
    Vector2 targetPos = Managers.Game.Player.transform.position + dir * Random.Range(1, 5);
    
    while (Vector3.Distance(_rb.position, targetPos) > 0.2f)
    {
      Vector2 dirVec = targetPos - _rb.position;
      Vector2 nextVec = dirVec.normalized * Speed * Time.fixedDeltaTime;
      _rb.MovePosition(_rb.position + nextVec);
      
      yield return null;
    }
    
    callback?.Invoke();
  }
}
