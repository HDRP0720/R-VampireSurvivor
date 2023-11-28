using System;
using System.Collections.Generic;

namespace Data
{
  [Serializable]
  public class DailyShopData
  {
    public int index;
    public int buyItemId;
    public int costItemId;
    public int costValue;
    public float discountValue;
  }

  [Serializable]
  public class DailyShopDataLoader : ILoader<int, DailyShopData>
  {
    public List<DailyShopData> dailys = new List<DailyShopData>();
    
    public Dictionary<int, DailyShopData> MakeDict()
    {
      Dictionary<int, DailyShopData> dict = new Dictionary<int, DailyShopData>();
      foreach (DailyShopData dai in dailys)
        dict.Add(dai.index, dai);
      return dict;
    }
  }
}
