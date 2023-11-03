using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UI_TotalDamagePopup : UI_Popup
{
  #region UI Feature List
  // 정보 갱신
  // TotalDamageContentObject : 가 들어갈 부모개체

  // 로컬라이징
  // BackgroundText : 탭하여 닫기
  // TotalDamagePopupTitleText : 총 데미지
  #endregion

  #region Enum For Binding UI Automatically
  enum GameObjects
  {
    ContentObject,
    TotalDamageContentObject,
  }
  enum Buttons
  {
    BackgroundButton
  }
  enum Texts
  {
    BackgroundText,
    TotalDamagePopupTitleText,
  }
  enum Images
  {
    
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
    BindImage(typeof(Images));

    GetButton((int)Buttons.BackgroundButton).gameObject.BindEvent(OnClickClosePopup);

    Refresh();
    
    return true;
  }
  
  public void SetInfo()
  {
    GetObject((int)GameObjects.TotalDamageContentObject).DestroyChildren();
    List<SkillBase> skillList = Managers.Game.Player.Skills.SkillList.ToList();
    foreach (SkillBase skill in skillList.FindAll(skill => skill.IsLearnedSkill))
    {
      UI_SkillDamageItem item = Managers.UI.MakeSubItem<UI_SkillDamageItem>(GetObject((int)GameObjects.TotalDamageContentObject).transform);
      item.SetInfo(skill);
      item.transform.localScale = Vector3.one;
    }
    
    Refresh();
  }
  
  private void Refresh() { }

  private void OnClickClosePopup()
  {
    Managers.UI.ClosePopupUI(this);
  }
}
