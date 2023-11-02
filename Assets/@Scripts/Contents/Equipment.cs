using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment
{
  public string key = "";
  public Data.EquipmentData equipmentData;

  public bool IsEquipped { get; set; } = false;
  public int Level { get; set; } = 1;
  public int AttackBonus { get; set; } = 0; 
  public int MaxHpBonus { get; set; } = 0;
  public bool IsOwned { get; set; } = false;
  public bool IsUpgradable { get; set; } = false;
  public bool IsConfirmed { get; set; } = false;                // 장비 획득을 확인했는지
  public bool IsEquipmentSynthesizable { get; set; } = false;   // 장비가 합성가능한지
  public bool IsSelected { get; set; } = false;                 // 합성팝업에서 선택 되어있는지
  public bool IsUnavailable { get; set; } = false;              // 합성팝업에서 선택 불가능한지
  
  // Constructor
  public Equipment(string key)
  {
    this.key = key;
    equipmentData = Managers.Data.EquipDataDic[key];
    SetInfo(Level);
    IsOwned = true;
  }
  
  private void SetInfo(int level)
  {
    Level = level;

    AttackBonus = equipmentData.atkDmgBonus + (Level - 1) * equipmentData.atkDmgBonusPerUpgrade;
    MaxHpBonus = equipmentData.maxHpBonus + (Level - 1) * equipmentData.maxHpBonusPerUpgrade;
  }

  public void LevelUp()
  {
    Level++;
    equipmentData = Managers.Data.EquipDataDic[key];
    AttackBonus = equipmentData.atkDmgBonus + (Level - 1) * equipmentData.atkDmgBonusPerUpgrade;
    MaxHpBonus = equipmentData.maxHpBonus + (Level - 1) * equipmentData.maxHpBonusPerUpgrade;
  }
}
