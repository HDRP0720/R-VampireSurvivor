using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UI_UserInfoItem : UI_Base
{
  #region UI Feature List
  // 정보 갱신
  // UserLevelText에 유저 레벨 동기화 (레벨 업 시 리프레시) 
  // UserExpSliderObject 유저 경험치 동기화 (경험치 습득 시 리프레시)
  // 스테미나 StaminaValueText : 현재 스테이나 / 맥스 스테미나 표시 (N / 30)
  // 다이아 DiaValueText, 골드 GoldValueText등의 데이터 동기화 (각 재화 습득 시 리프레시 필요)
  #endregion

  #region Enum For Binding UI Automatically
  enum GameObjects
  {
    //UserExpSliderObject, 
  }
  enum Buttons
  {
    StaminaButton,
    DiaButton,
    //GoldButton,
  }
  enum Texts
  {
    //UserLevelText, // 유저 계정 레벨
    StaminaValueText,
    DiaValueText,
    GoldValueText,
  }
  #endregion

  protected override bool Init()
  {
    if (base.Init() == false) return false;
    
    #region Object Bind
    BindObject(typeof(GameObjects));
    BindButton(typeof(Buttons));
    BindText(typeof(Texts));

    GetButton((int)Buttons.StaminaButton).gameObject.BindEvent(OnClickStaminaButton);
    GetButton((int)Buttons.StaminaButton).GetOrAddComponent<UI_ButtonAnimation>();
    GetButton((int)Buttons.DiaButton).gameObject.BindEvent(OnClickDiaButton);
    GetButton((int)Buttons.DiaButton).GetOrAddComponent<UI_ButtonAnimation>();
    //GetButton((int)Buttons.GoldButton).gameObject.BindEvent(OnClickGoldButton);
    //GetButton((int)Buttons.GoldButton).GetOrAddComponent<UI_ButtonAnimation>();
    #endregion

    Refresh();
    return true;
  }
  
  public void SetInfo()
  {
    Refresh();
  }
  
  private void Refresh()
  {

  }
  
  private void OnClickStaminaButton() 
  {
    Managers.Sound.PlayButtonClick();
    Managers.UI.ShowPopupUI<UI_StaminaChargePopup>();
  }

  private void OnClickDiaButton()
  {
    Managers.Sound.PlayButtonClick();
    Managers.UI.ShowPopupUI<UI_DiaChargePopup>();
  }

  // private void OnClickGoldButton()
  // {
  //   // 골드 구매 상점으로 이동
  // }
}
