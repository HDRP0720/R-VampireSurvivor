using System.Linq;
using System.Collections.Generic;

using Data;
using static Define;

public class UI_GachaListPopup : UI_Popup
{
  #region UI Feature List
  // 정보 갱신
  // GachaInfoContentObject : 장비 확률을 표시할 GachaGradeListItem이 들어갈 부모 개체

  // 로컬라이징
  // GachaListPopupTitleText : 상품 확률
  #endregion
  
  #region Enum For Binding UI Automatically
  enum GameObjects
  {
    ContentObject,
    GachaInfoContentObject,
    CommonGachaGradeRateItem,
    CommonGachaRateListObject,
    UncommonGachaGradeRateItem,
    UncommonGachaRateListObject,        
    RareGachaGradeRateItem,
    RareGachaRateListObject,
    EpicGachaGradeRateItem,
    EpicGachaRateListObject,
  }
  enum Buttons
  {
    BackgroundButton,
  }
  enum Texts
  {
    GachaListPopupTitleText,
    CommonGradeTitleText,
    CommonGradeRateValueText,        
    UncommonGradeTitleText,
    UncommonGradeRateValueText,
    RareGradeTitleText,
    RareGradeRateValueText,
    EpicGradeTitleText,
    EpicGradeRateValueText,
  }
  #endregion
  
  private EGachaType _gachaType;
  
  private void Awake()
  {
    Init();
  }
  private void OnEnable()
  {
    PopupOpenAnimation(GetObject((int)GameObjects.ContentObject));
  }

  protected override bool Init()
  {
    if (base.Init() == false) return false;

    BindObject(typeof(GameObjects));
    BindButton(typeof(Buttons));
    BindText(typeof(Texts));

    GetButton((int)Buttons.BackgroundButton).gameObject.BindEvent(OnClickBackgroundButton);

    Refresh();
    return true;
  }
  
  public void SetInfo(EGachaType gachaType)
  {
    _gachaType = gachaType;
    Refresh();
  }

  private void Refresh()
  {
    if (_init == false) return;

    if (_gachaType == EGachaType.None) return;
    
    float commonRate = 0f;
    float uncommonRate = 0f;
    float rareRate = 0f;
    float epicRate = 0f;

    GetObject((int)GameObjects.CommonGachaRateListObject).DestroyChildren();
    GetObject((int)GameObjects.UncommonGachaRateListObject).DestroyChildren();
    GetObject((int)GameObjects.RareGachaRateListObject).DestroyChildren();
    GetObject((int)GameObjects.EpicGachaRateListObject).DestroyChildren();
 
    List<GachaRateData> list = Managers.Data.GachaTableDataDic[_gachaType].gachaRateTable.ToList();
    list.Reverse();
    
    foreach (GachaRateData item in Managers.Data.GachaTableDataDic[_gachaType].gachaRateTable)
    {
      switch(Managers.Data.EquipDataDic[item.equipmentID].equipmentGrade)
      {
        case EEquipmentGrade.Common:
          commonRate += item.gachaRate;
          UI_GachaRateItem commonItem = Managers.Resource.Instantiate("UI_GachaRateItem", pooling: true).GetOrAddComponent<UI_GachaRateItem>();
          commonItem.transform.SetParent(GetObject((int)GameObjects.CommonGachaRateListObject).transform);
          commonItem.SetInfo(item);
          break;

        case EEquipmentGrade.Uncommon:
          uncommonRate += item.gachaRate;
          UI_GachaRateItem uncommonItem = Managers.Resource.Instantiate("UI_GachaRateItem", pooling: true).GetOrAddComponent<UI_GachaRateItem>();
          uncommonItem.transform.SetParent(GetObject((int)GameObjects.UncommonGachaRateListObject).transform);
          uncommonItem.SetInfo(item);
          break;

        case EEquipmentGrade.Rare:
          rareRate += item.gachaRate;
          UI_GachaRateItem rareItem = Managers.Resource.Instantiate("UI_GachaRateItem", pooling: true).GetOrAddComponent<UI_GachaRateItem>();
          rareItem.transform.SetParent(GetObject((int)GameObjects.RareGachaRateListObject).transform);
          rareItem.SetInfo(item);
          break;

        case EEquipmentGrade.Epic:
          epicRate += item.gachaRate;
          UI_GachaRateItem epicItem = Managers.Resource.Instantiate("UI_GachaRateItem", pooling: true).GetOrAddComponent<UI_GachaRateItem>();
          epicItem.transform.SetParent(GetObject((int)GameObjects.EpicGachaRateListObject).transform);
          epicItem.SetInfo(item);
          break;
      }
    }
    
    GetText((int)Texts.CommonGradeRateValueText).text = commonRate.ToString("P2");
    GetText((int)Texts.UncommonGradeRateValueText).text = uncommonRate.ToString("P2");
    GetText((int)Texts.RareGradeRateValueText).text = rareRate.ToString("P2");
    GetText((int)Texts.EpicGradeRateValueText).text = epicRate.ToString("P2");
    gameObject.SetActive(true);
  }
  
  private void OnClickBackgroundButton()
  {
    Managers.Sound.PlayPopupClose();
    gameObject.SetActive(false);
  }
}
