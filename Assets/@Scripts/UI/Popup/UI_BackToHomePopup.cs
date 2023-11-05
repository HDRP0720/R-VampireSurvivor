using Unity.VisualScripting;

using static Define;

public class UI_BackToHomePopup : UI_Popup
{
  #region UI Feature List
  // 로컬라이징
  // BackToBattleTitleText : 게임 포기
  // BackToBattleContentText : 지금 그만두면 보상을 모두 잃습니다.\n로비로 돌아가시겠습니까?
  // ConfirmText : 돌아가기
  // CancelText : 그만둔다
  #endregion

  #region Enum For Binding Ui Automatically
  enum GameObjects
  {
    ContentObject,
  }
  enum Buttons
  {
    ResumeButton,
    QuitButton,
  }
  enum Texts
  {
    BackToHomeTitleText,
    BackToHameContentText,
    ResumeText,
    QuitText,
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

    GetButton((int)Buttons.ResumeButton).gameObject.BindEvent(OnClickResumeButton);
    GetButton((int)Buttons.ResumeButton).GetOrAddComponent<UI_ButtonAnimation>();
    GetButton((int)Buttons.QuitButton).gameObject.BindEvent(OnClickQuitButton);
    GetButton((int)Buttons.QuitButton).GetOrAddComponent<UI_ButtonAnimation>();

    Refresh();
    
    return true;
  }
  
  private void Refresh() { }
  
  private void OnClickResumeButton()
  {
    Managers.UI.ClosePopupUI(this); 
  }
  private void OnClickQuitButton()
  {
    Managers.Sound.PlayButtonClick();

    Managers.Game.isGameEnd = true;
    Managers.Game.Player.StopAllCoroutines();

    StageClearInfo info;
    if (Managers.Game.DicStageClearInfo.TryGetValue(Managers.Game.CurrentStageData.stageIndex, out info))
    {
      if (Managers.Game.CurrentWaveIndex > info.maxWaveIndex)
      {
        info.maxWaveIndex = Managers.Game.CurrentWaveIndex;
        Managers.Game.DicStageClearInfo[Managers.Game.CurrentStageData.stageIndex] = info;
      }
    }

    Managers.Game.ClearContinueData();
    Managers.Scene.LoadScene(EScene.LobbyScene, transform);
  }
}
