using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class SoulController : DropItemController
{
  public int _soudCount = 5;
  Coroutine _coMoveToPlayer;
  
  public override void OnDisable()
  {
    if (_coMoveToPlayer != null)
    {
      StopCoroutine(_coMoveToPlayer);
      _coMoveToPlayer = null;
    }
  }
  
  public override bool Init()
  {
    base.Init();
   
    _soudCount = Define.STAGE_SOULCOUNT; //소울 갯수당 획득량
    return true;
  }
  
  public override void GetItem()
  {
    base.GetItem();
    if (_coMoveToPlayer == null && this.IsValid())
    {
      Sequence seq = DOTween.Sequence();
      Vector3 dir = (transform.position - Managers.Game.SoulDestination).normalized;
      Vector3 target = transform.position + dir * 0.5f;
      seq.Append(transform.DOMove(target, 0.4f).SetEase(Ease.Linear)).OnComplete(() =>
      {
        _coMoveToPlayer = StartCoroutine(CoMoveToPlayer());
      });
    }
  }
  
  public IEnumerator CoMoveToPlayer()
  {
    float speed = 17f;
    float acceleration = 8.5f;

    while (this.IsValid())
    {
      float dist = Vector3.Distance(gameObject.transform.position, Managers.Game.SoulDestination);

      // 현재 시간에 따른 속도 계산
      speed += acceleration * Time.deltaTime;

      // 목적지 방향으로 일정한 속도로 이동
      Vector3 direction = (Managers.Game.SoulDestination - transform.position).normalized;
      transform.position += direction * speed * Time.deltaTime;

      if (dist < 0.4f)
      {
        string soundClipName = "SoulGet_01";
        Managers.Sound.Play(Define.ESound.Effect, soundClipName);
        Managers.Game.Player.SoulCount += _soudCount * Managers.Game.Player.SoulBonusRate;
        Managers.Object.Despawn(this);
        yield break;
      }

      yield return new WaitForFixedUpdate();
    }
  }
}
