using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using Unity.VisualScripting;

public class UI_EquipmentInfoPopup : UI_Popup
{
  #region UI Feature List
  // 정보 갱신
  // EquipmentGradeValueText : 대상 장비의 등급 표시 및 색상을 등급에 맞추어 변경
  // - 일반(Common) : #A2A2A2
  // - 고급(Uncommon)  : #57FF0B
  // - 희귀(Rare) : #2471E0
  // - 유일(Epic) : #9F37F2
  // - 전설(Legendary) : #F67B09
  // - 신화(Myth) : #F1331A
  // EquipmentNameValueText : 대상 장비의 이름
  // EquipmentGradeBackgroundImage : 보상 아이템의 테두리 (색상 변경)
  // - 일반(Common) : #AC9B83
  // - 고급(Uncommon)  : #73EC4E
  // - 희귀(Rare) : #0F84FF
  // - 유일(Epic) : #B740EA
  // - 전설(Legendary) : #F19B02
  // - 신화(Myth) : #FC2302
  // EquipmentLevelValueText : 장비의 레벨 (현재 레벨/최대 레벨)
  // EquipmentOptionImage : 장비 옵션의 아이콘
  // EquipmentOptionValueText : 장비 옵션 수치
  // UncommonSkillOptionDescriptionValueText : 고급 장비 옵션 설명
  // RareSkillOptionDescriptionValueText : 희귀 장비 옵션 설명
  // EpicSkillOptionDescriptionValueText : 유일 장비 옵션 설명
  // LegendarySkillOptionDescriptionValueText : 전설 장비 옵션 설명
  // MythSkillOptionDescriptionValueText : 신화 장비 옵션 설명
  // 만약 장비 데이터 테이블의 각 등급셜 옵션(스킬ID)에 스킬이 없다면 등급에 맞는 옵션 오브젝트 비활성화
  // - 고급(Uncommon)  : UncommonSkillOptionObject
  // - 희귀(Rare) : RareSkillOptionObject
  // - 유일(Epic) : EpicSkillOptionObject
  // - 전설(Legendary) : LegendarySkillOptionObject
  // - 신화(Myth) : MythSkillOptionObject // 제거
  // EquipmentDescriptionValueText : 대상 장비의 설명 텍스트 // 제거
  // CostGoldValueText : 레벨업 비용 (보유 / 필요) 만약 코스트가 부족하다면 보유량을 빨간색(#F3614D)으로 보여준다. 부족하지 않다면 흰색(#FFFFFF)
  // CostMaterialImage : 레벨업 재료의 아이콘
  // CostMaterialValueText : 레벨업 재료 (보유 / 필요) 만약 코스트가 부족하다면 보유량을 빨간색(#F3614D)으로 보여준다. 부족하지 않다면 흰색(#FFFFFF)

  // 로컬라이징 텍스트
  // BackgroundText : 탭하여 닫기
  // EquipmentGradeSkillText : 등급 스킬
  // EquipButtonText : 장착
  // UnequipButtonText : 해제
  // LevelupButtonText : 레벨업
  // MergeButtonText : 합성
  #endregion
  
  #region Enum For Binding UI Automatically
  enum GameObjects
  {
    ContentObject,
    UncommonSkillOptionObject,
    RareSkillOptionObject,
    EpicSkillOptionObject,
    LegendarySkillOptionObject,
    EquipmentGradeSkillScrollContentObject,
    ButtonGroupObject,
    CostGoldObject,
    CostMaterialObject,
    LevelupCostGroupObject,
  }
  enum Buttons
  {
    BackgroundButton,
    EquipmentResetButton,
    EquipButton,
    UnquipButton,
    LevelupButton,
    MergeButton,
  }
  enum Texts
  {
    EquipmentGradeValueText,
    EquipmentNameValueText,
    EquipmentLevelValueText,
    EquipmentOptionValueText,
    UncommonSkillOptionDescriptionValueText,
    RareSkillOptionDescriptionValueText,
    EpicSkillOptionDescriptionValueText,
    LegendarySkillOptionDescriptionValueText,
    CostGoldValueText,
    CostMaterialValueText,
    EquipButtonText,
    UnequipButtonText,
    LevelupButtonText,
    MergeButtonText,
    EquipmentGradeSkillText,
    BackgroundText,
    EnforceValueText,
  }
  enum Images
  {
    EquipmentGradeBackgroundImage,
    EquipmentOptionImage,
    CostMaterialImage,
    EquipmentImage,
    GradeBackgroundImage,
    EquipmentEnforceBackgroundImage,
    EquipmentTypeBackgroundImage,
    EquipmentTypeImage,

    UncommonSkillLockImage,
    RareSkillLockImage,
    EpicSkillLockImage,
    LegendarySkillLockImage,
  }
  #endregion
  
  private  Equipment _equipment;
  
  private void Awake()
  {
    Init();
  }
  private void OnEnable()
  {
    PopupOpenAnimation(GetObject((int)GameObjects.ContentObject));
    LayoutRebuilder.ForceRebuildLayoutImmediate(GetObject((int)GameObjects.EquipmentGradeSkillScrollContentObject).GetComponent<RectTransform>());
  }

  protected override bool Init()
  {
    if (base.Init() == false) return false;

    BindObject(typeof(GameObjects));
    BindButton(typeof(Buttons));
    BindText(typeof(Texts));
    BindImage(typeof(Images));

    GetButton((int)Buttons.BackgroundButton).gameObject.BindEvent(OnClickBackgroundButton);
    GetButton((int)Buttons.EquipmentResetButton).gameObject.BindEvent(OnClickEquipmentResetButton); 
    GetButton((int)Buttons.EquipmentResetButton).GetOrAddComponent<UI_ButtonAnimation>();
    GetButton((int)Buttons.EquipButton).gameObject.BindEvent(OnClickEquipButton);
    GetButton((int)Buttons.EquipButton).GetOrAddComponent<UI_ButtonAnimation>();
    GetButton((int)Buttons.UnquipButton).gameObject.BindEvent(OnClickUnequipButton);
    GetButton((int)Buttons.UnquipButton).GetOrAddComponent<UI_ButtonAnimation>();
    GetButton((int)Buttons.LevelupButton).gameObject.BindEvent(OnClickLevelupButton);
    GetButton((int)Buttons.LevelupButton).GetOrAddComponent<UI_ButtonAnimation>();
    GetButton((int)Buttons.MergeButton).gameObject.BindEvent(OnClickMergeButton);
    GetButton((int)Buttons.MergeButton).GetOrAddComponent<UI_ButtonAnimation>();
    
    return true;
  }
  
  public void SetInfo(Equipment equipment)
  {
    _equipment = equipment;
    Refresh();
  }
  
  private void OnClickMergeButton() // 합성 버튼
  {
    Managers.Sound.PlayButtonClick();
    if (_equipment.IsEquipped) return;
    UI_MergePopup mergePopupUI = (Managers.UI.SceneUI as UI_LobbyScene).MergePopupUI;
    mergePopupUI.SetInfo(_equipment);
    mergePopupUI.gameObject.SetActive(true);
  }
}
