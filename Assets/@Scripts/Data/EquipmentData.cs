using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
  [Serializable]
  public class EquipmentData
  {
    public string dataId;
    public Define.EGachaRarity gachaRarity;
    public Define.EEquipmentType equipmentType;
    public Define.EEquipmentGrade equipmentGrade;
    public string nameTextID;
    public string descriptionTextID;
    public string spriteName;
    public string hpRegen;
    public int maxHpBonus;
    public int maxHpBonusPerUpgrade;
    public int atkDmgBonus;
    public int atkDmgBonusPerUpgrade;
    public int maxLevel;
    public int uncommonGradeSkill;
    public int rareGradeSkill;
    public int epicGradeSkill;
    public int legendaryGradeSkill;
    public int basicSkill;
    public Define.EMergeEquipmentType mergeEquipmentType1;
    public string mergeEquipment1;
    public Define.EMergeEquipmentType mergeEquipmentType2;
    public string mergeEquipment2;
    public string mergedItemCode;
    public int levelupMaterialID;
    public string downgradeEquipmentCode;
    public string downgradeMaterialCode;
    public int downgradeMaterialCount;
  }
  
  [Serializable]
  public class EquipmentDataLoader : ILoader<string, EquipmentData>
  {
    public List<EquipmentData> equipments = new List<EquipmentData>();
    
    public Dictionary<string, EquipmentData> MakeDict()
    {
      Dictionary<string, EquipmentData> dict = new Dictionary<string, EquipmentData>();
      foreach (EquipmentData equip in equipments)
        dict.Add(equip.dataId, equip);
      return dict;
    }
  }
}
