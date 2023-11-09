using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.VisualScripting;

using Data;
using static Define;

public class UI_FastRewardPopup : UI_Popup
{
  #region UI Feature List
  // 정보 갱신
  // RewardItemScrollContentObject 리워드 아이템 들어갈 부모개체
  // ClaimCostValueText : 빠른 보상을 받을 시 소모되는 스테미너 값
  // EemainingCountValueText : 하루 보상 리미트 횟수

  // 로컬라이징 텍스트
  // FastRewardPopupTitleText : 빠른 보상
  // FastRewardCommentText : 보상 수익 300분 즉시 적립
  // ADFreeText : Free
  #endregion
  
  #region Enum For Binding UI Automatically
  enum GameObjects
  {
    ContentObject,
    ItemContainer, 
  }
  enum Buttons
  {
    BackgroundButton,
    ADFreeButton,
    ClaimButton,
  }
  enum Texts
  {
    BackgroundText,
    FastRewardPopupTitleText,
    FastRewardCommentText,
    ADFreeText,
    ClaimCostValueText,
    EemainingCommentText,
    EemainingCountValueText,
  }
  #endregion
  
  private OfflineRewardData _offlineRewardData;
  private bool _isClaim = false;
  
  private void Awake()
  {
    Init();
  }

  protected override bool Init()
  {
    if (base.Init() == false) return false;

    BindObject(typeof(GameObjects));
    BindButton(typeof(Buttons));
    BindText(typeof(Texts));

    GetButton((int)Buttons.BackgroundButton).gameObject.BindEvent(OnClickBackgroundButton);
    GetButton((int)Buttons.ADFreeButton).gameObject.BindEvent(OnClickADFreeButton);
    GetButton((int)Buttons.ADFreeButton).GetOrAddComponent<UI_ButtonAnimation>();
    GetButton((int)Buttons.ClaimButton).gameObject.BindEvent(OnClickClaimButton);

    GetButton((int)Buttons.ClaimButton).GetComponent<Image>().color = Color.white;

    return true;
  }
  
  public void SetInfo(OfflineRewardData offlineReward)
  {
    _offlineRewardData = offlineReward;
    Refresh();
  }

  private void Refresh()
  {
    GameObject container = GetObject((int)GameObjects.ItemContainer);
    container.DestroyChildren();

    if (Managers.Game.Stamina >= 15 && Managers.Game.FastRewardCountStamina > 0)
    {
      GetButton((int)Buttons.ClaimButton).GetComponent<Image>().color = Utils.HexToColor("50D500");
      _isClaim = true;
      GetButton((int)Buttons.ClaimButton).GetOrAddComponent<UI_ButtonAnimation>();
    }
    else 
    {
      GetButton((int)Buttons.ClaimButton).GetComponent<Image>().color = Utils.HexToColor("989898");
      _isClaim = false;
    }

    UI_MaterialItem item = Managers.UI.MakeSubItem<UI_MaterialItem>(container.transform);
    int count = (_offlineRewardData.reward_Gold) * 5;
    item.SetInfo(GOLD_SPRITE_NAME, count);

    UI_MaterialItem scroll = Managers.UI.MakeSubItem<UI_MaterialItem>(container.transform);
    scroll.SetInfo("Scroll_Random_Icon", _offlineRewardData.fastReward_Scroll);

    UI_MaterialItem box = Managers.UI.MakeSubItem<UI_MaterialItem>(container.transform);
    box.SetInfo("Key_Silver_Icon", _offlineRewardData.fastReward_Box);

    GetText((int)Texts.EemainingCountValueText).text = Managers.Game.FastRewardCountStamina.ToString();

    LayoutRebuilder.ForceRebuildLayoutImmediate(GetButton((int)Buttons.ADFreeButton).gameObject.GetComponent<RectTransform>());
    LayoutRebuilder.ForceRebuildLayoutImmediate(GetButton((int)Buttons.ClaimButton).gameObject.GetComponent<RectTransform>());
  }
  
  private void OnClickBackgroundButton()
  {
    Managers.UI.ClosePopupUI(this);
  }
  private void OnClickADFreeButton()
  {
    Managers.Sound.PlayButtonClick();

    if (Managers.Game.FastRewardCountAds > 0)
    {
      Managers.Game.FastRewardCountAds--;
      // TODO : 광고 구현
      // Managers.Ads.ShowRewardedAd(() =>
      // {
      //   Managers.Time.GiveFastOfflineReward(_offlineRewardData);
      //   Managers.UI.ClosePopupUI(this);
      // });
    }
    else
    {
      Managers.UI.ShowToast("더이상 받을 수 없습니다."); 
    }
  }
  private void OnClickClaimButton()
  {
    Managers.Sound.PlayButtonClick();
    if (Managers.Game.Stamina >= 15 && Managers.Game.FastRewardCountStamina > 0 && _isClaim)
    {
      Managers.Game.Stamina -= 15;
      Managers.Game.FastRewardCountStamina--;
      Managers.Time.GiveFastOfflineReward(_offlineRewardData);
      Managers.UI.ClosePopupUI(this);
      Refresh();
    }
    else
    {
      return;
    }
  }
}

