using UnityEngine;

using Data;
using static Define;

public class PotionController : DropItemController
{
  private DropItemData _dropItemData;

  public override bool Init()
  {
    base.Init();
    itemType = EObjectType.Potion;
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
    CollectDist = Define.POTION_COLLECT_DISTANCE;
    GetComponent<SpriteRenderer>().sprite = Managers.Resource.Load<Sprite>(_dropItemData.spriteName);

  }

  public override void CompleteGetItem()
  {
    float healAmount = 30;
    if(DicPotionAmount.TryGetValue(_dropItemData.dataId, out healAmount) == true)
      Managers.Game.Player.Healing(healAmount);
        
    Managers.Object.Despawn(this);
  }
}
