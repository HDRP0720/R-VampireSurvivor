using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class UI_MergeAllResultPopup : UI_Popup
{
  #region Enum For Binding UI Automatically
  enum GameObjects
  {
    MergeAlIScrollContentObject,
    ContentObject,
  }
  enum Buttons
  {
    BackgroundButton,
  }
  enum Texts
  {
    MergeAllPopupTitleText,
    BackgroundText
  }
  #endregion
  
  private List<Equipment> _items = new List<Equipment>();
  
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
  
  public void SetInfo(List<Equipment> items)
  {
    _items = items;

    Refresh();
  }
  
  private void Refresh()
  {
    GameObject container = GetObject((int)GameObjects.MergeAlIScrollContentObject);
    container.DestroyChildren();

    foreach (Equipment item in _items)
    {
      UI_EquipItem equipItem = Managers.Resource.Instantiate("UI_EquipItem", pooling: true).GetOrAddComponent<UI_EquipItem>();
      equipItem.transform.SetParent(container.transform);
      equipItem.SetInfo(item, UI_ItemParentType.EquipInventoryGroup);
    }

  }
  
  private void OnClickBackgroundButton()
  {
    Managers.UI.ClosePopupUI(this);
  }
}
