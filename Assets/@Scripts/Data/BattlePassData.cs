using System;
using System.Collections.Generic;

namespace Data
{
  [Serializable]
  public class BattlePassData
  {
    public int passLevel;
    public int freeRewardItemId;
    public int freeRewardItemValue;
    public int rareRewardItemId;
    public int rareRewardItemValue;
    public int epicRewardItemId;
    public int epicRewardItemValue;
  }

  [Serializable]
  public class BattlePassDataLoader : ILoader<int, BattlePassData>
  {
    public List<BattlePassData> battles = new List<BattlePassData>();
    
    public Dictionary<int, BattlePassData> MakeDict()
    {
      Dictionary<int, BattlePassData> dict = new Dictionary<int, BattlePassData>();
      foreach (BattlePassData bts in battles)
        dict.Add(bts.passLevel, bts);
      return dict;
    }
  }
}
