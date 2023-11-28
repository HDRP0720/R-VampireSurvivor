using System;
using System.Collections.Generic;

namespace Data
{
  [Serializable]
  public class CreatureData
  {
    public int dataId;
    public string descriptionTextID;
    public string prefabLabel;
    public float maxHp;
    public float maxHpBonus;
    public float atk;
    public float atkBonus;
    public float def;
    public float moveSpeed;
    public float totalExp;
    public float hpRate;
    public float atkRate;
    public float defRate;
    public float moveSpeedRate;
    public string iconLabel;
    public List<int> skillTypeList;//InGameSkills를 제외한 추가스킬들
  }

  [Serializable]
  public class CreatureDataLoader : ILoader<int, CreatureData>
  {
    public List<CreatureData> creatures = new List<CreatureData>();
    public Dictionary<int, CreatureData> MakeDict()
    {
      Dictionary<int, CreatureData> dict = new Dictionary<int, CreatureData>();
      foreach (CreatureData creature in creatures)
        dict.Add(creature.dataId, creature);
      return dict;
    }
  }
}

