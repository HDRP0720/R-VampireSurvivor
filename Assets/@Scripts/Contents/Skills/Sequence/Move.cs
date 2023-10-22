using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Move : SequenceSkill
{
  private Rigidbody2D _rb;
  private Coroutine _coroutine;
  private float _speed = 2.0f;
  private string _animationName = "Moving";
  
  public override void DoSkill(Action callback = null)
  {
    if (_coroutine != null)
      StopCoroutine(_coroutine);

    _coroutine = StartCoroutine(CoMove(callback));
  }

  private IEnumerator CoMove(Action callback = null)
  {
    _rb = GetComponent<Rigidbody2D>();
    this.GetComponent<Animator>().Play(_animationName);
    float elapsedTime = 0;

    while (true)
    {
      elapsedTime += Time.deltaTime;
      if (elapsedTime > 5.0f) break;

      Vector3 dir = ((Vector2)Managers.Game.Player.transform.position - _rb.position).normalized;
      Vector2 targetPos = Managers.Game.Player.transform.position + dir * Random.Range(1, 4);
      
      if(Vector3.Distance(_rb.position, targetPos) <= 0.2f) continue;

      Vector2 dirVec = targetPos - _rb.position;
      Vector2 nextVec = dirVec.normalized * _speed * Time.fixedDeltaTime;
      _rb.MovePosition(_rb.position + nextVec);

      yield return null;
    }
    
    callback?.Invoke();
  }
}
