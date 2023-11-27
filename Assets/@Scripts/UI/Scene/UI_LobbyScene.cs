using UnityEngine;
using UnityEngine.UI;
using TMPro;

using DG.Tweening;

public class UI_LobbyScene : UI_Scene
{
  #region UI Feature List
  // 로컬라이징 텍스트
  // ShopToggleText : 상점
  // EquipmentToggleText : 장비
  // BattleToggleText : 전투
  // ChallengeToggleText : 도전
  // EvolveToggleText : 룬
  #endregion
  
  #region Enum For Binding UI Automatically
  enum GameObjects
  {
    ShopToggleRedDotObject, // 알림 상황 시 사용 할 레드닷
    EquipmentToggleRedDotObject,
    BattleToggleRedDotObject,
    // ChallengeToggleRedDotObject,
    // EvolveToggleRedDotObject,

    MenuToggleGroup,
    CheckShopImageObject,
    CheckEquipmentImageObject,
    CheckBattleImageObject,
    // CheckChallengeImageObject,
    // CheckEvolveImageObject,
  }
  enum Buttons
  {
    //UserIconButton, // 추후 유저 정보 팝업 만들어서 호출
  }
  enum Texts
  {
    ShopToggleText,
    EquipmentToggleText,
    BattleToggleText,
    // ChallengeToggleText,
    // EvolveToggleText,
    // UserNameText,
    // UserLevelText,
    StaminaValueText,
    DiaValueText,
    GoldValueText
  }
  enum Toggles
  {
    ShopToggle,
    EquipmentToggle,
    BattleToggle,
    //ChallengeToggle,
    //EvolveToggle,
  }
  enum Images
  {
    BackgroundImage,
  }
  #endregion

  #region Variables
  private bool _isSelectedBattle = false;
  private bool _isSelectedEquipment = false;
  private bool _isSelectedShop = false;
  private UI_BattlePopup _battlePopupUI;
  private UI_EquipmentPopup _equipmentPopupUI;
  private UI_MergePopup _mergePopupUI;
  private UI_EquipmentInfoPopup _equipmentInfoPopupUI;
  private UI_EquipmentResetPopup _equipmentResetPopupUI;
  private UI_RewardPopup _rewardPopupUI;
  private UI_MergeResultPopup _mergeResultPopupUI;
  private UI_ShopPopup _shopPopupUI;
  // private UI_EvolvePopup _evolvePopupUI;
  // private UI_ChallengePopup _challengePopupUI;
  #endregion

  #region Properties
  public UI_EquipmentPopup EquipmentPopupUI => _equipmentPopupUI;
  public UI_MergePopup MergePopupUI => _mergePopupUI;
  public UI_EquipmentInfoPopup EquipmentInfoPopupUI => _equipmentInfoPopupUI;
  public UI_EquipmentResetPopup EquipmentResetPopupUI => _equipmentResetPopupUI;
  public UI_RewardPopup RewardPopupUI => _rewardPopupUI;
  public UI_MergeResultPopup MergeResultPopupUI => _mergeResultPopupUI;
  #endregion
  
  public void OnDestroy()
  {
    if(Managers.Game != null)
      Managers.Game.OnResourcesChanged -= Refresh;
  }

  protected override bool Init()
  {
    BindObject(typeof(GameObjects));
    BindButton(typeof(Buttons));
    BindText(typeof(Texts));
    BindToggle(typeof(Toggles));
    BindImage(typeof(Images)); 

    GetToggle((int)Toggles.ShopToggle).gameObject.BindEvent(OnClickShopToggle);
    GetToggle((int)Toggles.EquipmentToggle).gameObject.BindEvent(OnClickEquipmentToggle);
    GetToggle((int)Toggles.BattleToggle).gameObject.BindEvent(OnClickBattleToggle);

    _shopPopupUI = Managers.UI.ShowPopupUI<UI_ShopPopup>();
    _equipmentPopupUI = Managers.UI.ShowPopupUI<UI_EquipmentPopup>();
    _battlePopupUI = Managers.UI.ShowPopupUI<UI_BattlePopup>();
    _equipmentInfoPopupUI = Managers.UI.ShowPopupUI<UI_EquipmentInfoPopup>();
    _mergePopupUI = Managers.UI.ShowPopupUI<UI_MergePopup>();
    _equipmentResetPopupUI = Managers.UI.ShowPopupUI<UI_EquipmentResetPopup>();
    _rewardPopupUI = Managers.UI.ShowPopupUI<UI_RewardPopup>();
    _mergeResultPopupUI = Managers.UI.ShowPopupUI<UI_MergeResultPopup>();
    // _evolvePopupUI = Managers.UI.ShowPopupUI<UI_EvolvePopup>();
    // _challengePopupUI = Managers.UI.ShowPopupUI<UI_ChallengePopup>();
  
    TogglesInit();
    GetToggle((int)Toggles.BattleToggle).gameObject.GetComponent<Toggle>().isOn = true;
    OnClickBattleToggle();

    Managers.Game.OnResourcesChanged += Refresh;
    Refresh();

    return true;
  }
  
  private void Refresh()
  {
    GetText((int)Texts.StaminaValueText).text = $"{Managers.Game.Stamina}/{Define.MAX_STAMINA}";
    GetText((int)Texts.DiaValueText).text = Managers.Game.Dia.ToString();
    GetText((int)Texts.GoldValueText).text = Managers.Game.Gold.ToString();
    
    LayoutRebuilder.ForceRebuildLayoutImmediate(GetObject((int)GameObjects.MenuToggleGroup).GetComponent<RectTransform>());
  }

  // 토글 초기화
  private void TogglesInit()
  {
    #region 팝업 초기화
    _shopPopupUI.gameObject.SetActive(false);
    _equipmentPopupUI.gameObject.SetActive(false);
    _battlePopupUI.gameObject.SetActive(false);
    _equipmentInfoPopupUI.gameObject.SetActive(false);
    _mergePopupUI.gameObject.SetActive(false);
    _equipmentResetPopupUI.gameObject.SetActive(false);
    _rewardPopupUI.gameObject.SetActive(false);
    _mergeResultPopupUI.gameObject.SetActive(false);
    // _evolvePopupUI.gameObject.SetActive(false);
    // _challengePopupUI.gameObject.SetActive(false);
    #endregion

    #region 토글 버튼 초기화
    // 재 클릭 방지 트리거 초기화
    _isSelectedEquipment = false;
    _isSelectedShop = false;
    _isSelectedBattle = false;
    //GetToggle((int)Toggles.ChallengeToggle).enabled = false; // 도전 탭 비활성화 #Neo
    //GetToggle((int)Toggles.EvolveToggle).enabled = false; // 진화 탭 비활성화 #Neo

    // 버튼 레드닷 초기화
    GetObject((int)GameObjects.ShopToggleRedDotObject).SetActive(false);
    GetObject((int)GameObjects.EquipmentToggleRedDotObject).SetActive(false);
    GetObject((int)GameObjects.BattleToggleRedDotObject).SetActive(false);
    //GetObject((int)GameObjects.ChallengeToggleRedDotObject).SetActive(false);
    //GetObject((int)GameObjects.EvolveToggleRedDotObject).SetActive(false);

    // 선택 토글 아이콘 초기화
    GetObject((int)GameObjects.CheckShopImageObject).SetActive(false);
    GetObject((int)GameObjects.CheckEquipmentImageObject).SetActive(false);
    GetObject((int)GameObjects.CheckBattleImageObject).SetActive(false);
    //GetObject((int)GameObjects.CheckChallengeImageObject).SetActive(false);
    //GetObject((int)GameObjects.CheckEvolveImageObject).SetActive(false);

    GetObject((int)GameObjects.CheckShopImageObject).GetComponent<RectTransform>().sizeDelta = new Vector2(200, 155);
    GetObject((int)GameObjects.CheckEquipmentImageObject).GetComponent<RectTransform>().sizeDelta = new Vector2(200, 155);
    GetObject((int)GameObjects.CheckBattleImageObject).GetComponent<RectTransform>().sizeDelta = new Vector2(200, 155);
    //GetObject((int)GameObjects.CheckChallengeImageObject).GetComponent<RectTransform>().sizeDelta = new Vector2(200, 155);
    //GetObject((int)GameObjects.CheckEvolveImageObject).GetComponent<RectTransform>().sizeDelta = new Vector2(200, 155);


    // 메뉴 텍스트 초기화
    GetText((int)Texts.ShopToggleText).gameObject.SetActive(false);
    GetText((int)Texts.EquipmentToggleText).gameObject.SetActive(false);
    GetText((int)Texts.BattleToggleText).gameObject.SetActive(false);
    //GetText((int)Texts.ChallengeToggleText).gameObject.SetActive(false);
    //GetText((int)Texts.EvolveToggleText).gameObject.SetActive(false);

    // 토글 크기 초기화
    GetToggle((int)Toggles.ShopToggle).GetComponent<RectTransform>().sizeDelta = new Vector2(200, 150);
    GetToggle((int)Toggles.EquipmentToggle).GetComponent<RectTransform>().sizeDelta = new Vector2(200, 150);
    GetToggle((int)Toggles.BattleToggle).GetComponent<RectTransform>().sizeDelta = new Vector2(200, 150);
    //GetToggle((int)Toggles.ChallengeToggle).GetComponent<RectTransform>().sizeDelta = new Vector2(200, 150);
    //GetToggle((int)Toggles.EvolveToggle).GetComponent<RectTransform>().sizeDelta = new Vector2(200, 150);
    #endregion
  }

  private void ShowUI(GameObject contentPopup, Toggle toggle, TMP_Text text, GameObject obj2, float duration = 0.1f)
  {
    TogglesInit();
    
    contentPopup.SetActive(true);
    toggle.GetComponent<RectTransform>().sizeDelta = new Vector2(280, 150);
    text.gameObject.SetActive(true);
    obj2.SetActive(true);
    obj2.GetComponent<RectTransform>().DOSizeDelta(new Vector2(200, 180), duration).SetEase(Ease.InOutQuad);

    Refresh();
  }

  private void OnClickShopToggle()
  { 
    Managers.Sound.PlayButtonClick();
    GetImage((int)Images.BackgroundImage).color = Utils.HexToColor("525DAD"); // 배경 색상 변경
    if (_isSelectedShop) return;  // 활성화 후 토글 클릭 방지
        
    ShowUI(_shopPopupUI.gameObject, GetToggle((int)Toggles.ShopToggle), GetText((int)Texts.ShopToggleText), GetObject((int)GameObjects.CheckShopImageObject));
    _isSelectedShop = true;
  }
  private void OnClickEquipmentToggle()
  {
      Managers.Sound.PlayButtonClick();
      GetImage((int)Images.BackgroundImage).color = Utils.HexToColor("5C254B"); // 배경 색상 변경
      if (_isSelectedEquipment == true) // 활성화 후 토글 클릭 방지
          return;

      ShowUI(_equipmentPopupUI.gameObject, GetToggle((int)Toggles.EquipmentToggle), GetText((int)Texts.EquipmentToggleText), GetObject((int)GameObjects.CheckEquipmentImageObject));
      _isSelectedEquipment = true;

      _equipmentPopupUI.SetInfo();
  }
  private void OnClickBattleToggle()
  {
    Managers.Sound.PlayButtonClick();
    GetImage((int)Images.BackgroundImage).color = Utils.HexToColor("1F5FA0");
    
    if (_isSelectedBattle) return;
       
    ShowUI(_battlePopupUI.gameObject, GetToggle((int)Toggles.BattleToggle), GetText((int)Texts.BattleToggleText), GetObject((int)GameObjects.CheckBattleImageObject));
    _isSelectedBattle = true;
  }
}
