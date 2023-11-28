using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
  [Serializable]
  public class MaterialData
  {
    public int dataId;
    public Define.EMaterialType materialType;
    public Define.EMaterialGrade materialGrade;
    public string nameTextID;
    public string descriptionTextID;
    public string spriteName;
  }

  [Serializable]
  public class MaterialDataLoader : ILoader<int, MaterialData>
  {
    public List<MaterialData> materials = new List<MaterialData>();
    
    public Dictionary<int, MaterialData> MakeDict()
    {
      Dictionary<int, MaterialData> dict = new Dictionary<int, MaterialData>();
      foreach (MaterialData mat in materials)
        dict.Add(mat.dataId, mat);
      return dict;
    }
  }
}
