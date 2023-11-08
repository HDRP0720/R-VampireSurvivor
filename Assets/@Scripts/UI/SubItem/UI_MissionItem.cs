using System.Collections;
using System.Collections.Generic;
using Data;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_MissionItem : UI_Base
{
  #region UI Feature List
  // 정보 갱신
  // RewardItemIconImage : 보상 아이템의 아이콘
  // RewardItemValueText : 보상 아이템의 수량
  // ProgressSliderObject : 미션의 진행도 슬라이더로 표시

  // MissionNameValueText : 미션의 이름
  // MissionProgressValueText : 미션의 진행도 (현재 / 목표)

  // 로컬라이징
  // ProgressText  
  #endregion
  
  #region Enum For Binding Ui Automatically
  enum GameObjects
  {
    ProgressSliderObject,
  }
  enum Buttons
  {
    GetButton,
  }
  enum Texts
  {
    RewardItemValueText,
    ProgressText,
    CompleteText,
    MissionNameValueText,
    MissionProgressValueText,
  }
  enum Images
  {
    RewardItemIconImage,
  }
  enum MissionState
  {
    Progress,
    Complete,
    Rewarded,
  }
  #endregion
  
  private MissionData _missionData;
  
  private void Awake()
  {
    Init();
  }

  protected override bool Init()
  {
    if (base.Init() == false) return false;

    BindObject(typeof(GameObjects));
    BindButton(typeof(Buttons));
    BindText(typeof(Texts));
    BindImage(typeof(Images));

    GetButton((int)Buttons.GetButton).gameObject.BindEvent(OnClickGetButton);
    GetButton((int)Buttons.GetButton).GetOrAddComponent<UI_ButtonAnimation>();

    AchievementContentInit();

    Refresh();
    return true;
  }
  
  public void SetInfo(MissionData missionData)
  {
    transform.localScale = Vector3.one;
    _missionData = missionData;

    Refresh();
  }
  
  // 미션 클리어 상태에 따라 활성화
  // - ProgressText : 진행중
  // - GetButton : 클리어 시
  // - CompleteText : 보상 지급 완료
  private void Refresh()
  {
    if (_missionData == null) return;
    
    GetText((int)Texts.RewardItemValueText).text = $"{_missionData.rewardValue}";
    GetText((int)Texts.MissionNameValueText).text = $"{_missionData.descriptionTextID}";
    GetObject((int)GameObjects.ProgressSliderObject).GetComponent<Slider>().value = 0;
    
    if (Managers.Game.DicMission.TryGetValue(_missionData.missionTarget, out MissionInfo missionInfo))
    { 
      if (missionInfo.progress > 0)
        GetObject((int)GameObjects.ProgressSliderObject).GetComponent<Slider>().value = (float)missionInfo.progress / _missionData.missionTargetValue;

      if (missionInfo.progress >= _missionData.missionTargetValue)
      {
        SetButtonUI(MissionState.Complete);
        if (missionInfo.isRewarded == true)
          SetButtonUI(MissionState.Rewarded);
      }
      else
      {
        SetButtonUI(MissionState.Progress);
      }
      GetText((int)Texts.MissionProgressValueText).text = $"{missionInfo.progress}/{_missionData.missionTargetValue}";
    }
    string sprName = Managers.Data.MaterialDic[_missionData.clearRewardItmeId].spriteName;
    GetImage((int)Images.RewardItemIconImage).sprite = Managers.Resource.Load<Sprite>(sprName);
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
        GetText((int)Texts.ProgressText).text = $"진행중";

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
    count[0] = _missionData.rewardValue;

    UI_RewardPopup rewardPopup = (Managers.UI.SceneUI as UI_LobbyScene).RewardPopupUI;
    rewardPopup.gameObject.SetActive(true);
    Managers.Game.Dia += _missionData.rewardValue;
    if (Managers.Game.DicMission.TryGetValue(_missionData.missionTarget, out MissionInfo info))
    { 
      info.isRewarded = true;
    }
    Refresh();

    rewardPopup.SetInfo(spriteName, count);
  }
}
