using UnityEngine;
using static Define;

public class UI_GameResultPopup : UI_Popup
{
  #region Enum For Searching Objects
  enum GameObjects
  {
    ContentObject,
    ResultRewardScrollContentObject,
    ResultGoldObject,
    ResultKillObject
  }
  enum Texts
  {
    GameResultPopupTitleText,
    ResultStageValueText,
    ResultSurvivalTimeText,
    ResultSurvivalTimeValueText,
    ResultGoldValueText,
    ResultKillValueText,
    ConfirmButtonText
  }
  enum Buttons
  {
    StatisticsButton,
    ConfirmButton
  }
  #endregion

  protected override bool Init()
  {
    if (base.Init() == false) return false;
    
    BindObject(typeof(GameObjects));
    BindText(typeof(Texts));
    BindButton(typeof(Buttons));
    
    GetButton((int)Buttons.StatisticsButton).gameObject.BindEvent(OnClickStatisticsButton);
    GetButton((int)Buttons.ConfirmButton).gameObject.BindEvent(OnClickConfirmButton);

    RefreshUI();
    return true;
  }

  public void SetInfo()
  {
    RefreshUI();
  }
  private void RefreshUI()
  {
    if (_init == false) return;
    
    GetText((int)Texts.GameResultPopupTitleText).text = "Game Result";
    GetText((int)Texts.ResultStageValueText).text = "4 STAGE";
    GetText((int)Texts.ResultSurvivalTimeText).text = "Survival Time";
    GetText((int)Texts.ResultSurvivalTimeValueText).text = "14:58";
    GetText((int)Texts.ResultGoldValueText).text = "299";
    GetText((int)Texts.ResultKillValueText).text = "199";
    GetText((int)Texts.ConfirmButtonText).text = "OK";
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
