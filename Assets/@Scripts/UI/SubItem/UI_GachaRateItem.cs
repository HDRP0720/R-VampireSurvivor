using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Data;
using static Define;

public class UI_GachaRateItem : UI_Base
{
  #region Enums For Binding UI Automatically
  enum Texts
  {
    EquipmentNameValueText,
    EquipmentRateValueText,
  }
  enum Images
  {
    BackgroundImage,
  }
  #endregion
  
  private GachaRateData _gachaRateData;

  protected override bool Init()
  {
    if (base.Init() == false) return false;

    BindText(typeof(Texts));
    BindImage(typeof(Images));

    Refresh();
    return true;
  }

  public void SetInfo(GachaRateData gachaRateData)
  {
        
    _gachaRateData = gachaRateData;

    Refresh();
    transform.localScale = Vector3.one;
  }

  private void Refresh()
  {
    if (_init == false) return;
    
    string weaponName = Managers.Data.EquipDataDic[_gachaRateData.equipmentID].nameTextID;
    GetText((int)Texts.EquipmentNameValueText).text = weaponName;
    GetText((int)Texts.EquipmentRateValueText).text = _gachaRateData.gachaRate.ToString("P2");
    switch (_gachaRateData.equipGrade)
    {
      case EEquipmentGrade.Common:
        GetImage((int)Images.BackgroundImage).color = EquipmentUIColors.Common;
        break;
      case EEquipmentGrade.Uncommon:
        GetImage((int)Images.BackgroundImage).color = EquipmentUIColors.Uncommon;
        break;
      case EEquipmentGrade.Rare:
        GetImage((int)Images.BackgroundImage).color = EquipmentUIColors.Rare;
        break;
      case EEquipmentGrade.Epic:
        GetImage((int)Images.BackgroundImage).color = EquipmentUIColors.Epic;
        break;
      default:
        break;
    }
  }
}
