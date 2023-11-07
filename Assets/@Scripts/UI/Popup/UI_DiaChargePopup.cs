using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UI_DiaChargePopup : UI_Popup
{
  #region Enum For Binding UI Automatically
  enum GameObjects
  {
    ContentObject,
  }
  enum Buttons
  {
    BackgroundButton,
    BuyADButton,
  }
  enum Texts
  {
    BackgroundText,
    BuyADText,
    UI_DiaChargePopupTitleText,
    ADChargeValueText,
    ADRemainingValueText,
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
    BindText(typeof(Texts));

    GetButton((int)Buttons.BackgroundButton).gameObject.BindEvent(OnClickBackgroundButton);
    GetButton((int)Buttons.BuyADButton).gameObject.BindEvent(OnClickBuyADButton);
    GetButton((int)Buttons.BuyADButton).GetOrAddComponent<UI_ButtonAnimation>();
    
    Refresh();
    return true;
  }

  public void SetInfo()
  {
    Refresh();
  }

  private void Refresh()
  {
    GetText((int)Texts.ADRemainingValueText).text = $"오늘 남은 횟수 : {Managers.Game.DiaCountAds}";
  }
  
  private void OnClickBackgroundButton()
  {
    Managers.UI.ClosePopupUI(this);
  }
  private void OnClickBuyADButton()
  {
    Managers.Sound.PlayButtonClick();

    if (Managers.Game.DiaCountAds > 0)
    {
      // TODO: Show Rewarded Ad
      // Managers.Ads.ShowRewardedAd(() =>
      // {
      //   string[] spriteName = new string[1];
      //   int[] count = new int[1];
      //
      //   spriteName[0] = Managers.Data.MaterialDic[Define.ID_DIA].spriteName;
      //   count[0] = 200;
      //
      //   UI_RewardPopup rewardPopup = (Managers.UI.SceneUI as UI_LobbyScene)?.RewardPopupUI;
      //   rewardPopup.gameObject.SetActive(true);
      //   Managers.Game.DiaCountAds--;
      //   Managers.Game.ExchangeMaterial(Managers.Data.MaterialDic[Define.ID_DIA], 200);
      //   Refresh();
      //   rewardPopup.SetInfo(spriteName, count);
      // });
    }
  }
}
