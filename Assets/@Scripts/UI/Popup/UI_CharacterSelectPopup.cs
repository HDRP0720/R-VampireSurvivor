using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_CharacterSelectPopup : UI_Popup
{
  #region UI Feature List
  // 정보 갱신
  // StarOn_0 ~ 4 : 스킬 레벨에 따라 활성화
  // CharacterImage : 선택된 캐릭터의 이미지
  // CharacterNameValueText : 선택된 캐릭터의 이름
  // AttackValueText : 캐릭터의 최종 공격력 
  // AttackBonusValueText : 증가 공격력 (+N)으로 표기
  // HealthValueText : 캐릭터의 최종 체력
  // HealthBonusValueText : 증가 체력 (+N)으로 표기

  // EnhanceCostGoldValueText : 강화 시 필요한 골드
  // EnhanceCostMaterialValueText : 강화 시 필요한 재료
  // UpgradeCostMaterialValueText : 업그레이드 시 필요한 재료

  // 로컬라이징
  // CharacterInventoryTlileText : 캐릭터 목록
  // EnhanceButtonText : 강화
  // EquipButtonText : 장착
  // LevelUpButtonText : 레벨업
  #endregion

  #region Enum For Binding UI Automatically
  enum GameObjects
  {
    ContentObject,
    CharacterLevelObject,
    AttackPointObject,
    HealthPointObject,
    CharacterEnhancePanelObject,
    CharacterEnhanceContentObject,
    CharacterUpgradeContentObject,
    EnhanceCostObject,
    UpgradeCostObject,
  }
  enum Images
  {
    StarOn_1,
    StarOn_2,
    StarOn_3,
    StarOn_4,
    AttackImage,
    CharacterImage,
    HealthImage,
  }
  enum Buttons
  {
    EnhanceButton,
    LevelUpButton,
    EquipButton,
    BackButton,
  }
  enum Texts
  {
    CharacterNameValueText,
    AttackValueText,
    AttackBonusValueText,
    HealthValueText,
    HealthBonusValueText,
    CharacterInventoryTlileText,
    EnhanceButtonText,
    EquipButtonText,
    LevelUpButtonText,
    EnhanceCostGoldValueText,
    EnhanceCostMaterialValueText,
    UpgradeCostMaterialValueText,
  }
  enum Toggles
  {
    EnhanceToggle,
    UpgradeToggle,
  }
  #endregion
  
  private bool _isCharacterEnhancePanelOpen = false;
  
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
    BindToggle(typeof(Toggles));

    GetObject((int)GameObjects.CharacterEnhanceContentObject).gameObject.SetActive(false);
    GetObject((int)GameObjects.CharacterUpgradeContentObject).gameObject.SetActive(false);
    GetObject((int)GameObjects.CharacterEnhancePanelObject).gameObject.SetActive(false);
    _isCharacterEnhancePanelOpen = false;
    GetImage((int)Images.StarOn_1).gameObject.SetActive(false);
    GetImage((int)Images.StarOn_2).gameObject.SetActive(false);
    GetImage((int)Images.StarOn_3).gameObject.SetActive(false);
    GetImage((int)Images.StarOn_4).gameObject.SetActive(false);

    GetButton((int)Buttons.EnhanceButton).gameObject.BindEvent(OnClickEnhanceButton);
    GetButton((int)Buttons.EnhanceButton).GetOrAddComponent<UI_ButtonAnimation>();
    GetButton((int)Buttons.LevelUpButton).gameObject.BindEvent(OnClickLevelUpButton);
    GetButton((int)Buttons.LevelUpButton).GetOrAddComponent<UI_ButtonAnimation>();
    GetButton((int)Buttons.LevelUpButton).gameObject.SetActive(false);
    GetButton((int)Buttons.EquipButton).gameObject.BindEvent(OnClickEquipButton);
    GetButton((int)Buttons.EquipButton).GetOrAddComponent<UI_ButtonAnimation>();
    GetButton((int)Buttons.BackButton).gameObject.BindEvent(OnClickBackButton);
    GetButton((int)Buttons.BackButton).GetOrAddComponent<UI_ButtonAnimation>();
    GetToggle((int)Toggles.EnhanceToggle).gameObject.BindEvent(OnClickEnhanceToggle);
    GetToggle((int)Toggles.UpgradeToggle).gameObject.BindEvent(OnClickUpgradeToggle);

    Refresh();
    return true;
  }
  
  private void Refresh()
  {
    // 리프레시 버그 대응
    LayoutRebuilder.ForceRebuildLayoutImmediate(GetObject((int)GameObjects.CharacterLevelObject).GetComponent<RectTransform>());
    LayoutRebuilder.ForceRebuildLayoutImmediate(GetObject((int)GameObjects.AttackPointObject).GetComponent<RectTransform>());
    LayoutRebuilder.ForceRebuildLayoutImmediate(GetObject((int)GameObjects.HealthPointObject).GetComponent<RectTransform>());
    LayoutRebuilder.ForceRebuildLayoutImmediate(GetObject((int)GameObjects.EnhanceCostObject).GetComponent<RectTransform>());
    LayoutRebuilder.ForceRebuildLayoutImmediate(GetObject((int)GameObjects.UpgradeCostObject).GetComponent<RectTransform>());
  }
  
  // 선택 버튼
  private void OnClickEquipButton() 
  {
    // TODO: 현재 선택된 캐릭터를 장착한다.
  }
  private void OnClickEnhanceButton() 
  {
    Managers.Sound.PlayButtonClick();

    GetButton((int)Buttons.EnhanceButton).gameObject.SetActive(false);
    GetButton((int)Buttons.LevelUpButton).gameObject.SetActive(true);

    GetObject((int)GameObjects.CharacterEnhancePanelObject).gameObject.SetActive(true);
    _isCharacterEnhancePanelOpen = true;
    OnClickEnhanceToggle();
    GetButton((int)Buttons.EquipButton).gameObject.SetActive(false);
  }
  private void OnClickLevelUpButton()
  {
    Managers.Sound.PlayButtonClick();
  }
  private void OnClickBackButton()
  {
    Managers.Sound.PlayButtonClick();
    if (_isCharacterEnhancePanelOpen)
    {
      GetButton((int)Buttons.EnhanceButton).gameObject.SetActive(true);
      GetButton((int)Buttons.LevelUpButton).gameObject.SetActive(false);
      GetObject((int)GameObjects.CharacterEnhancePanelObject).gameObject.SetActive(false); // 강화 패널 닫기
      _isCharacterEnhancePanelOpen = false;
      GetButton((int)Buttons.EquipButton).gameObject.SetActive(true);
    }
    else
    {
      Managers.UI.ClosePopupUI(this);
    }
  }
  private void OnClickEnhanceToggle()
  {
    Managers.Sound.PlayButtonClick();
    GetObject((int)GameObjects.CharacterEnhanceContentObject).gameObject.SetActive(true);
    GetObject((int)GameObjects.CharacterUpgradeContentObject).gameObject.SetActive(false);
    Refresh();
  }
  private void OnClickUpgradeToggle()
  {
    Managers.Sound.PlayButtonClick();
    GetObject((int)GameObjects.CharacterEnhanceContentObject).gameObject.SetActive(false);
    GetObject((int)GameObjects.CharacterUpgradeContentObject).gameObject.SetActive(true);
    Refresh();
  }
}
