using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using Data;
using static Define;

public class UI_MaterialItem : UI_Base
{
  #region UI Feature List
  // 정보 갱신
  // MaterialItemImage : 재료 아이템 아이콘
  // ItemCountValueText : 재료 아이콘 개수
  #endregion
  
  #region Enum For Binding UI Automatically
  enum GameObjects
  {
    GetEffectObject,
  }
  enum Buttons
  {
    MaterialInfoButton,
  }
  enum Texts
  {
    ItemCountValueText
  }
  enum Images
  {
    MaterialItemImage,
    MaterialItemBackgroundImage,
  }
  #endregion
  
  private MaterialData _materialData;
  private Transform _makeSubItemParents;
  private ScrollRect _scrollRect;
  
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
        
    GetObject((int)GameObjects.GetEffectObject).SetActive(false);

    GetButton((int)Buttons.MaterialInfoButton).gameObject.BindEvent(null, OnDrag, Define.UIEvent.Drag);
    GetButton((int)Buttons.MaterialInfoButton).gameObject.BindEvent(null, OnBeginDrag, Define.UIEvent.BeginDrag);
    GetButton((int)Buttons.MaterialInfoButton).gameObject.BindEvent(null, OnEndDrag, Define.UIEvent.EndDrag);
    gameObject.BindEvent(OnClickMaterialInfoButton);
    GetButton((int)Buttons.MaterialInfoButton).gameObject.BindEvent(OnClickMaterialInfoButton);

    return true;
  }
  
  public void SetInfo(string spriteName, int count)
  {
    transform.localScale = Vector3.one;
    GetImage((int)Images.MaterialItemImage).sprite = Managers.Resource.Load<Sprite>(spriteName);
    GetImage((int)Images.MaterialItemBackgroundImage).color = EquipmentUIColors.Epic;
    GetText((int)Texts.ItemCountValueText).text = $"{count}";
    GetObject((int)GameObjects.GetEffectObject).SetActive(true);

  }
  public void SetInfo(MaterialData data, Transform makeSubItemParents, int count, ScrollRect scrollRect = null)
  {
    transform.localScale = Vector3.one;
    _scrollRect = scrollRect;
    _makeSubItemParents = makeSubItemParents;
    _materialData = data;

    GetImage((int)Images.MaterialItemImage).sprite = Managers.Resource.Load<Sprite>(_materialData.SpriteName);
    GetText((int)Texts.ItemCountValueText).text = $"{count}";

    switch (data.MaterialGrade)
    {
      case EMaterialGrade.Common:
        GetImage((int)Images.MaterialItemBackgroundImage).color = EquipmentUIColors.Common;
        break;
      case EMaterialGrade.Uncommon:
        GetImage((int)Images.MaterialItemBackgroundImage).color = EquipmentUIColors.Uncommon;
        break;
      case EMaterialGrade.Rare:
        GetImage((int)Images.MaterialItemBackgroundImage).color = EquipmentUIColors.Rare;
        break;
      case EMaterialGrade.Epic:
      case EMaterialGrade.Epic1:
      case EMaterialGrade.Epic2:
        GetImage((int)Images.MaterialItemBackgroundImage).color = EquipmentUIColors.Epic;
        break;
      case EMaterialGrade.Legendary:
      case EMaterialGrade.Legendary1:
      case EMaterialGrade.Legendary2:
      case EMaterialGrade.Legendary3:
        GetImage((int)Images.MaterialItemBackgroundImage).color = EquipmentUIColors.Legendary;
        break;
      default:
        break;
    }
  }
  
  private void OnClickMaterialInfoButton()
  {
    Managers.Sound.PlayButtonClick();
    UI_ToolTipItem item = Managers.UI.MakeSubItem<UI_ToolTipItem>(_makeSubItemParents);
    item.transform.localScale = Vector3.one;
    RectTransform targetPos = this.gameObject.GetComponent<RectTransform>();
    RectTransform parentsCanvas = _makeSubItemParents.gameObject.GetComponent<RectTransform>();
    item.SetInfo(_materialData, targetPos, parentsCanvas);
    item.transform.SetAsLastSibling();
  }
  
  #region Scroll Functions
  private void OnDrag(BaseEventData baseEventData)
  {
    if (_scrollRect == null) return;
    
    PointerEventData pointerEventData = baseEventData as PointerEventData;
    _scrollRect.OnDrag(pointerEventData);
  }
  private void OnBeginDrag(BaseEventData baseEventData)
  {
    if (_scrollRect == null) return;

    PointerEventData pointerEventData = baseEventData as PointerEventData;
    _scrollRect.OnBeginDrag(pointerEventData);
  }
  private void OnEndDrag(BaseEventData baseEventData)
  {
    if (_scrollRect == null) return;

    PointerEventData pointerEventData = baseEventData as PointerEventData;
    _scrollRect.OnEndDrag(pointerEventData);
  }
  #endregion
}
