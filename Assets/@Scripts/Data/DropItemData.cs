using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
  [Serializable]
  public class DropItemData
  {
    public int dataId;
    public Define.EDropItemType dropItemType;
    public string nameTextID;
    public string descriptionTextID;
    public string spriteName;
  }
  
  [Serializable]
  public class DropItemDataLoader : ILoader<int, DropItemData>
  {
    public List<DropItemData> dropItems = new List<DropItemData>();
    
    public Dictionary<int, DropItemData> MakeDict()
    {
      Dictionary<int, DropItemData> dict = new Dictionary<int, DropItemData>();
      foreach (DropItemData dropItem in dropItems)
        dict.Add(dropItem.dataId, dropItem);
      return dict;
    }
  }
}
