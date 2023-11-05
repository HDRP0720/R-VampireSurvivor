using UnityEngine;

using Data;
using static Define;

public class MagnetController : DropItemController
{
  private DropItemData _dropItemData;

  public override bool Init()
  {
    base.Init();
    itemType = EObjectType.Magnet;
    return true;
  }

  public override void GetItem()
  {
    base.GetItem();
    if (coroutine == null && this.IsValid())
      coroutine = StartCoroutine(CoCheckDistance());
  }

  public void SetInfo(Data.DropItemData data)
  {
    _dropItemData = data;
    CollectDist = BOX_COLLECT_DISTANCE;
    GetComponent<SpriteRenderer>().sprite = Managers.Resource.Load<Sprite>(_dropItemData.spriteName);
  }

  public override void CompleteGetItem()
  {
    Managers.Object.CollectAllItems();
    Managers.Object.Despawn(this);
  }
}
