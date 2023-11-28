using System;
using System.Collections.Generic;

namespace Data
{
  [Serializable]
  public class AccountPassData
  {
    public int accountLevel;
    public int freeRewardItemId;
    public int freeRewardItemValue;
    public int rareRewardItemId;
    public int rareRewardItemValue;
    public int epicRewardItemId;
    public int epicRewardItemValue;
  }

  [Serializable]
  public class AccountPassDataLoader : ILoader<int, AccountPassData>
  {
    public List<AccountPassData> accounts = new List<AccountPassData>();
    
    public Dictionary<int, AccountPassData> MakeDict()
    {
      Dictionary<int, AccountPassData> dict = new Dictionary<int, AccountPassData>();
      foreach (AccountPassData aps in accounts)
        dict.Add(aps.accountLevel, aps);
      return dict;
    }
  }
}

