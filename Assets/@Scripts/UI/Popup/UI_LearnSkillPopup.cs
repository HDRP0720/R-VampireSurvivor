using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_LearnSkillPopup : UI_Popup
{
  #region UI Feature List
  // 정보 갱신
  // LearnSkillListObject : 얻은 스킬이 (UI_SkillCardItem) 들어가는 부모개체

  // 로컬라이징
  // BackgroundText : 탭하여 닫기
  // LearnSkillCommentText : 스킬 입수!
  #endregion
  
  #region Enum For Binding UI Automatically
  enum GameObjects
  {
    ContentObject,
  }
  enum Buttons
  {
    BackgroundButton,
  }
  enum Texts
  {
    BackgroundText,
    LearnSkillCommentText,
    SkillDescriptionText,
    CardNameText
  }
  enum Images
  {
    SkillCardBackgroundImage,
    SkillImage,
    StarOn_0,
    StarOn_1,
    StarOn_2,
    StarOn_3,
    StarOn_4,
    StarOn_5
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

    BindObject(typeof(GameObjects));
    BindButton(typeof(Buttons));
    BindText(typeof(Texts));
    BindImage(typeof(Images));
    GetButton((int)Buttons.BackgroundButton).gameObject.BindEvent(OnClickBackgroundButton);

    return true;
  }
  
  public void SetInfo()
  {
    //배우고있는 스킬 중 하나 레벨업 시켜준다.
    int index = UnityEngine.Random.Range(0, Managers.Game.Player.Skills.ActivatedSkills.Count);
    _skill = Managers.Game.Player.Skills.RecommendDropSkill();
        
    if (_skill != null)
      Managers.Game.Player.Skills.LevelUpSkill(_skill.SkillType);
    else
    {
      // TODO: 배울 스킬이 없을땐 고기나 금화 주기?
      Managers.UI.ClosePopupUI(this);
    }
        
    GetImage((int)Images.SkillImage).sprite = Managers.Resource.Load<Sprite>(_skill.SkillData.iconLabel);
    GetText((int)Texts.CardNameText).text = _skill.SkillData.name;
    GetText((int)Texts.SkillDescriptionText).text = _skill.SkillData.description;
    GetImage((int)Images.StarOn_1).gameObject.SetActive(_skill.Level >= 2);
    GetImage((int)Images.StarOn_2).gameObject.SetActive(_skill.Level >= 3);
    GetImage((int)Images.StarOn_3).gameObject.SetActive(_skill.Level >= 4);
    GetImage((int)Images.StarOn_4).gameObject.SetActive(_skill.Level >= 5);
    GetImage((int)Images.StarOn_5).gameObject.SetActive(_skill.Level >= 6);
  }
  
  private void OnClickBackgroundButton()
  {
    Managers.UI.ClosePopupUI(this);
  }
}
