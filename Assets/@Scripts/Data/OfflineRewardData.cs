using System;
using System.Collections.Generic;

namespace Data
{
  [Serializable]
  public class OfflineRewardData
  {
    public int stageIndex;
    public int reward_Gold;
    public int reward_Exp;
    public int fastReward_Scroll;
    public int fastReward_Box;
  }

  [Serializable]
  public class OfflineRewardDataLoader : ILoader<int, OfflineRewardData>
  {
    public List<OfflineRewardData> offlines = new List<OfflineRewardData>();
    public Dictionary<int, OfflineRewardData> MakeDict()
    {
      Dictionary<int, OfflineRewardData> dict = new Dictionary<int, OfflineRewardData>();
      foreach (OfflineRewardData ofr in offlines)
        dict.Add(ofr.stageIndex, ofr);
      return dict;
    }
  }
}
