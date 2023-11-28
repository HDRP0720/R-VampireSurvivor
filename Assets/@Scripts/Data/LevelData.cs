using System;
using System.Collections.Generic;

namespace Data
{
  [Serializable]
  public class LevelData
  {
    public int level;
    public int totalExp;
    public int requiredExp;
  }

  [Serializable]
  public class LevelDataLoader : ILoader<int, LevelData>
  {
    public List<LevelData> levels = new List<LevelData>();
    public Dictionary<int, LevelData> MakeDict()
    {
      Dictionary<int, LevelData> dict = new Dictionary<int, LevelData>();
      foreach (LevelData levelData in levels)
        dict.Add(levelData.level, levelData);
      return dict;
    }
  }
}

