using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using UnityEngine;
using UnityEngine.UI;
using Unity.VisualScripting;

using static Define;

public class UI_MergePopup : UI_Popup
{
  #region UI Feature List
  // 정보 갱신
  // EquipInventoryScrollContentObject : 보유한 장비들이 들어갈 부모개체

  // 인벤토리에 있는 장비를 선택 시 (or 선택한 상태로 진입 시)
  // - TargetEquip : 합성할 장비 선택 시 활성화
  // - OptionResultObject : 합성할 장비 선택 시 활성화
  // - SelectMergeCommentText : 합성할 장비 선택 시 비 활성화 (머지 안내문)

  // MergeResultGroup
  // EquipResultButton (장비 결과 표시)
  //  MergePossibleOutlineImage : 합성 가능하면 색상 변경
  //    - 기본 : #FFFFFF (흰색)
  //    - 합성 가능 : #0AFF00 (초록)
  //  TargetEquipObject (합성할 장비)
  //    TargetEquipGradeBackgroundImage : 합성 할 장비 등급의 테두리 (색상 변경)
  //    - 일반(Common) : #AC9B83
  //    - 고급(Uncommon)  : #73EC4E
  //    - 희귀(Rare) : #0F84FF
  //    - 유일(Epic) : #B740EA
  //    - 전설(Legendary) : #F19B02
  //    - 신화(Myth) : #FC2302
  //    TargetEquipImage : 선택한 장비의 아이콘
  //    TargetEquipLevelValueText : 선택한 장비의 레벨
  //    TargetEquipEnforceBackgroundImage : 유일 +1 등급부터 활성화되고 등급에 따라 컬러 변경
  //    - 유일(Epic) : #B740EA
  //    - 전설(Legendary) : #F19B02
  //    - 신화(Myth) : #FC2302
  //    TargetEquipEnforceValueText : 등급의 벨류

  // EquipResultGradeBackgroundImage : 합성 후 장비 등급의 테두리 (리소스 변경)
  // EquipResultImage : 선택한 장비의 아이콘
  // EquipResultLevelValueText : 합성 후 장비 레벨 
  // EquipResultEnforceBackgroundImage : 유일 +1 등급부터 활성화되고 등급에 따라 컬러 변경
  // EquipResultEnforceValueText : 등급의 벨류
    
  // OptionResult (옵션 결과 리스트)
  //  EquipmentNameText : 장비 이름
  //  BeforeLevelValueText : 합성 전 레벨
  //  LevelArrowImage : 기본 비활성화, 합성이 가능할 때 활성화
  //  AfterLevelValueText : 합성 후 레벨 (기본 비활성화, 합성아 가능할 때 활성화)
  //  ImproveATKObject : 공격력 옵션이 있다면 활성화
  //  BeforeATKValueText : 합성 전 공격력
  //  ATKArrowImage : 기본 비활성화, 합성이 가능할 때 활성화
  //  AfterATKValueText : 합성 후 공격력 ((기본 비활성화, 합성아 가능할 때 활성화))
  //  ImproveHPObject : 체력 옵션이 있다면 활성화
  //  BeforeHPValueText : 합성 전 체력
  //  HPArrowImage : 기본 비활성화, 합성이 가능할 때 활성화
  //  AfterHPValueText : 합성 후 체력 ((기본 비활성화, 합성아 가능할 때 활성화))
  //  ImproveOptionValueText : 합성 후 추가 옵션


  // MergeCostGroup (합성 재료)
  //  FirstCostEquipNeedObject : 합성할 장비 선택 시 활성화
  //    FirstCostEquipGradeBackgroundImage : 필요한 재료 등급의 테두리 (리소스 변경)
  //    FirstCostEquipImage : 필요한 재료의 종류 아이콘 (리소스 변경)
  //    - 무기 : @Resources\Sprites\UI\Common\Icon\Ui_Sword_Icon
  //    - 장갑 : @Resources\Sprites\UI\Common\Icon\Ui_Glove_Icon
  //    - 반지 : @Resources\Sprites\UI\Common\Icon\Ui_Ring_Icon
  //    - 헬멧 : @Resources\Sprites\UI\Common\Icon\Ui_Helmet_Icon
  //    - 갑옷 : @Resources\Sprites\UI\Common\Icon\Ui_Top_Icon
  //    - 부츠 : @Resources\Sprites\UI\Common\Icon\Ui_Boots_Icon
  //    FirstCostEquipBackgroundImage : 유일 +1 등급부터 활성화되고 등급에 따라 컬러 변경
  //    FirstCostEquipEnforceValueText : 등급의 벨류

  // FirstCostEquipSelectObject : 첫번째 재료 선택 시 활성화 
  //  FirstSelectEquipGradeBackgroundImage : 선택한 장비 등급의 테두리 (리소스 변경)
  //  FirstSelectEquipImage : 선택한 장비의 아이콘
  //  FirstSelectEquipLevelValueText :선택한 장비의 레벨
  //  FirstSelectEquipEnforceBackgroundImage : 유일 +1 등급부터 활성화되고 등급에 따라 컬러 변경
  //  FirstSelectEquipEnforceValueText : 등급의 벨류

  // SecondCostEquipNeedObject : 합성할 장비 선택 시 활성화
  //  SecondCostEquipGradeBackgroundImage : 필요한 재료 등급의 테두리 (리소스 변경)
  //  SecondCostEquipImage : 필요한 재료의 종류 아이콘 (리소스 변경)

  // SecondCostEquipSelectObject : 첫번째 재료 선택 시 활성화 
  //  SecondSelectEquipGradeBackgroundImage : 선택한 장비 등급의 테두리 (리소스 변경)
  //  SecondSelectEquipImage : 선택한 장비의 아이콘
  //  SecondSelectEquipLevelValueText :선택한 장비의 레벨
  //  SecondSelectEquipEnforceBackgroundImage : 유일 +1 등급부터 활성화되고 등급에 따라 컬러 변경
  //  SecondSelectEquipEnforceValueText : 등급의 벨류

  // MergeButton : 합성 버튼, 합성이 가능하다면 활성화

  // 로컬라이징 텍스트
  // ImproveLevelText : 최대 레벨
  // ImproveATKText : 공격력
  // ImproveHPText : 체력
  // EquipmentTitleText : 장비
  // SortButtonText : 정렬
  // MergeAllButtonText : 모두합성
  #endregion

  #region Enum For Binding UI Automatically
  enum GameObjects
  {
    ContentObject,
    SelectedEquipObject,
    OptionResultObject,
    ImprovATKObject,
    ImprovHPObject,
    FirstCostEquipNeedObject,
    FirstCostEquipSelectObject,
    SecondCostEquipNeedObject,
    SecondCostEquipSelectObject,
    MergeAllButtonRedDotObject,
    EquipInventoryScrollContentObject,
    MergeStartEffect,
    MergeFinishEffect,
  }
  enum Buttons
  {
    EquipResultButton,
    FirstCostButton,
    SecondCostButton,
    SortButton,
    MergeAllButton,
    MergeButton,
    BackButton,
  }
  enum Texts
  {
    SelectedEquipLevelValueText,
    SelectedEquipEnforceValueText,
    EquipmentNameText,
    BeforeGradeValueText,
    AfterGradeValueText,
    BeforeLevelValueText,
    AfterLevelValueText, 
    ImprovLevelText, 
    BeforeATKValueText,
    AfterATKValueText,
    ImprovHPText,
    BeforeHPValueText,
    AfterHPValueText,
    ImprovOptionValueText,
    FirstCostEquipEnforceValueText,
    FirstSelectEquipLevelValueText,
    FirstSelectEquipEnforceValueText,
    SecondSelectEquipLevelValueText,
    SecondSelectEquipEnforceValueText,
    EquipmentTitleText,
    SortButtonText,
    MergeAllButtonText,
    SelectEquipmentCommentText,
    SelectMergeCommentText,
  }
  enum Images
  {
    MergePossibleOutlineImage,
    SelectedEquipGradeBackgroundImage,
    SelectedEquipImage,
    SelectedEquipEnforceBackgroundImage,
    SelectedEquipTypeBackgroundImage,
    SelectedEquipTypeImage,
    GradeArrowImage,
    LevelArrowImage,
    ATKArrowImage,
    HPArrowImage,
    FirstCostEquipGradeBackgroundImage,
    FirstCostEquipImage,
    FirstCostEquipBackgroundImage,
    FirstSelectEquipGradeBackgroundImage,
    FirstSelectEquipImage,
    FirstSelectEquipEnforceBackgroundImage,
    FirstSelectEquipTypeBackgroundImage,
    FirstSelectEquipTypeImage,
    SecondCostEquipGradeBackgroundImage,
    SecondCostEquipImage,
    SecondSelectEquipGradeBackgroundImage,
    SecondSelectEquipImage,
    SecondSelectEquipEnforceBackgroundImage,
    SecondSelectEquipTypeBackgroundImage,
    SecondSelectEquipTypeImage
  }
  #endregion
  
  [SerializeField] private ScrollRect _scrollRect;

  private Equipment _equipment;
  private Equipment _mergeEquipment1;
  private Equipment _mergeEquipment2;
  private EEquipmentSortType _equipmentSortType;
  
  private string _sortText_Level = "정렬 : 레벨";
  private string _sortText_Grade = "정렬 : 등급";
  
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
    
    #region Object Bind
    BindObject(typeof(GameObjects));
    BindButton(typeof(Buttons));
    BindText(typeof(Texts));
    BindImage(typeof(Images));

    GetButton((int)Buttons.EquipResultButton).gameObject.BindEvent(OnClickEquipResultButton);
    GetObject((int)GameObjects.SelectedEquipObject).gameObject.SetActive(false); // 합성할 장비
    GetText((int)Texts.SelectEquipmentCommentText).gameObject.SetActive(false); 
    GetText((int)Texts.SelectMergeCommentText).gameObject.SetActive(false); 
    GetObject((int)GameObjects.OptionResultObject).gameObject.SetActive(false); // 옵션 결과

    GetObject((int)GameObjects.MergeStartEffect).gameObject.SetActive(false); // 합성 시작 이펙트
    GetObject((int)GameObjects.MergeFinishEffect).gameObject.SetActive(false); // 합성 끝 이펙트

    GetButton((int)Buttons.FirstCostButton).gameObject.BindEvent(OnClickFirstCostButton);
    GetObject((int)GameObjects.FirstCostEquipNeedObject).gameObject.SetActive(false); // 첫번째 재료
    GetObject((int)GameObjects.FirstCostEquipSelectObject).gameObject.SetActive(false);

    GetButton((int)Buttons.SecondCostButton).gameObject.BindEvent(OnClickSecondCostButton);
    GetObject((int)GameObjects.SecondCostEquipNeedObject).gameObject.SetActive(false); // 두번째 재료
    GetObject((int)GameObjects.SecondCostEquipSelectObject).gameObject.SetActive(false);

    GetButton((int)Buttons.SortButton).gameObject.BindEvent(OnClickSortButton);
    GetButton((int)Buttons.SortButton).GetOrAddComponent<UI_ButtonAnimation>();
    GetButton((int)Buttons.MergeAllButton).gameObject.BindEvent(OnClickMergeAllButton);
    GetButton((int)Buttons.MergeAllButton).GetOrAddComponent<UI_ButtonAnimation>();

    // 정렬 기준 디폴트
    _equipmentSortType = EEquipmentSortType.Level;
    GetText((int)Texts.SortButtonText).text = _sortText_Level;

    GetButton((int)Buttons.MergeButton).gameObject.BindEvent(OnClickMergeButton);
    GetButton((int)Buttons.MergeButton).GetOrAddComponent<UI_ButtonAnimation>();
    GetButton((int)Buttons.MergeButton).gameObject.SetActive(false); // 합성 버튼
    GetObject((int)GameObjects.MergeAllButtonRedDotObject).gameObject.SetActive(false); // 모두 합성 레드닷


    GetButton((int)Buttons.BackButton).gameObject.BindEvent(OnClickBackButton); // 뒤로가기
    GetButton((int)Buttons.BackButton).GetOrAddComponent<UI_ButtonAnimation>();
    #endregion
    
    Refresh();
    return true;
  }
  
  public void SetInfo(Equipment equipment)
  {
    _equipment = equipment;
    _mergeEquipment1 = null;
    _mergeEquipment2 = null;
    Refresh();
  }
  
  public void SetMergeItem(Equipment equipment, bool showUI = true)
  {
    if (equipment.IsEquipped) return;

    if (equipment.Level > 1) return;

    if (_equipment == null)
    {
      _equipment = equipment;
      if (showUI)
      {
        Refresh_SelectedEquipObject();
        SortEquipments();
      }
      return;
    }

    if (_equipment == equipment) return;
    
    if (_equipment.equipmentData.equipmentType != equipment.equipmentData.equipmentType) return;

    if (equipment.Equals(_mergeEquipment1)) return;

    if (equipment.Equals(_mergeEquipment2)) return;

    if (_mergeEquipment1 == null)
    {
      if (_equipment.equipmentData.mergeEquipmentType1 == EMergeEquipmentType.ItemCode)
      {
        if (equipment.equipmentData.dataId != _equipment.equipmentData.mergeEquipment1)
          return;
      }
      else if (_equipment.equipmentData.mergeEquipmentType1 == EMergeEquipmentType.Grade)
      {
        if (equipment.equipmentData.equipmentGrade != (EEquipmentGrade)Enum.Parse(typeof(EEquipmentGrade), _equipment.equipmentData.mergeEquipment1))
          return;
      }
      else
      {
        return;
      }
      _mergeEquipment1 = equipment;
      if (showUI)
        Refresh_MergeEquip1();
    }
    else if (_mergeEquipment2 == null)
    {
        if (_equipment.equipmentData.mergeEquipmentType2 == EMergeEquipmentType.ItemCode)
        {
          if (equipment.equipmentData.dataId != _equipment.equipmentData.mergeEquipment2)
            return;
        }
        else if (_equipment.equipmentData.mergeEquipmentType2 == EMergeEquipmentType.Grade)
        {
          if (equipment.equipmentData.equipmentGrade != (EEquipmentGrade)Enum.Parse(typeof(EEquipmentGrade), _equipment.equipmentData.mergeEquipment2))
            return;
        }
        else
        {
          return;
        }
        _mergeEquipment2 = equipment;
        
        if (showUI) Refresh_MergeEquip2();
    }
    else
    {
      return;
    }
    
    if (showUI) CheckEnableMergeButton();

    SortEquipments();
  }

  private void Refresh()
  {
    if (_init == false) return;

    Refresh_SelectedEquipObject();
    Refresh_MergeEquip1();
    Refresh_MergeEquip2();
    CheckEnableMergeButton();
    SortEquipments();
  }

  private void Refresh_SelectedEquipObject()
  {
    if (_equipment == null)
    {
      GetObject((int)GameObjects.SelectedEquipObject).SetActive(false);
      GetButton((int)Buttons.FirstCostButton).gameObject.SetActive(true);
      GetButton((int)Buttons.SecondCostButton).gameObject.SetActive(true);
      GetText((int)Texts.SelectEquipmentCommentText).gameObject.SetActive(true);
      GetText((int)Texts.SelectMergeCommentText).gameObject.SetActive(false);
      GetObject((int)GameObjects.OptionResultObject).gameObject.SetActive(false);
      GetImage((int)Images.MergePossibleOutlineImage).gameObject.SetActive(false);
      GetImage((int)Images.SelectedEquipEnforceBackgroundImage).gameObject.SetActive(false);
      return;
    }
    else
    {
      GetImage((int)Images.SelectedEquipImage).sprite = Managers.Resource.Load<Sprite>(_equipment.equipmentData.spriteName);
      GetImage((int)Images.SelectedEquipTypeImage).sprite = Managers.Resource.Load<Sprite>($"{_equipment.equipmentData.equipmentType}_Icon.sprite");

      switch (_equipment.equipmentData.equipmentGrade)
      {
        case EEquipmentGrade.Common:
          GetImage((int)Images.SelectedEquipGradeBackgroundImage).color = EquipmentUIColors.Common;
          GetImage((int)Images.SelectedEquipTypeBackgroundImage).color = EquipmentUIColors.Common;
          break;
        case EEquipmentGrade.Uncommon:
          GetImage((int)Images.SelectedEquipGradeBackgroundImage).color = EquipmentUIColors.Uncommon;
          GetImage((int)Images.SelectedEquipTypeBackgroundImage).color = EquipmentUIColors.Uncommon;
          break;
        case EEquipmentGrade.Rare:
          GetImage((int)Images.SelectedEquipGradeBackgroundImage).color = EquipmentUIColors.Rare;
          GetImage((int)Images.SelectedEquipTypeBackgroundImage).color = EquipmentUIColors.Rare;
          break;
        case EEquipmentGrade.Epic:
        case EEquipmentGrade.Epic1:
        case EEquipmentGrade.Epic2:
          GetImage((int)Images.SelectedEquipGradeBackgroundImage).color = EquipmentUIColors.Epic;
          GetImage((int)Images.SelectedEquipEnforceBackgroundImage).color = EquipmentUIColors.EpicBg;
          GetImage((int)Images.SelectedEquipTypeBackgroundImage).color = EquipmentUIColors.EpicBg;
          break;
        case EEquipmentGrade.Legendary:
        case EEquipmentGrade.Legendary1:
        case EEquipmentGrade.Legendary2:
        case EEquipmentGrade.Legendary3:
          GetImage((int)Images.SelectedEquipGradeBackgroundImage).color = EquipmentUIColors.Legendary;
          GetImage((int)Images.SelectedEquipEnforceBackgroundImage).color = EquipmentUIColors.LegendaryBg;
          GetImage((int)Images.SelectedEquipTypeBackgroundImage).color = EquipmentUIColors.LegendaryBg;
          break;
        default:
            break;
      }

      string gradeName = _equipment.equipmentData.equipmentGrade.ToString();
      int num = 0;

      // Epic1 -> 1 리턴 Epic2 ->2 리턴 Common처럼 숫자가 없으면 0 리턴
      Match match = Regex.Match(gradeName, @"\d+$");
      if (match.Success) num = int.Parse(match.Value);

      if (num == 0)
      {
        GetText((int)Texts.SelectedEquipEnforceValueText).text = "";
        GetImage((int)Images.SelectedEquipEnforceBackgroundImage).gameObject.SetActive(false);
      }
      else
      {
        GetText((int)Texts.SelectedEquipEnforceValueText).text = num.ToString();
        GetImage((int)Images.SelectedEquipEnforceBackgroundImage).gameObject.SetActive(true);
      }

      GetText((int)Texts.SelectedEquipLevelValueText).text = $"Lv.{_equipment.Level}";

      GetObject((int)GameObjects.SelectedEquipObject).SetActive(true);

      GetObject((int)GameObjects.OptionResultObject).gameObject.SetActive(false); // 옵션 결과
      GetText((int)Texts.SelectEquipmentCommentText).gameObject.SetActive(false); // 장비 코멘트
      GetText((int)Texts.SelectMergeCommentText).gameObject.SetActive(true); // 재료 코멘트
    }

    if(_equipment.equipmentData.mergeEquipmentType1 == EMergeEquipmentType.None)
    {
      GetButton((int)Buttons.FirstCostButton).gameObject.SetActive(false);
      GetButton((int)Buttons.SecondCostButton).gameObject.SetActive(false);
    }
    else if(_equipment.equipmentData.mergeEquipmentType2 == EMergeEquipmentType.None)
    {
      // Require 1 upgrade stone 
      GetButton((int)Buttons.FirstCostButton).gameObject.SetActive(true);
      GetButton((int)Buttons.SecondCostButton).gameObject.SetActive(false);
    }
    else
    {
      // Require 2 upgrade stones 
      GetButton((int)Buttons.FirstCostButton).gameObject.SetActive(true);
      GetButton((int)Buttons.SecondCostButton).gameObject.SetActive(true);
    }
  }
  private void Refresh_MergeEquip1()
  {
    if (_mergeEquipment1 == null)
      GetObject((int)GameObjects.FirstCostEquipSelectObject).SetActive(false);
    else
    {
      GetImage((int)Images.FirstSelectEquipImage).sprite = Managers.Resource.Load<Sprite>(_mergeEquipment1.equipmentData.spriteName);
      GetImage((int)Images.FirstSelectEquipTypeImage).sprite = Managers.Resource.Load<Sprite>($"{_mergeEquipment1.equipmentData.equipmentType}_Icon.sprite");
      switch (_mergeEquipment1.equipmentData.equipmentGrade)
      {
        case EEquipmentGrade.Common:
          GetImage((int)Images.FirstSelectEquipGradeBackgroundImage).color = EquipmentUIColors.Common;
          GetImage((int)Images.FirstSelectEquipTypeBackgroundImage).color = EquipmentUIColors.Common;
          break;

        case EEquipmentGrade.Uncommon:
          GetImage((int)Images.FirstSelectEquipGradeBackgroundImage).color = EquipmentUIColors.Uncommon;
          GetImage((int)Images.FirstSelectEquipTypeBackgroundImage).color = EquipmentUIColors.Uncommon;
          break;

        case EEquipmentGrade.Rare:
          GetImage((int)Images.FirstSelectEquipGradeBackgroundImage).color = EquipmentUIColors.Rare;
          GetImage((int)Images.FirstSelectEquipTypeBackgroundImage).color = EquipmentUIColors.Rare;
          break;

        case EEquipmentGrade.Epic:
        case EEquipmentGrade.Epic1:
        case EEquipmentGrade.Epic2:
          GetImage((int)Images.FirstSelectEquipGradeBackgroundImage).color = EquipmentUIColors.Epic;
          GetImage((int)Images.FirstSelectEquipEnforceBackgroundImage).color = EquipmentUIColors.EpicBg;
          GetImage((int)Images.FirstSelectEquipTypeBackgroundImage).color = EquipmentUIColors.EpicBg;
          break;

        case EEquipmentGrade.Legendary:
        case EEquipmentGrade.Legendary1:
        case EEquipmentGrade.Legendary2:
        case EEquipmentGrade.Legendary3:
          GetImage((int)Images.FirstSelectEquipGradeBackgroundImage).color = EquipmentUIColors.Legendary;
          GetImage((int)Images.FirstSelectEquipEnforceBackgroundImage).color = EquipmentUIColors.LegendaryBg;
          GetImage((int)Images.FirstSelectEquipTypeBackgroundImage).color = EquipmentUIColors.LegendaryBg;
          break;

        default:
          break;
      }
      string gradeName = _mergeEquipment1.equipmentData.equipmentGrade.ToString();
      int num = 0;

      // Epic1 -> 1 리턴 Epic2 ->2 리턴 Common처럼 숫자가 없으면 0 리턴
      Match match = Regex.Match(gradeName, @"\d+$");
      if (match.Success) num = int.Parse(match.Value);

      if (num == 0)
      {
        GetText((int)Texts.FirstSelectEquipEnforceValueText).text = "";
        GetImage((int)Images.FirstSelectEquipEnforceBackgroundImage).gameObject.SetActive(false);
      }
      else
      {
        GetText((int)Texts.FirstSelectEquipEnforceValueText).text = num.ToString();
        GetImage((int)Images.FirstSelectEquipEnforceBackgroundImage).gameObject.SetActive(true);
      }

      GetText((int)Texts.FirstSelectEquipLevelValueText).text = $"Lv.{_mergeEquipment1.Level}";
      GetObject((int)GameObjects.FirstCostEquipSelectObject).SetActive(true);
    }
  }
  private void Refresh_MergeEquip2()
  {
    if (_mergeEquipment2 == null)
      GetObject((int)GameObjects.SecondCostEquipSelectObject).SetActive(false);
    else
    {
      GetImage((int)Images.SecondSelectEquipImage).sprite = Managers.Resource.Load<Sprite>(_mergeEquipment2.equipmentData.spriteName);
      GetImage((int)Images.SecondSelectEquipTypeImage).sprite = Managers.Resource.Load<Sprite>($"{_mergeEquipment2.equipmentData.equipmentType}_Icon.sprite");

      switch (_mergeEquipment2.equipmentData.equipmentGrade)
      {
          case EEquipmentGrade.Common:
            GetImage((int)Images.SecondSelectEquipGradeBackgroundImage).color = EquipmentUIColors.Common;
            GetImage((int)Images.SecondSelectEquipTypeBackgroundImage).color = EquipmentUIColors.Common;
            break;

          case EEquipmentGrade.Uncommon:
            GetImage((int)Images.SecondSelectEquipGradeBackgroundImage).color = EquipmentUIColors.Uncommon;
            GetImage((int)Images.SecondSelectEquipTypeBackgroundImage).color = EquipmentUIColors.Uncommon;
            break;

          case EEquipmentGrade.Rare:
            GetImage((int)Images.SecondSelectEquipGradeBackgroundImage).color = EquipmentUIColors.Rare;
            GetImage((int)Images.SecondSelectEquipTypeBackgroundImage).color = EquipmentUIColors.Rare;
            break;

          case EEquipmentGrade.Epic:
          case EEquipmentGrade.Epic1:
          case EEquipmentGrade.Epic2:
            GetImage((int)Images.SecondSelectEquipGradeBackgroundImage).color = EquipmentUIColors.Epic;
            GetImage((int)Images.SecondSelectEquipEnforceBackgroundImage).color = EquipmentUIColors.EpicBg;
            GetImage((int)Images.SecondSelectEquipTypeBackgroundImage).color = EquipmentUIColors.EpicBg;
            break;

          case EEquipmentGrade.Legendary:
          case EEquipmentGrade.Legendary1:
          case EEquipmentGrade.Legendary2:
          case EEquipmentGrade.Legendary3:
            GetImage((int)Images.SecondSelectEquipGradeBackgroundImage).color = EquipmentUIColors.Legendary;
            GetImage((int)Images.SecondSelectEquipEnforceBackgroundImage).color = EquipmentUIColors.LegendaryBg;
            GetImage((int)Images.SecondSelectEquipTypeBackgroundImage).color = EquipmentUIColors.LegendaryBg;
            break;

          default:
            break;
      }

      string gradeName = _mergeEquipment2.equipmentData.equipmentGrade.ToString();
      int num = 0;

      // Epic1 -> 1 리턴 Epic2 ->2 리턴 Common처럼 숫자가 없으면 0 리턴
      Match match = Regex.Match(gradeName, @"\d+$");
      if (match.Success) num = int.Parse(match.Value);

      if (num == 0)
      {
          GetText((int)Texts.SecondSelectEquipEnforceValueText).text = "";
          GetImage((int)Images.SecondSelectEquipEnforceBackgroundImage).gameObject.SetActive(false);
      }
      else
      {
          GetText((int)Texts.SecondSelectEquipEnforceValueText).text = num.ToString();
          GetImage((int)Images.SecondSelectEquipEnforceBackgroundImage).gameObject.SetActive(true);
      }

      GetText((int)Texts.SecondSelectEquipLevelValueText).text = $"Lv.{_mergeEquipment2.Level}";
      GetObject((int)GameObjects.SecondCostEquipSelectObject).SetActive(true);
    }
  }
  private bool CheckEnableMergeButton()
  {
    #region Check Item Merge
    if (_equipment == null)
    {
      GetButton((int)Buttons.MergeButton).gameObject.SetActive(false);
      GetObject((int)GameObjects.MergeStartEffect).gameObject.SetActive(false); // 합성 시작 이펙트
      GetObject((int)GameObjects.MergeFinishEffect).gameObject.SetActive(false); // 합성 끝 이펙트
      return false;
    }

    if(_mergeEquipment2 == null && GetButton((int)Buttons.SecondCostButton).gameObject.activeSelf)
    {
      GetButton((int)Buttons.MergeButton).gameObject.SetActive(false);
      GetImage((int)Images.MergePossibleOutlineImage).gameObject.SetActive(false); // 아웃라인
      GetObject((int)GameObjects.MergeStartEffect).gameObject.SetActive(false); // 합성 시작 이펙트
      GetObject((int)GameObjects.MergeFinishEffect).gameObject.SetActive(false); // 합성 끝 이펙트
      return false;
    }

    if(_mergeEquipment1 == null)
    {
      GetButton((int)Buttons.MergeButton).gameObject.SetActive(false);
      GetImage((int)Images.MergePossibleOutlineImage).gameObject.SetActive(false); // 아웃라인
      GetObject((int)GameObjects.MergeStartEffect).gameObject.SetActive(false); // 합성 시작 이펙트
      GetObject((int)GameObjects.MergeFinishEffect).gameObject.SetActive(false); // 합성 끝 이펙트
      return false;
    }
        
    GetObject((int)GameObjects.OptionResultObject).gameObject.SetActive(true); // 옵션 결과
    GetText((int)Texts.SelectEquipmentCommentText).gameObject.SetActive(false); // 장비 코맨트
    GetText((int)Texts.SelectMergeCommentText).gameObject.SetActive(false); // 재료 코멘트
    GetObject((int)GameObjects.MergeStartEffect).gameObject.SetActive(true); // 합성 시작 이펙트
    GetObject((int)GameObjects.MergeFinishEffect).gameObject.SetActive(false); // 합성 끝 이펙트

    #endregion
    
    #region Options
    GetImage((int)Images.SelectedEquipImage).sprite = Managers.Resource.Load<Sprite>(_equipment.equipmentData.spriteName);
    string mergedItemId = _equipment.equipmentData.mergedItemCode;
    GetImage((int)Images.MergePossibleOutlineImage).gameObject.SetActive(true);
    GetImage((int)Images.SelectedEquipEnforceBackgroundImage).gameObject.SetActive(false);
    GetObject((int)GameObjects.OptionResultObject).gameObject.SetActive(true); // 옵션 결과
    GetText((int)Texts.EquipmentNameText).text = $"{Managers.Data.EquipDataDic[mergedItemId].nameTextID}"; // 이름
    GetText((int)Texts.BeforeLevelValueText).text = $"{Managers.Data.EquipDataDic[_equipment.equipmentData.dataId].maxLevel}"; // 합성 전 최고 레벨
    GetText((int)Texts.AfterLevelValueText).text = $"{Managers.Data.EquipDataDic[mergedItemId].maxLevel}"; // 합성 후 최고 레벨

    if (Managers.Data.EquipDataDic[mergedItemId].atkDmgBonus != 0) // 장비의 공격력이 0이 아니면 무기
    {
      // 공격력 옵션
      GetObject((int)GameObjects.ImprovATKObject).gameObject.SetActive(true);
      GetObject((int)GameObjects.ImprovHPObject).gameObject.SetActive(false);

      GetText((int)Texts.BeforeATKValueText).text = $"{Managers.Data.EquipDataDic[_equipment.equipmentData.dataId].maxLevel}"; // 합성 전 공격력
      GetText((int)Texts.AfterATKValueText).text = $"{Managers.Data.EquipDataDic[mergedItemId].maxLevel}"; // 합성 후 공격력
    }
    else
    {
      // 체력 옵션
      GetObject((int)GameObjects.ImprovATKObject).gameObject.SetActive(false);
      GetObject((int)GameObjects.ImprovHPObject).gameObject.SetActive(true);

      GetText((int)Texts.BeforeHPValueText).text = Managers.Data.EquipDataDic[_equipment.equipmentData.dataId].maxLevel.ToString(); // 합성 전 체력
      GetText((int)Texts.AfterHPValueText).text = Managers.Data.EquipDataDic[mergedItemId].maxLevel.ToString(); // 합성 후 체력 
    }

    // 등급별 처리
    switch (Managers.Data.EquipDataDic[mergedItemId].equipmentGrade)
    {
      case EEquipmentGrade.Uncommon:
        GetImage((int)Images.SelectedEquipGradeBackgroundImage).color = EquipmentUIColors.Uncommon;
        GetImage((int)Images.MergePossibleOutlineImage).color = EquipmentUIColors.Uncommon;
        GetImage((int)Images.SelectedEquipTypeBackgroundImage).color = EquipmentUIColors.Uncommon;

        int uncommonGradeSkillId = Managers.Data.EquipDataDic[mergedItemId].uncommonGradeSkill;
        GetText((int)Texts.ImprovOptionValueText).text = $"+ {Managers.Data.SupportSkillDic[uncommonGradeSkillId].description}"; // 추가 옵션
        GetText((int)Texts.BeforeGradeValueText).text = "일반"; // 합성 전 등급
        GetText((int)Texts.AfterGradeValueText).text = "고급"; // 합성 후 등급
        break;
      case EEquipmentGrade.Rare:
        GetImage((int)Images.SelectedEquipGradeBackgroundImage).color = EquipmentUIColors.Rare;
        GetImage((int)Images.MergePossibleOutlineImage).color = EquipmentUIColors.Rare;
        GetImage((int)Images.SelectedEquipTypeBackgroundImage).color = EquipmentUIColors.Rare;

        int rareGradeSkillId = Managers.Data.EquipDataDic[mergedItemId].rareGradeSkill;
        GetText((int)Texts.ImprovOptionValueText).text = $"+ {Managers.Data.SupportSkillDic[rareGradeSkillId].description}"; // 추가 옵션

        GetText((int)Texts.BeforeGradeValueText).text = "고급"; // 합성 전 등급
        GetText((int)Texts.AfterGradeValueText).text = "희귀"; // 합성 후 등급
        break;
      case EEquipmentGrade.Epic:
        GetImage((int)Images.SelectedEquipGradeBackgroundImage).color = EquipmentUIColors.Epic;
        GetImage((int)Images.MergePossibleOutlineImage).color = EquipmentUIColors.Epic;
        GetImage((int)Images.SelectedEquipTypeBackgroundImage).color = EquipmentUIColors.EpicBg;

        int epicGradeSkillId = Managers.Data.EquipDataDic[mergedItemId].epicGradeSkill;
        GetText((int)Texts.ImprovOptionValueText).text = $"+ {Managers.Data.SupportSkillDic[epicGradeSkillId].description}"; // 추가 옵션

        GetText((int)Texts.BeforeGradeValueText).text = "희귀"; // 합성 전 등급
        GetText((int)Texts.AfterGradeValueText).text = "에픽"; // 합성 후 등급
        break;
      case EEquipmentGrade.Epic1:
        GetImage((int)Images.SelectedEquipGradeBackgroundImage).color = EquipmentUIColors.Epic;
        GetImage((int)Images.MergePossibleOutlineImage).color = EquipmentUIColors.Epic;
        GetImage((int)Images.SelectedEquipTypeBackgroundImage).color = EquipmentUIColors.EpicBg;

        GetImage((int)Images.SelectedEquipEnforceBackgroundImage).gameObject.SetActive(true);
        GetImage((int)Images.SelectedEquipEnforceBackgroundImage).color = EquipmentUIColors.EpicBg;
        GetText((int)Texts.SelectedEquipEnforceValueText).text = "1";

        GetText((int)Texts.BeforeGradeValueText).text = "에픽"; // 합성 전 등급
        GetText((int)Texts.AfterGradeValueText).text = "에픽 1"; // 합성 후 등급
          break;
      case EEquipmentGrade.Epic2:
        GetImage((int)Images.SelectedEquipGradeBackgroundImage).color = EquipmentUIColors.Epic;
        GetImage((int)Images.MergePossibleOutlineImage).color = EquipmentUIColors.Epic;
        GetImage((int)Images.SelectedEquipTypeBackgroundImage).color = EquipmentUIColors.EpicBg;

        GetImage((int)Images.SelectedEquipEnforceBackgroundImage).gameObject.SetActive(true);
        GetImage((int)Images.SelectedEquipEnforceBackgroundImage).color = EquipmentUIColors.EpicBg;
        GetText((int)Texts.SelectedEquipEnforceValueText).text = "2";

        GetText((int)Texts.BeforeGradeValueText).text = "에픽 1"; // 합성 전 등급
        GetText((int)Texts.AfterGradeValueText).text = "에픽 2"; // 합성 후 등급
          break;
      case EEquipmentGrade.Legendary:
        GetImage((int)Images.SelectedEquipGradeBackgroundImage).color = EquipmentUIColors.Legendary;
        GetImage((int)Images.MergePossibleOutlineImage).color = EquipmentUIColors.Legendary;
        GetImage((int)Images.SelectedEquipTypeBackgroundImage).color = EquipmentUIColors.LegendaryBg;
        
        int legendaryGradeSkillId = Managers.Data.EquipDataDic[mergedItemId].legendaryGradeSkill;
        GetText((int)Texts.ImprovOptionValueText).text = $"+ {Managers.Data.SupportSkillDic[legendaryGradeSkillId].description}"; // 추가 옵션
        GetText((int)Texts.BeforeGradeValueText).text = "에픽 2"; // 합성 전 등급
        GetText((int)Texts.AfterGradeValueText).text = "전설"; // 합성 후 등급
          break;
      case EEquipmentGrade.Legendary1:
        GetImage((int)Images.SelectedEquipGradeBackgroundImage).color = EquipmentUIColors.Legendary;
        GetImage((int)Images.MergePossibleOutlineImage).color = EquipmentUIColors.Legendary;
        GetImage((int)Images.SelectedEquipTypeBackgroundImage).color = EquipmentUIColors.LegendaryBg;

        GetImage((int)Images.SelectedEquipEnforceBackgroundImage).gameObject.SetActive(true);
        GetImage((int)Images.SelectedEquipEnforceBackgroundImage).color = EquipmentUIColors.LegendaryBg;
        GetText((int)Texts.SelectedEquipEnforceValueText).text = "1";

        GetText((int)Texts.BeforeGradeValueText).text = "전설"; // 합성 전 등급
        GetText((int)Texts.AfterGradeValueText).text = "전설 1"; // 합성 후 등급
          break;
      case EEquipmentGrade.Legendary2:
        GetImage((int)Images.SelectedEquipGradeBackgroundImage).color = EquipmentUIColors.Legendary;
        GetImage((int)Images.MergePossibleOutlineImage).color = EquipmentUIColors.Legendary;
        GetImage((int)Images.SelectedEquipTypeBackgroundImage).color = EquipmentUIColors.LegendaryBg;

        GetImage((int)Images.SelectedEquipEnforceBackgroundImage).gameObject.SetActive(true);
        GetImage((int)Images.SelectedEquipEnforceBackgroundImage).color = EquipmentUIColors.LegendaryBg;
        GetText((int)Texts.SelectedEquipEnforceValueText).text = "2";

        GetText((int)Texts.BeforeGradeValueText).text = "전설 1"; // 합성 전 등급
        GetText((int)Texts.AfterGradeValueText).text = "전설 2"; // 합성 후 등급
        break;
      case EEquipmentGrade.Legendary3:
        GetImage((int)Images.SelectedEquipGradeBackgroundImage).color = EquipmentUIColors.Legendary;
        GetImage((int)Images.MergePossibleOutlineImage).color = EquipmentUIColors.Legendary;
        GetImage((int)Images.SelectedEquipTypeBackgroundImage).color = EquipmentUIColors.LegendaryBg;

        GetImage((int)Images.SelectedEquipEnforceBackgroundImage).gameObject.SetActive(true);
        GetImage((int)Images.SelectedEquipEnforceBackgroundImage).color = EquipmentUIColors.LegendaryBg;
        GetText((int)Texts.SelectedEquipEnforceValueText).text = "3";

        GetText((int)Texts.BeforeGradeValueText).text = "전설 2"; // 합성 전 등급
        GetText((int)Texts.AfterGradeValueText).text = "전설 3"; // 합성 후 등급
        break;

      default:
        break;
    }
    #endregion
    
    GetButton((int)Buttons.MergeButton).gameObject.SetActive(true);
    return true;
  }
  private void SortEquipments()
  {
    Managers.Game.SortEquipment(_equipmentSortType);
    GetObject((int)GameObjects.EquipInventoryScrollContentObject).DestroyChildren();
    
    // 장비 리스트 작성
    foreach (Equipment inventoryEquipmentItem in Managers.Game.OwnedEquipments)
    {
      bool isSelected = false;
      bool isLock = false;
      // 장비 상태 결정 (선택됨 or 선택 불가 or 선택 가능)
      if (_equipment != null)
      {
        if (_equipment == inventoryEquipmentItem || _mergeEquipment1 == inventoryEquipmentItem || _mergeEquipment2 == inventoryEquipmentItem)
        {
          isSelected = true;
          continue;
        }
        else if(_mergeEquipment1 != null) // 2개 재료가 모두 있거나 합성 불가능이면 잠금
        {
          if (_equipment.equipmentData.mergeEquipmentType2 == EMergeEquipmentType.None || _mergeEquipment2 != null)
          {
            isLock = true;
          }
        }
        
        if (_equipment.equipmentData.equipmentType != inventoryEquipmentItem.equipmentData.equipmentType) // 타입 검사
        {
          isLock = true;
        }
        else
        {
          if (_equipment.equipmentData.mergeEquipmentType1 != EMergeEquipmentType.None && _mergeEquipment1 == null) // 1번 재료 판단
          {
            if (_equipment.equipmentData.mergeEquipmentType1 == EMergeEquipmentType.ItemCode) // 합성 재료가 동일 아이템 일때
            {
              if (inventoryEquipmentItem.equipmentData.dataId != _equipment.equipmentData.mergeEquipment1)
                isLock = true;
            }
            else if (_equipment.equipmentData.mergeEquipmentType1 == EMergeEquipmentType.Grade) // 합성 재료가 동일 등급 일때
            {
              if (inventoryEquipmentItem.equipmentData.equipmentGrade != (EEquipmentGrade)Enum.Parse(typeof(EEquipmentGrade), _equipment.equipmentData.mergeEquipment1))
                isLock = true;
            }
          }
          
          if (_equipment.equipmentData.mergeEquipmentType2 != EMergeEquipmentType.None && _mergeEquipment2 == null) // 2번 재료 판단
          {
            if (_equipment.equipmentData.mergeEquipmentType2 == EMergeEquipmentType.ItemCode) // 합성 재료가 동일 아이템 일때
            {
              if (inventoryEquipmentItem.equipmentData.dataId != _equipment.equipmentData.mergeEquipment2)
                isLock = true;
            }
            else if (_equipment.equipmentData.mergeEquipmentType2 == EMergeEquipmentType.Grade) // 합성 재료가 동일 등급 일때
            {
              if (inventoryEquipmentItem.equipmentData.equipmentGrade != (EEquipmentGrade)Enum.Parse(typeof(EEquipmentGrade), _equipment.equipmentData.mergeEquipment2))
                isLock = true;
            }
            if (inventoryEquipmentItem.Level > 1)
              isLock = true;
            if (inventoryEquipmentItem.IsEquipped)
              isLock = true;
          }
        }
      }
      UI_MergeEquipItem item = Managers.UI.MakeSubItem<UI_MergeEquipItem>(GetObject((int)GameObjects.EquipInventoryScrollContentObject).transform);
      item.OnClickEquipItem = () =>
      {
        inventoryEquipmentItem.IsConfirmed = true;
      };
      item.SetInfo(inventoryEquipmentItem, Define.UI_ItemParentType.EquipInventoryGroup, _scrollRect, isSelected, isLock);
    }
  }
  
  private void OnClickBackButton()
  {
    Managers.Sound.PlayPopupClose();
    gameObject.SetActive(false);
    (Managers.UI.SceneUI as UI_LobbyScene)?.EquipmentPopupUI.SetInfo();
  }
  private void OnClickEquipResultButton()
  {
    Managers.Sound.PlayButtonClick();
    _equipment = null;
    _mergeEquipment1 = null;
    _mergeEquipment2 = null;
    Refresh();
  }
  private void OnClickFirstCostButton()
  {
    Managers.Sound.PlayButtonClick();
    _mergeEquipment1 = null;
    Refresh();
  }
  private void OnClickSecondCostButton()
  {
    Managers.Sound.PlayButtonClick();
    _mergeEquipment2 = null;
    Refresh();
  }
  private void OnClickSortButton() // 정렬 버튼
  {
    Managers.Sound.PlayButtonClick();
    // 레벨로 정렬, 등급으로 정렬 누를때마다 정렬방식 변경
    if (_equipmentSortType == EEquipmentSortType.Level)
    {
      _equipmentSortType = EEquipmentSortType.Grade;
      GetText((int)Texts.SortButtonText).text = _sortText_Grade;
    }
    else if (_equipmentSortType == EEquipmentSortType.Grade)
    {
      _equipmentSortType = EEquipmentSortType.Level;
      GetText((int)Texts.SortButtonText).text = _sortText_Level;
    }

    SortEquipments();
  }
  private void OnClickMergeAllButton()
  {
    Managers.Sound.PlayButtonClick();
    StartCoroutine(CoMergeAll());
  }
  private void OnClickMergeButton()
  {
    Managers.Sound.PlayButtonClick();
 
    Equipment beforeEquipment = _equipment;
    // 합성을 하고 합성의 결과물을 인벤토리에 추가한다.
    Equipment newItem = Managers.Game.MergeEquipment(_equipment, _mergeEquipment1, _mergeEquipment2);

    UI_MergeResultPopup resultPopup = (Managers.UI.SceneUI as UI_LobbyScene)?.MergeResultPopupUI;
    resultPopup.SetInfo(beforeEquipment, newItem, OnClosedMergeResultPopup);
    resultPopup.gameObject.SetActive(true);
    
    SortEquipments();
  }
  private void OnClosedMergeResultPopup()
  {
    OnClickEquipResultButton();
  }
  private IEnumerator CoMergeAll()
  {
    // 자동 합성 버튼
    Managers.Game.SortEquipment(EEquipmentSortType.Grade);

    Equipment[] equipments = Managers.Game.OwnedEquipments.ToArray();
    List<Equipment> newEquipments = new List<Equipment>();
    for (int i = 0; i < equipments.Length; i++)
    {
      if (equipments[i].equipmentData.equipmentGrade > EEquipmentGrade.Epic) continue;

      if (equipments[i].IsEquipped) continue;

      if (equipments[i] != null)
        SetMergeItem(equipments[i], false);

      if (_equipment == null) continue;

      for (int j = i; j < equipments.Length; j++)
      {
        if (equipments[j] != null)
        {
          SetMergeItem(equipments[j], false);
          if (CheckEnableMergeButton())
          {
            Equipment newItem = Managers.Game.MergeEquipment(_equipment, _mergeEquipment1, _mergeEquipment2, true);
            if (newItem != null)
              newEquipments.Add(newItem);
          }
        }
      }
      if(i % 5 == 0)
        yield return new WaitForEndOfFrame();
      
      _equipment = null;
      _mergeEquipment1 = null;
      _mergeEquipment2 = null;
    }
    
    SortEquipments();
    if (newEquipments.Count > 0)
      Managers.UI.ShowPopupUI<UI_MergeAllResultPopup>().SetInfo(newEquipments);
    
    Managers.Game.SaveGame();
  }
}
