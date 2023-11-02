using System;
using UnityEngine;

public class UI_RewardPopup : UI_Popup
{
  #region UI Feature List
  // 정보 갱신
  // RewardItemScrollContentObject : 리워드 아이템 들어갈 부모개체

  // 호출되는 곳
  // 미션 팝업 : 미션 완료 보상
  // 오프라인 보상 팝업 : 보상 수령 시
  // 빠른 보상 팝업 : 보상 수령 시
  // 스테이지 보상 팝업 : 클리어 보상 수령 시
  // 상점 페이지 : 상품 구매
  // 장비 초기화 팝업 : 초기화 결과 수령 시

  // 호출 예정
  // 배틀 패스 팝업 : 패스 보상 
  // 월정액 팝업 : 즉시 보상
  // 다이아 패스 팝업 : 즉시 보상
  // 첫 결제 보상 팝업 : 즉시 보상
  // 일일 특가 팝업 : 상품 구매

  // 로컬라이징 텍스트
  // RewardPopupTitleText : 보상
  // BackgroundText : 탭하여 닫기
  #endregion
  
  #region Enum For Binding UI Automatically
  enum GameObjects
  {
    RewardItemScrollContentObject, 
  }
  enum Buttons
  {
    BackgroundButton,
  }
  enum Texts
  {
    RewardPopupTitleText,
    BackgroundText
  }
  #endregion
  
  public Action OnClosed;
  
  private string[] _spriteName;
  private int[] _count;
  
  protected override bool Init()
  {
    if (base.Init() == false) return false;
   
    BindObject(typeof(GameObjects));
    BindButton(typeof(Buttons));
    BindText(typeof(Texts));
    GetButton((int)Buttons.BackgroundButton).gameObject.BindEvent(OnClickBackgroundButton);

    Refresh();
    Managers.Sound.Play(Define.ESound.Effect, "PopupOpen_Reward");

    return true;
  }
  
  public void SetInfo(string[] spriteName, int[] count, Action callback = null)
  {
    _spriteName = spriteName;
    _count = count;
    OnClosed = callback;
    Refresh();
  }
  
  private void Refresh()
  {
    if (_init == false) return;

    GetObject((int)GameObjects.RewardItemScrollContentObject).DestroyChildren();
    for (int i = 0; i < _spriteName.Length; i++)
    {
      Debug.Log(_spriteName[i]);
      UI_MaterialItem item = Managers.UI.MakeSubItem<UI_MaterialItem>(GetObject((int)GameObjects.RewardItemScrollContentObject).transform);
      item.SetInfo(_spriteName[i], _count[i]);
    }
  }
  
  private void OnClickBackgroundButton()
  {
    Managers.Sound.PlayPopupClose();
    gameObject.SetActive(false);
    OnClosed?.Invoke();
  }
}
