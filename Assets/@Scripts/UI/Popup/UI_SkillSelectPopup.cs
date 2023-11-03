using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UI_SkillSelectPopup : UI_Popup
{
  #region UI Feature List
  // 정보 갱신
  // BeforeLevelValueText : 이전 레벨
  // AfterLevelValueText : 현재 레벨
  // ExpSliderObject : 게임씬의 경험치와 동기화 ( 추후 탕탕같은 연출 추가 될수 있음)

  // 로컬라이징
  // CharacterLevelupTitleText : LEVEL UP!
  // SkillSelectTitleText : 스킬 선택
  // CardRefreshText : 새로고침
  // ADRefreshText : 새로고침
  // SkillSelectCommentText : 배울 스킬을 선택하십시오.
  #endregion
  
  #region Enum For Binding UI Automatically
  enum GameObjects
  {
    ContentObject,
    SkillCardSelectListObject,
    ExpSliderObject,
    DisabledObject,
    CharacterLevelObject,
  }
  enum Buttons
  {
    CardRefreshButton,
    ADRefreshButton,
  }

  enum Texts
  {
    SkillSelectCommentText,
    SkillSelectTitleText,
    CardRefreshText,
    CardRefreshCountValueText,
    ADRefreshText,

    CharacterLevelupTitleText,

    CharacterLevelValueText,
    BeforeLevelValueText,
    AfterLevelValueText,
  }

  enum Images
  {
    BattleSkilI_Icon_0,
    BattleSkilI_Icon_1,
    BattleSkilI_Icon_2,
    BattleSkilI_Icon_3,
    BattleSkilI_Icon_4,
    BattleSkilI_Icon_5,
  }
  #endregion

  private GameManager _game;
  
  private void OnEnable()
  {
    Init();
    PopupOpenAnimation(GetObject((int)GameObjects.ContentObject));
  }
  
  protected override bool Init()
  {
    if (base.Init() == false)
      return false;
    #region Object Bind
    BindObject(typeof(GameObjects));
    BindButton(typeof(Buttons));
    BindText(typeof(Texts));
    BindImage(typeof(Images));

    GetButton((int)Buttons.CardRefreshButton).gameObject.BindEvent(OnClickCardRefreshButton);
    GetButton((int)Buttons.ADRefreshButton).gameObject.BindEvent(OnClickADRefreshButton);
    GetObject((int)GameObjects.DisabledObject).gameObject.SetActive(false);
    #endregion

    _game = Managers.Game;

    Refresh();

    SetRecommendSkills();
    List<SkillBase> activeSkills = Managers.Game.Player.Skills.SkillList.Where(skill => skill.IsLearnedSkill).ToList();

    for (int i = 0; i < activeSkills.Count; i++)
      SetCurrentSkill(i, activeSkills[i]);
    
    Managers.Sound.Play(Define.ESound.Effect, "PopupOpen_SkillSelect");
    return true;
  }
  
  private void Refresh()
  {
    //GetObject((int)GameObjects.ExpSliderObject).GetComponent<Slider>().value = _game.Player.ExpRatio;
    GetText((int)Texts.CharacterLevelValueText).text = $"{_game.Player.Level}";
    GetText((int)Texts.BeforeLevelValueText).text = $"Lv.{_game.Player.Level - 1 }";
    GetText((int)Texts.AfterLevelValueText).text = $"Lv.{_game.Player.Level}";

    if (Managers.Game.Player.SkillRefreshCount > 0)
      GetText((int)Texts.CardRefreshCountValueText).text = $"남은 횟수 : {Managers.Game.Player.SkillRefreshCount}";
    else
      GetText((int)Texts.CardRefreshCountValueText).text = $"<color=red>남은 횟수 : 0</color>";

    LayoutRebuilder.ForceRebuildLayoutImmediate(GetObject((int)GameObjects.CharacterLevelObject).GetComponent<RectTransform>());
  }
  
  private void SetRecommendSkills()
  {
    GameObject container = GetObject((int)GameObjects.SkillCardSelectListObject);
    //초기화
    container.DestroyChildren();
    List<SkillBase> List = Managers.Game.Player.Skills.RecommendSkills();

    foreach (SkillBase skill in List)
    {
      UI_SkillCardItem item = Managers.UI.MakeSubItem<UI_SkillCardItem>(container.transform);
      item.GetComponent<UI_SkillCardItem>().SetInfo(skill);
    }
  }
  
  private void SetCurrentSkill(int index, SkillBase skill)
  {
    GetImage(index).sprite = Managers.Resource.Load<Sprite>(skill.SkillData.iconLabel);
    GetImage(index).enabled = true;
  }

  #region MyRegion
  private void OnClickCardRefreshButton()
  {
    Managers.Sound.PlayButtonClick();
    if (Managers.Game.Player.SkillRefreshCount > 0)
    { 
      SetRecommendSkills();
      Managers.Game.Player.SkillRefreshCount--;   
    }
    Refresh();
  }
  private void OnClickADRefreshButton()
  {
    Managers.Sound.PlayButtonClick();
    if (Managers.Game.SkillRefreshCountAds > 0)
    {
      // TODO: set ad callback
      // Managers.Ads.ShowRewardedAd(() =>
      // {
      //   Managers.Game.SkillRefreshCountAds--;
      //   SetRecommendSkills();
      // });
    }
    else
    {
      Managers.UI.ShowToast("더이상 사용 할 수 없습니다.");
    }
  }
  #endregion
}
