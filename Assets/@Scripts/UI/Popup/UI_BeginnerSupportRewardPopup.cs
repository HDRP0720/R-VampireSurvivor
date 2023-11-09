using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_BeginnerSupportRewardPopup : UI_Popup
{
  #region Enum For Binding UI Automatically
  enum GameObjects
  {
    ContentObject,
  }
  enum Buttons
  {
    BackgroundButton,
  }
  #endregion
  
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

    GetButton((int)Buttons.BackgroundButton).gameObject.BindEvent(OnClickBackgroundButton);

    return true;
  }
  
  private void OnClickBackgroundButton()
  {
    Managers.UI.ClosePopupUI(this);
  }
}
