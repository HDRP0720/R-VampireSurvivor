using UnityEngine;
using static Define;

public class UI_SettingPopup : UI_Popup
{
  #region UI Feature List
  // 정보 갱신
  // UseIDValueText : 유저 아이디
  // VersionValueText : 현재 클라이언트의 버전 표시

  // 버튼
  // SoundEffectOffButton, SoundEffectOnButton : 효과음 온오프 기능 (상태 저장)
  // BackgroundSoundOffButton, BackgroundSoundOnButton : BGM 온오프 기능 (상태 저장)
  // JoystickFixedOffButton, JoystickFixedOnButton : 조이스틱 고정 온오프 기능 (상태 저장)

  // 로컬라이징 텍스트
  // SettingTlileText : 설정
  // UserInfoText : 유저 정보
  // SoundEffectText : 효과음
  // BackgroundSoundText : 배경음
  // JoystickText : 조이스틱
  // LanguageText : 언어
  // TermsOfServiceButtonText : 서비스 이용약관
  // PrivacyPolicyButtonText : 개인정보 처리방침
  // FeedbackButtonText : Feedback
  // ErrorFixButtonText : Error Fix
  #endregion

  #region Enum For Binding Ui Automatically
  enum GameObjects
  {
    ContentObject,
    // TODO: LanguageObject
  }
  enum Buttons
  {
    BackgroundButton,
    SoundEffectOffButton,
    SoundEffectOnButton,
    BackgroundSoundOffButton,
    BackgroundSoundOnButton,
    JoystickFixedOffButton,
    JoystickFixedOnButton,
    // TODO: LanguageButton,
    // TODO: TermsOfServiceButton,
    // TODO: PrivacyPolicyButton,
  }

  enum Texts
  {
    SettingTitleText,
    // TODO: UserInfoText,
    // TODO: UseIDValueText,
    SoundEffectText,
    BackgroundSoundText,
    JoystickText,
    // TODO: LanguageText,
    // TODO: LanguageValueText,
    // TODO: TermsOfServiceButtonText,
    // TODO: PrivacyPolicyButtonText,
    VersionValueText
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
    
    #region Object Bind
    BindObject(typeof(GameObjects));
    BindButton(typeof(Buttons));
    BindText(typeof(Texts));

    GetButton((int)Buttons.BackgroundButton).gameObject.BindEvent(OnClickBackgroundButton);

    GetButton((int)Buttons.SoundEffectOffButton).gameObject.BindEvent(EffectSoundOn);
    GetButton((int)Buttons.SoundEffectOnButton).gameObject.BindEvent(EffectSoundOff);

    GetButton((int)Buttons.BackgroundSoundOffButton).gameObject.BindEvent(BackgroundSoundOn);
    GetButton((int)Buttons.BackgroundSoundOnButton).gameObject.BindEvent(BackgroundSoundOff);

    GetButton((int)Buttons.JoystickFixedOffButton).gameObject.BindEvent(OnClickJoystickFixed);
    GetButton((int)Buttons.JoystickFixedOnButton).gameObject.BindEvent(OnClickJoystickNonFixed);

    GetText((int)Texts.VersionValueText).text = $"버전 : {Application.version}";
    #endregion
    
    if (Managers.Game.BGMOn == false)
      BackgroundSoundOff();
    else
      BackgroundSoundOn();
    
    if (Managers.Game.EffectSoundOn == false)
      EffectSoundOff();
    else
      EffectSoundOn();

    if (Managers.Game.JoystickType == EJoystickType.Fixed)
      OnClickJoystickFixed();
    else
      OnClickJoystickNonFixed();

    Refresh();
    
    return true;
  }
  
  private void Refresh() { }
  
  private void OnClickBackgroundButton()
  {
    Managers.UI.ClosePopupUI(this);
  }
  private void OnClickJoystickFixed()
  {
    Managers.Sound.PlayButtonClick();
    Managers.Game.JoystickType = EJoystickType.Fixed;
    GetButton((int)Buttons.JoystickFixedOnButton).gameObject.SetActive(true);
    GetButton((int)Buttons.JoystickFixedOffButton).gameObject.SetActive(false);
  }
  private void OnClickJoystickNonFixed()
  {
    Managers.Sound.PlayButtonClick();
    Managers.Game.JoystickType = EJoystickType.Flexible;
    GetButton((int)Buttons.JoystickFixedOnButton).gameObject.SetActive(false);
    GetButton((int)Buttons.JoystickFixedOffButton).gameObject.SetActive(true);
  }
  
  private void BackgroundSoundOn()
  {
    Managers.Sound.PlayButtonClick();
    Managers.Game.BGMOn = true;
    GetButton((int)Buttons.BackgroundSoundOnButton).gameObject.SetActive(true);
    GetButton((int)Buttons.BackgroundSoundOffButton).gameObject.SetActive(false);
  }
  private void BackgroundSoundOff()
  {
    Managers.Sound.PlayButtonClick();
    Managers.Game.BGMOn = false;
    GetButton((int)Buttons.BackgroundSoundOnButton).gameObject.SetActive(false);
    GetButton((int)Buttons.BackgroundSoundOffButton).gameObject.SetActive(true);
  }
  private void EffectSoundOn()
  {
    Managers.Sound.PlayButtonClick();
    Managers.Game.EffectSoundOn = true;
    GetButton((int)Buttons.SoundEffectOnButton).gameObject.SetActive(true);
    GetButton((int)Buttons.SoundEffectOffButton).gameObject.SetActive(false);
  }
  private void EffectSoundOff()
  {
    Managers.Sound.PlayButtonClick();
    Managers.Game.EffectSoundOn = false;
    GetButton((int)Buttons.SoundEffectOnButton).gameObject.SetActive(false);
    GetButton((int)Buttons.SoundEffectOffButton).gameObject.SetActive(true);
  }
}
