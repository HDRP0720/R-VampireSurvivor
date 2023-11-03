using Unity.VisualScripting;

public class UI_PausePopup : UI_Popup
{
  #region UI Feature List
  // 정보 갱신
  // BattleSkillSlotGroupObject에 현재 보유하고 있는 전투 스킬 표시
  // SupportSkillSlotGroupObject에 현재 보유하고 있는 서포트 스킬 표시

  // 로컬라이징
  // PauseTitleText : 일시정지
  #endregion
  
  #region Enum For Binding Ui Automatically
  enum GameObjects
  {
    ContentObject,
  }
  enum Buttons
  {
    ResumeButton,
    HomeButton,
    StatisticsButton,
    SoundButton,
    SettingButton,
  }
  enum Texts
  {
    PauseTitleText,
    ResumeButtonText
  }
  #endregion

  private SkillBase _skill;
  
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
 
    #region Object Bind
    BindButton(typeof(Buttons));
    BindText(typeof(Texts));
    BindObject(typeof(GameObjects));

    GetButton((int)Buttons.HomeButton).gameObject.BindEvent(OnClickHomeButton);
    GetButton((int)Buttons.HomeButton).GetOrAddComponent<UI_ButtonAnimation>();
    GetButton((int)Buttons.ResumeButton).gameObject.BindEvent(OnClickResumeButton);
    GetButton((int)Buttons.ResumeButton).GetOrAddComponent<UI_ButtonAnimation>(); 
    GetButton((int)Buttons.SettingButton).gameObject.BindEvent(OnClickSettingButton);
    GetButton((int)Buttons.SettingButton).GetOrAddComponent<UI_ButtonAnimation>(); 
    GetButton((int)Buttons.SoundButton).gameObject.BindEvent(OnClickSoundButton);
    GetButton((int)Buttons.SoundButton).GetOrAddComponent<UI_ButtonAnimation>(); 
    GetButton((int)Buttons.StatisticsButton).gameObject.BindEvent(OnClickStatisticsButton);
    GetButton((int)Buttons.StatisticsButton).GetOrAddComponent<UI_ButtonAnimation>();
    #endregion

    return true;
  }
  
  private void OnClickResumeButton() // 되돌아가기 버튼
  {
    Managers.UI.ClosePopupUI(this);
  }
  private void OnClickHomeButton() // 로비 버튼
  {
    Managers.Sound.PlayButtonClick();
    Managers.UI.ShowPopupUI<UI_BackToHomePopup>();
  }
  private void OnClickSettingButton() // 설정 버튼
  {
    Managers.Sound.PlayButtonClick();
    Managers.UI.ShowPopupUI<UI_SettingPopup>();
  }
  private void OnClickSoundButton() // 사운드 버튼
  {
    Managers.Sound.PlayButtonClick();
  }
  private void OnClickStatisticsButton() // 통계 버튼
  {
    Managers.Sound.PlayButtonClick();
    // 통계 팝업 호출(아직 안만듬)
    Managers.UI.ShowPopupUI<UI_TotalDamagePopup>().SetInfo();
  }
}
