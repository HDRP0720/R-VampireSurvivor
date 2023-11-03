using UnityEngine;
using UnityEngine.UI;
using Unity.VisualScripting;

using static Define;

public class UI_GameoverPopup : UI_Popup
{
  #region UI Feature List
  // 정보 갱신
  // GameoverStageValueText : 해당 스테이지 수
  // GameoverLastWaveValueText : 죽기전 마지막 웨이브 수
  // GameoverGoldValueText : 죽기전 까지 얻은 골드
  // GameoverKillValueText : 죽기전 까지 킬 수

  // 로컬라이징 텍스트
  // GameoverPopupTitleText : 게임 오버
  // LastWaveText : 마지막 웨이브
  // ConfirmButtonText : OK
  #endregion

  #region Enum For Binding UI Automatically
  enum GameObjects
  {
    ContentObject,
    GameoverKillObject,
  }
  enum Texts
  {
    GameoverPopupTitleText,
    GameoverStageValueText,
    LastWaveText,
    GameoverLastWaveValueText,
    GameoverKillValueText,
    ConfirmButtonText,
  }
  enum Buttons
  {
    StatisticsButton,
    ConfirmButton,
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
    if (base.Init() == false)
      return false;
    BindObject(typeof(GameObjects));
    BindText(typeof(Texts));
    BindButton(typeof(Buttons));

    GetButton((int)Buttons.StatisticsButton).gameObject.BindEvent(OnClickStatisticsButton);
    GetButton((int)Buttons.StatisticsButton).GetOrAddComponent<UI_ButtonAnimation>();
    GetButton((int)Buttons.ConfirmButton).gameObject.BindEvent(OnClickConfirmButton);
    GetButton((int)Buttons.ConfirmButton).GetOrAddComponent<UI_ButtonAnimation>();
    Managers.Sound.Play(ESound.Effect, "PopupOpen_Gameover");

    Refresh();
    return true;
  }
  
  public void SetInfo()
  {
    GetText((int)Texts.GameoverStageValueText).text = $"{Managers.Game.CurrentStageData.StageIndex} STAGE";
    GetText((int)Texts.GameoverLastWaveValueText).text = $"{Managers.Game.CurrentWaveIndex + 1}";
    GetText((int)Texts.GameoverKillValueText).text = $"{Managers.Game.Player.KillCount}";
    
    Refresh();
  }
  
  private void Refresh()
  {
    // 리프레시 버그 대응
    LayoutRebuilder.ForceRebuildLayoutImmediate(GetObject((int)GameObjects.GameoverKillObject).GetComponent<RectTransform>());
  }
  
  private void OnClickStatisticsButton()
  {
    Managers.Sound.PlayButtonClick();
    Managers.UI.ShowPopupUI<UI_TotalDamagePopup>().SetInfo();
  }
  private void OnClickConfirmButton()
  {
    Managers.Sound.PlayButtonClick();

    StageClearInfo info;
    if (Managers.Game.DicStageClearInfo.TryGetValue(Managers.Game.CurrentStageData.StageIndex, out info))
    {
      if (Managers.Game.CurrentWaveIndex > info.maxWaveIndex)
      {
        info.maxWaveIndex = Managers.Game.CurrentWaveIndex;
        Managers.Game.DicStageClearInfo[Managers.Game.CurrentStageData.StageIndex] = info;
      }
    }

    Managers.Game.ClearContinueData();
    Managers.Scene.LoadScene(EScene.LobbyScene, transform);
  }
}
