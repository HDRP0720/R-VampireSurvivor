using System;
using System.Collections.Generic;

namespace Data
{
  [Serializable]
  public class CheckOutData
  {
    public int day;
    public int rewardItemId;
    public int missionTargetRewardItemValue;
  }

  [Serializable]
  public class CheckOutDataLoader : ILoader<int, CheckOutData>
  {
    public List<CheckOutData> checkouts = new List<CheckOutData>();
    public Dictionary<int, CheckOutData> MakeDict()
    {
      Dictionary<int, CheckOutData> dict = new Dictionary<int, CheckOutData>();
      foreach (CheckOutData chk in checkouts)
        dict.Add(chk.day, chk);
      return dict;
    }
  }
}
