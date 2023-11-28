using System;
using System.Collections.Generic;

namespace Data
{
  [Serializable]
  public class GachaTableData
  {
    public Define.EGachaType type;
    public List<GachaRateData> gachaRateTable = new List<GachaRateData>();
  }

  [Serializable]
  public class GachaTableDataLoader : ILoader<Define.EGachaType, GachaTableData>
  {
    public List<GachaTableData> gachaTable = new List<GachaTableData>();
    public Dictionary<Define.EGachaType, GachaTableData> MakeDict()
    {
      Dictionary<Define.EGachaType, GachaTableData> dict = new Dictionary<Define.EGachaType, GachaTableData>();
      foreach (GachaTableData gacha in gachaTable)
        dict.Add(gacha.type, gacha);
      return dict;
    }
  }
  
  public class GachaRateData
  {
    public string equipmentID;
    public float gachaRate;
    public Define.EEquipmentGrade equipGrade;
  }
}
