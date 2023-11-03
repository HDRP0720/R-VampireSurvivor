using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using Data;
using static Define;

public class UI_SupportSkillItem : UI_Base
{
  #region Enum For Binding UI Automatically
  enum Buttons
  {
    SupportSkillButton,
  }
  enum Images
  {
    SupportSkillImage,
    BackgroundImage,
  }
  #endregion
  
  private SupportSkillData _supportSkillData;
  private Transform _makeSubItemParents;
  private ScrollRect _scrollRect;
  
  private void Awake()
  {
    Init();
  }

  protected override bool Init()
  {
    if (base.Init() == false) return false;

    BindImage(typeof(Images));
    BindButton(typeof(Buttons));
    GetButton((int)Buttons.SupportSkillButton).gameObject.BindEvent(OnClickSupportSkillItem);
    GetButton((int)Buttons.SupportSkillButton).gameObject.BindEvent(null, OnDrag, Define.UIEvent.Drag);
    GetButton((int)Buttons.SupportSkillButton).gameObject.BindEvent(null, OnBeginDrag, Define.UIEvent.BeginDrag);
    GetButton((int)Buttons.SupportSkillButton).gameObject.BindEvent(null, OnEndDrag, Define.UIEvent.EndDrag);

    return true;
  }
  
  public void SeteInfo(Data.SupportSkillData skill, Transform makeSubItemParents, ScrollRect scrollRect)
  {
    transform.localScale = Vector3.one;
    Image img = GetImage((int)Images.SupportSkillImage);
    img.sprite = Managers.Resource.Load<Sprite>(skill.iconLabel);
    
    _supportSkillData = skill;
    _makeSubItemParents = makeSubItemParents;
    _scrollRect = scrollRect;
    
    // Change background color by grade
    switch (skill.supportSkillGrade)
    {
      case ESupportSkillGrade.Common:
        GetImage((int)Images.BackgroundImage).color = EquipmentUIColors.Common;
        break;
      case ESupportSkillGrade.Uncommon:
        GetImage((int)Images.BackgroundImage).color = EquipmentUIColors.Uncommon;
        break;
      case ESupportSkillGrade.Rare:
        GetImage((int)Images.BackgroundImage).color = EquipmentUIColors.Rare;
        break;
      case ESupportSkillGrade.Epic:
        GetImage((int)Images.BackgroundImage).color = EquipmentUIColors.Epic;
        break;
      case ESupportSkillGrade.Legend:
        GetImage((int)Images.BackgroundImage).color = EquipmentUIColors.Legendary;
        break;
      default:
        break;
    }
  }

  #region Functions For Binding UI
  private void OnClickSupportSkillItem()
  {
    Managers.Sound.PlayButtonClick();
    // UI_ToolTipItem 프리팹 생성
    UI_ToolTipItem item = Managers.UI.MakeSubItem<UI_ToolTipItem>(_makeSubItemParents);
    item.transform.localScale = Vector3.one;
    RectTransform TargetPos = this.gameObject.GetComponent<RectTransform>();
    RectTransform parentsCanvas = _makeSubItemParents.gameObject.GetComponent<RectTransform>();
    item.SetInfo(_supportSkillData, TargetPos, parentsCanvas);
    item.transform.SetAsLastSibling();
  }
  private void OnDrag(BaseEventData baseEventData)
  {
    PointerEventData pointerEventData = baseEventData as PointerEventData;
    _scrollRect.OnDrag(pointerEventData);
  }
  private void OnBeginDrag(BaseEventData baseEventData)
  {
    PointerEventData pointerEventData = baseEventData as PointerEventData;
    _scrollRect.OnBeginDrag(pointerEventData);
  }
  private void OnEndDrag(BaseEventData baseEventData)
  {
    PointerEventData pointerEventData = baseEventData as PointerEventData;
    _scrollRect.OnEndDrag(pointerEventData);
  }
  #endregion
  
}
