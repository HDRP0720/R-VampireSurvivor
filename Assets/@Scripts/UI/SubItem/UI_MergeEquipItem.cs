using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Define;

public class UI_MergeEquipItem : UI_Base
{
  #region Enum For Binding UI Automatically
  enum GameObjects
  {
    EquipmentRedDotObject,
    NewTextObject,
    EquippedObject,
    SelectObject,
    LockObject,
    SpecialImage,
  }

  enum Texts
  {
    EquipmentLevelValueText,
    EnforceValueText,
  }

  enum Images
  {
    EquipmentGradeBackgroundImage,
    EquipmentImage,
    EquipmentEnforceBackgroundImage,
    EquipmentTypeBackgroundImage,
    EquipmentTypeImage,
  }
  #endregion
  
  private Equipment _equipment;
  private ScrollRect _scrollRect;
  private bool _isDrag = false;
  
  public Action OnClickEquipItem;
  
  private void Awake()
  {
    Init();
  }

  protected override bool Init()
  {
    if (base.Init() == false) return false;

    BindObject(typeof(GameObjects));
    BindText(typeof(Texts));
    BindImage(typeof(Images));

    gameObject.BindEvent(null, OnDrag, UIEvent.Drag);
    gameObject.BindEvent(null, OnBeginDrag, UIEvent.BeginDrag);
    gameObject.BindEvent(null, OnEndDrag, UIEvent.EndDrag);

    gameObject.BindEvent(OnClickEquipItemButton);
    return true;
  }

  public void SetInfo(Equipment item, UI_ItemParentType parentType, ScrollRect scrollRect, bool isSelected, bool isLock)
  {
    _equipment = item;
    transform.localScale = Vector3.one;
    _scrollRect = scrollRect;

    #region Change Colors
    // EquipmentGradeBackgroundImage : 합성 할 장비 등급의 테두리 (색상 변경)
    // EquipmentEnforceBackgroundImage : 유일 +1 등급부터 활성화되고 등급에 따라 이미지 색깔 변경
    switch (_equipment.equipmentData.equipmentGrade)
    {
      case EEquipmentGrade.Common:
        GetImage((int)Images.EquipmentGradeBackgroundImage).color = EquipmentUIColors.Common;
        GetImage((int)Images.EquipmentTypeBackgroundImage).color = EquipmentUIColors.Common;
        break;

      case EEquipmentGrade.Uncommon:
        GetImage((int)Images.EquipmentGradeBackgroundImage).color = EquipmentUIColors.Uncommon;
        GetImage((int)Images.EquipmentTypeBackgroundImage).color = EquipmentUIColors.Uncommon;
        break;

      case EEquipmentGrade.Rare:
        GetImage((int)Images.EquipmentGradeBackgroundImage).color = EquipmentUIColors.Rare;
        GetImage((int)Images.EquipmentTypeBackgroundImage).color = EquipmentUIColors.Rare;
        break;

      case EEquipmentGrade.Epic:
      case EEquipmentGrade.Epic1:
      case EEquipmentGrade.Epic2:
        GetImage((int)Images.EquipmentGradeBackgroundImage).color = EquipmentUIColors.Epic;
        GetImage((int)Images.EquipmentEnforceBackgroundImage).color = EquipmentUIColors.EpicBg;
        GetImage((int)Images.EquipmentTypeBackgroundImage).color = EquipmentUIColors.EpicBg;
        break;

      case EEquipmentGrade.Legendary:
      case EEquipmentGrade.Legendary1:
      case EEquipmentGrade.Legendary2:
      case EEquipmentGrade.Legendary3:
        GetImage((int)Images.EquipmentGradeBackgroundImage).color = EquipmentUIColors.Legendary;
        GetImage((int)Images.EquipmentEnforceBackgroundImage).color = EquipmentUIColors.LegendaryBg;
        GetImage((int)Images.EquipmentTypeBackgroundImage).color = EquipmentUIColors.LegendaryBg;
        break;

      default:
        break;
    }
    #endregion

    #region 유일 +1 등의 등급 벨류
    string gradeName = _equipment.equipmentData.equipmentGrade.ToString();
    int num = 0;

    // Epic1 -> 1 리턴 Epic2 ->2 리턴 Common처럼 숫자가 없으면 0 리턴
    Match match = Regex.Match(gradeName, @"\d+$");
    if (match.Success) num = int.Parse(match.Value);

    if (num == 0)
    {
        GetText((int)Texts.EnforceValueText).text = "";
        GetImage((int)Images.EquipmentEnforceBackgroundImage).gameObject.SetActive(false);
    }
    else
    {
        GetText((int)Texts.EnforceValueText).text = num.ToString();
        GetImage((int)Images.EquipmentEnforceBackgroundImage).gameObject.SetActive(true);
    }
    #endregion

    // EquipmentImage : 장비의 아이콘
    Sprite spr = Managers.Resource.Load<Sprite>(_equipment.equipmentData.spriteName);
    GetImage((int)Images.EquipmentImage).sprite = spr;
    // 장비 타입 아이콘
    Sprite tpy = Managers.Resource.Load<Sprite>($"{_equipment.equipmentData.equipmentType}_Icon.sprite");
    GetImage((int)Images.EquipmentTypeImage).sprite = tpy;
    // EquipmentLevelValueText : 장비의 현재 레벨
    GetText((int)Texts.EquipmentLevelValueText).text = $"Lv.{_equipment.Level}";
    // EquipmentRedDotObject : 장비가 강화가 가능할때 출력 
    GetObject((int)GameObjects.EquipmentRedDotObject).SetActive(_equipment.IsUpgradable);
    // NewTextObject : 장비를 처음 습득했을때 출력
    GetObject((int)GameObjects.NewTextObject).SetActive(!_equipment.IsConfirmed);
    // EquippedObject : 합성 팝업에서 착용장비 표시용
    GetObject((int)GameObjects.EquippedObject).SetActive(_equipment.IsEquipped);
    // SelectObject : 합성 팝업에서 장비 선택 표시용
    GetObject((int)GameObjects.SelectObject).SetActive(_equipment.IsSelected);
    // Special아이템일때 
    bool isSpecial = _equipment.equipmentData.gachaRarity == EGachaRarity.Special ? true : false;
    //케릭터 장착중인 슬롯에 있으면 "착용중" 오브젝트 끔
    GetObject((int)GameObjects.SpecialImage).SetActive(isSpecial);
    if (parentType == UI_ItemParentType.CharacterEquipmentGroup)
      GetObject((int)GameObjects.EquippedObject).SetActive(false);

    GetObject((int)GameObjects.SelectObject).gameObject.SetActive(isSelected);
    GetObject((int)GameObjects.LockObject).gameObject.SetActive(isLock);
  }
      
  private void OnClickEquipItemButton() // 장비 선택 시
  {
    Managers.Sound.PlayButtonClick();
    
    if (_isDrag) return;
    
    if (!_equipment.IsConfirmed)
      OnClickEquipItem?.Invoke();

    (Managers.UI.SceneUI as UI_LobbyScene)?.MergePopupUI.SetMergeItem(_equipment);
  }
  private void OnDrag(BaseEventData baseEventData)
  {
    _isDrag = true;
    PointerEventData pointerEventData = baseEventData as PointerEventData;
    _scrollRect.OnDrag(pointerEventData);
  }
  private void OnBeginDrag(BaseEventData baseEventData)
  {
    _isDrag = true;
    PointerEventData pointerEventData = baseEventData as PointerEventData;
    _scrollRect.OnBeginDrag(pointerEventData);
  }
  private void OnEndDrag(BaseEventData baseEventData)
  {
    _isDrag = false;
    PointerEventData pointerEventData = baseEventData as PointerEventData;
    _scrollRect.OnEndDrag(pointerEventData);
  }
}
