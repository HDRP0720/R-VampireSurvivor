using System.Collections;
using System.Collections.Generic;
using Data;
using UnityEngine;

public class UI_AchievementPopup : UI_Popup
{
  #region UI Feature List
  // 정보 갱신
  // AchievementScrollObject : 업적용  AchievementItem이 들어갈 부모 개체

  // 로컬라이징
  // BackgroundText : 터치하여 닫기
  // AchievementTitleText : 업적
  #endregion
  
  #region Enum For Binding UI Automatically
  enum GameObjects
  {
    ContentObject,
    AchievementScrollObject,
  }
  enum Buttons
  {
    BackgroundButton,
  }
  enum Texts
  {
    BackgroundText,
    AchievementTitleText,
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
    if (base.Init() == false)  return false;

    BindObject(typeof(GameObjects));
    BindButton(typeof(Buttons));
    BindText(typeof(Texts));

    GetButton((int)Buttons.BackgroundButton).gameObject.BindEvent(OnClickBackgroundButton);

    Refresh();
    return true;
  }
  
  private void Refresh()
  {
    if (_init == false) return;

    GetObject((int)GameObjects.AchievementScrollObject).DestroyChildren();

    foreach (AchievementData data in Managers.Achievement.GetProceedingAchievment())
    {
      UI_AchievementItem item = Managers.UI.MakeSubItem<UI_AchievementItem>(GetObject((int)GameObjects.AchievementScrollObject).transform);
      item.SetInfo(data);
    }
  }
  
  private void OnClickBackgroundButton()
  {
    Managers.UI.ClosePopupUI(this);
  }
}
