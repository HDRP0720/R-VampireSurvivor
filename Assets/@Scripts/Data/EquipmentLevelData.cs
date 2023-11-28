using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
  [Serializable]
  public class EquipmentLevelData
  {
    public int level;
    public int upgradeCost;
    public int upgradeRequiredItems;
  }

  [Serializable]
  public class EquipmentLevelDataLoader : ILoader<int, EquipmentLevelData>
  {
    public List<EquipmentLevelData> levels = new List<EquipmentLevelData>();
    
    public Dictionary<int, EquipmentLevelData> MakeDict()
    {
      Dictionary<int, EquipmentLevelData> dict = new Dictionary<int, EquipmentLevelData>();

      foreach (EquipmentLevelData levelData in levels)
        dict.Add(levelData.level, levelData);
      return dict;
    }
  }
}
