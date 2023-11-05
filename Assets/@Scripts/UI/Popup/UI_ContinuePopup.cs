using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public class UI_ContinuePopup : UI_Popup
{
  #region UI Feature List
  // 정보 갱신
  // CountdownValueText : 10초 부터 카운트 다운하고, 0초가 되면 UI_ContinuePopup을 닫고  UI_GameResultPopup 호출 (닷트윈으로 스케일 애니메이션 연출 생각중)

  // 버튼
  // ContinueButton : 열쇠(아직 없음)를 사용하여 캐릭터가 부활하며 팝업이 닫힘
  // ContinueCostValueText : 부활에 필요한 열쇠표시 ( 필요 / 보유)
  // ADContinueButton : 광고 후 부활하며 팝업이 닫힘

  // 로컬라이징 텍스트
  // ContinuePopupTitleText : CONTINUE
  // ContinueButtonText : 계속하기
  // ADContinueText : 계속하기
  #endregion

  #region Enum For Binding UI Automatically
  enum GameObjects
  {
    ContentObject
  }
  enum Texts
  {
    ContinuePopupTitleText,
    CountdownValueText,
    ContinueButtonText,
    ContinueCostValueText,
    ADContinueText,
  }
  enum Buttons
  {
    CloseButton,
    ContinueButton,
    ADContinueButton,
  }
  #endregion
  
  private void Awake()
  {
    Init();
  }
  private void Start()
  {
    StartCoroutine(CoCountdown());
  }
  private void OnEnable()
  {
    PopupOpenAnimation(GetObject((int)GameObjects.ContentObject));
  }

  protected override bool Init()
  {
    if (base.Init() == false) return false;
 
    BindObject(typeof(GameObjects));
    BindText(typeof(Texts));
    BindButton(typeof(Buttons));
    GetButton((int)Buttons.CloseButton).gameObject.BindEvent(OnClickCloseButton);
    GetButton((int)Buttons.CloseButton).GetOrAddComponent<UI_ButtonAnimation>();
    GetButton((int)Buttons.ContinueButton).gameObject.BindEvent(OnClickContinueButton);
    GetButton((int)Buttons.ContinueButton).GetOrAddComponent<UI_ButtonAnimation>();
    GetButton((int)Buttons.ADContinueButton).gameObject.BindEvent(OnClickADContinueButton);
    GetButton((int)Buttons.ADContinueButton).GetOrAddComponent<UI_ButtonAnimation>();
    Refresh();
    return true;
  }
  
  public void SetInfo()
  {
    Refresh();
  }
  
  private void Refresh()
  {
    if (Managers.Game.ItemDictionary.TryGetValue(ID_BRONZE_KEY, out int keyCount) == true)
      GetText((int)Texts.ContinueCostValueText).text = $"1/{keyCount}";
    else
      GetText((int)Texts.ContinueCostValueText).text = $"<color=red>0</color>";

    LayoutRebuilder.ForceRebuildLayoutImmediate(GetButton((int)Buttons.ADContinueButton).gameObject.GetComponent<RectTransform>());
  }
  
  private IEnumerator CoCountdown()
  {
    int count = 10;

    while (count>0)
    {
      yield return new WaitForSecondsRealtime(1f);
      count--;
      GetText((int)Texts.CountdownValueText).text = count.ToString(); 
      if (count == 0)
        break;
    }
    yield return new WaitForSecondsRealtime(1f);

    Managers.UI.ClosePopupUI(this);
    Managers.Game.GameOver();
  }
  
  private void OnClickCloseButton()
  {
    Managers.UI.ClosePopupUI(this);
    Managers.Game.GameOver();
  }
  private void OnClickContinueButton()
  {
    Managers.Sound.PlayButtonClick();

    if (Managers.Game.ItemDictionary.TryGetValue(ID_BRONZE_KEY, out int keyCount))
    {
      Managers.Game.RemoveMaterialItem(ID_BRONZE_KEY, 1);
      Managers.Game.Player.Resurrection(1);
      Managers.UI.ClosePopupUI(this);
    }
  }
  private void OnClickADContinueButton()
  {
    Managers.Sound.PlayButtonClick();
    
    // TODO: 광고 후 부활
    // Managers.Ads.ShowRewardedAd(() =>
    // {
    //   Managers.Game.Player.Resurrection(1);
    //   Managers.UI.ClosePopupUI(this);
    // });
  }
}
