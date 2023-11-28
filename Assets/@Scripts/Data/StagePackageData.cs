using System;
using System.Collections.Generic;

namespace Data
{
  [Serializable]
  public class StagePackageData
  {
    public int stageIndex;
    public int diaValue;
    public int goldValue;
    public int randomScrollValue;
    public int goldKeyValue;
    public int productCostValue;
  }

  [Serializable]
  public class StagePackageDataLoader : ILoader<int, StagePackageData>
  {
    public List<StagePackageData> stagePackages = new List<StagePackageData>();
    
    public Dictionary<int, StagePackageData> MakeDict()
    {
      Dictionary<int, StagePackageData> dict = new Dictionary<int, StagePackageData>();
      foreach (StagePackageData stp in stagePackages)
        dict.Add(stp.stageIndex, stp);
      return dict;
    }
  }
}

