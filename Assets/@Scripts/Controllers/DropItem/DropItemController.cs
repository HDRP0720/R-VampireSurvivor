using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class DropItemController : BaseController
{

  public Coroutine coroutine;
  public EObjectType itemType;
  
  public float CollectDist { get; set; } = 4.0f;
  
  public void OnEnable()
  {
    GetComponent<SpriteRenderer>().sortingOrder = Define.SOUL_SORT;
  }
  public virtual void OnDisable()
  {
    if (coroutine != null)
    {
      StopCoroutine(coroutine);
      coroutine = null;
    }
  }
  
  public override bool Init()
  {
    base.Init();
    return true;
  }
  
  public virtual void GetItem()
  {
    GetComponent<SpriteRenderer>().sortingOrder = Define.SOUL_SORT_GETITEM;
    Managers.Game.CurrentMap.grid.Remove(this);
  }
  
  public virtual void CompleteGetItem() { }
  
  public IEnumerator CoCheckDistance()
  {
    while (this.IsValid() == true)
    {
      float dist = Vector3.Distance(gameObject.transform.position, Managers.Game.Player.PlayerCenterPos);
      transform.position = Vector3.MoveTowards(transform.position, Managers.Game.Player.PlayerCenterPos, Time.deltaTime * 15.0f);
      if (dist < 1f)
      {
        CompleteGetItem();
        yield break;
      }

      yield return new WaitForFixedUpdate();
    }
  }
}
