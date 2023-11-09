using Unity.VisualScripting;
using static Define;

public class UI_BackToBattlePopup : UI_Popup
{
  #region UI Feature List
  // 로컬라이징
  // BackToBattleTitleText : 이어서 하기
  // BackToBattleContentText : 진행중인 전투가 있습니다.\n계속하시겠습니까?
  // ConfirmText : OK
  // CancelText : 취소
  #endregion
  
  #region Enum For Binding Ui Automatically
  enum GameObjects
  {
    ContentObject,
  }
  enum Buttons
  {
    ConfirmButton,
    CancelButton,
  }
  enum Texts
  {
    BackToBattleTitleText,
    BackToBattleContentText,
    ConfirmText,
    CancelText,
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

    GetButton((int)Buttons.ConfirmButton).gameObject.BindEvent(OnClickConfirmButton);
    GetButton((int)Buttons.ConfirmButton).GetOrAddComponent<UI_ButtonAnimation>();
    GetButton((int)Buttons.CancelButton).gameObject.BindEvent(OnClickCancelButton);
    GetButton((int)Buttons.CancelButton).GetOrAddComponent<UI_ButtonAnimation>();
    
    return true;
  }
  
  private void OnClickConfirmButton()
  {
    Managers.Sound.PlayButtonClick();
    Managers.Scene.LoadScene(EScene.GameScene, transform);
  }
  private void OnClickCancelButton()
  {
    Managers.Sound.PlayButtonClick();
    Managers.Game.ClearContinueData();
    Managers.UI.ClosePopupUI(this);
  }
}
