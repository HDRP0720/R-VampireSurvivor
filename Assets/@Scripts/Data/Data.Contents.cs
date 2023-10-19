using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

namespace Data
{
  #region Collect Data From PlayerData json
  // [System.Serializable]
  // public class PlayerData
  // {
  //   public int level;
  //   public int maxHp;
  //   public int attack;
  //   public int totalExp;
  // }
  //
  // [System.Serializable]
  // public class PlayerDataLoader : ILoader<int, PlayerData>
  // {
  //   public List<PlayerData> stats = new List<PlayerData>();
  //
  //   public Dictionary<int, PlayerData> MakeDict()
  //   {
  //     Dictionary<int, PlayerData> dict = new Dictionary<int, PlayerData>();
  //     foreach (PlayerData stat in stats)
  //     {
  //       dict.Add(stat.level, stat);
  //     }
  //
  //     return dict;
  //   }
  // }
  #endregion

  #region Collect Data From PlayerData xml
  public class PlayerData
  {
    [XmlAttribute] public int level;
    [XmlAttribute] public int maxHp;
    [XmlAttribute] public int attack;
    [XmlAttribute] public int totalExp;
  }
  
  [System.Serializable, XmlRoot("PlayerDatas")]
  public class PlayerDataLoader : ILoader<int, PlayerData>
  {
    [XmlElement("PlayerData")]
    public List<PlayerData> stats = new List<PlayerData>();

    public Dictionary<int, PlayerData> MakeDict()
    {
      Dictionary<int, PlayerData> dict = new Dictionary<int, PlayerData>();
      foreach (PlayerData stat in stats)
      {
        dict.Add(stat.level, stat);
      }

      return dict;
    }
  }
  #endregion

  #region Collect Data From MonsterData xml
  // public class MonsterData
  // {
  //   [XmlAttribute] public string name;
  //   [XmlAttribute] public string prefab;
  //   [XmlAttribute] public int level;
  //   [XmlAttribute] public int maxHp;
  //   [XmlAttribute] public int attack;
  //   [XmlAttribute] public float speed;
  //   
  //   // TODO: DropData
  // }
  #endregion
}
