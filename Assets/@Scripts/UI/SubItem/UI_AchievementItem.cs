using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Data;
using Unity.VisualScripting;
using UnityEngine.UI;

public class UI_AchievementItem : UI_Base
{
  #region UI Feature List
  // 정보 갱신
  // RewardItemIconImage : 보상 아이템의 아이콘
  // RewardItemValueText : 보상 아이템의 수량
  // ProgressSliderObject : 미션의 진행도 슬라이더로 표시

  // AchievementNameValueText : 미션의 이름
  // AchievementValueText : 미션의 진행도 (현재 / 목표)
  #endregion
  
  #region Enum For Binding UI Automatically
  enum GameObjects
  {
    ProgressSlider,
  }
  enum Buttons
  {
    GetButton,
    //GoNowButton,
  }
  enum Texts
  {
    RewardItemValueText,
    CompleteText,
    AchievementNameValueText,
    AchievementValueText,
    ProgressText
  }
  enum Images
  {
    RewardItemIcon,
  }
  enum MissionState
  {
    Progress,
    Complete,
    Rewarded,
  }
  #endregion
  
  private AchievementData _achievementData;

  protected override bool Init()
  {
    if (base.Init() == false) return false;
  
    #region Object Bind
    BindObject(typeof(GameObjects));
    BindButton(typeof(Buttons));
    BindText(typeof(Texts));
    BindImage(typeof(Images));

    GetButton((int)Buttons.GetButton).gameObject.BindEvent(OnClickGetButton);
    GetButton((int)Buttons.GetButton).GetOrAddComponent<UI_ButtonAnimation>();
    //GetButton((int)Buttons.GoNowButton).gameObject.BindEvent(OnClickGoNowButton);
    //GetButton((int)Buttons.GoNowButton).GetOrAddComponent<UI_ButtonAnimation>();
    AchievementContentInit();
    #endregion

    Refresh();
    return true;    
  }
  
  public void SetInfo(AchievementData achievementData)
  {
    transform.localScale = Vector3.one;
    _achievementData = achievementData;
    Refresh();
  }
  
  // 미션 클리어 상태에 따라 활성화
  // - GoNowButton : 진행중
  // - GetButton : 클리어 시
  // - CompleteText : 보상 지급 완료
  private void Refresh()
  {
    if (_init == false) return;

    GetText((int)Texts.RewardItemValueText).text = $"{_achievementData.rewardValue}";
    GetText((int)Texts.AchievementNameValueText).text = $"{_achievementData.descriptionTextID}";
    GetObject((int)GameObjects.ProgressSlider).GetComponent<Slider>().value = 0;
     
    int progress = Managers.Achievement.GetProgressValue(_achievementData.missionTarget);
    if (progress > 0)
      GetObject((int)GameObjects.ProgressSlider).GetComponent<Slider>().value = (float)progress / _achievementData.missionTargetValue;

    if (progress >= _achievementData.missionTargetValue)
    {
      SetButtonUI(MissionState.Complete);
      if (_achievementData.isRewarded)
        SetButtonUI(MissionState.Rewarded);
    }
    else
    {
      SetButtonUI(MissionState.Progress);
    }
    GetText((int)Texts.AchievementValueText).text = $"{progress}/{_achievementData.missionTargetValue}";

    string sprName = Managers.Data.MaterialDic[_achievementData.clearRewardItemId].spriteName;
    GetImage((int)Images.RewardItemIcon).sprite = Managers.Resource.Load<Sprite>(sprName);
  }
  private void SetButtonUI(MissionState state)
  {
    GameObject objComplte = GetButton((int)Buttons.GetButton).gameObject;
    GameObject objProgress = GetText((int)Texts.ProgressText).gameObject;
    GameObject objRewarded = GetText((int)Texts.CompleteText).gameObject;

    switch (state)
    {
      case MissionState.Rewarded:
        objRewarded.SetActive(true);
        objComplte.SetActive(false);
        objProgress.SetActive(false);
        break;
      case MissionState.Complete:
        objRewarded.SetActive(false);
        objComplte.SetActive(true);
        objProgress.SetActive(false);
        break;
      case MissionState.Progress:
        objRewarded.SetActive(false);
        objComplte.SetActive(false);
        objProgress.SetActive(true);
        break;
    }
  }
  
  private void AchievementContentInit()
  {
    GetButton((int)Buttons.GetButton).gameObject.SetActive(true); // 임시로 활성화
    GetText((int)Texts.ProgressText).gameObject.SetActive(false);
    GetText((int)Texts.CompleteText).gameObject.SetActive(false);
  }
  
  private void OnClickGetButton()
  {
    Managers.Sound.PlayButtonClick();

    string[] spriteName = new string[1];
    int[] count = new int[1];

    spriteName[0] = Managers.Data.MaterialDic[Define.ID_DIA].spriteName;
    count[0] = _achievementData.rewardValue;

    UI_RewardPopup rewardPopup = (Managers.UI.SceneUI as UI_LobbyScene).RewardPopupUI;
    rewardPopup.gameObject.SetActive(true);
    Managers.Game.Dia += _achievementData.rewardValue;
    Managers.Achievement.RewardedAchievement(_achievementData.achievementID);
    _achievementData = Managers.Achievement.GetNextAchievment(_achievementData.achievementID);
    if(_achievementData != null)
      Refresh();
    rewardPopup.SetInfo(spriteName, count);
  }
}
