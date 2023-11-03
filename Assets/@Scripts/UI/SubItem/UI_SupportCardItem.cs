using UnityEngine;
using Unity.VisualScripting;

using Data;
using static Define;

public class UI_SupportCardItem : UI_Base
{
  #region UI 기능 리스트
  // 정보 갱신
  // CardNameText : 서포트 스킬 이름
  // SupportSkillImage : 서포트 스킬 아이콘
  // TargetDescriptionText : 서포트 스킬 설명
  // SoulValueText : 서포트 스킬 코스트
  // SoldOutObject : 서포트 카드 구매 완료시 활성화 
  // LockToggle : 토글이 활성화 되었다면 서포트 스킬이 변경되지 않음(잠금을 해제 할때까지 유지)

  // 로컬라이징
  // LockToggleText : 잠금
  #endregion

  #region Enum For Binding UI Automatically
  enum GameObjects
  {
    SoldOutObject,
  }
  enum Texts
  {
    CardNameText,
    SoulValueText,
    LockToggleText,
    SkillDescriptionText,
  }
  enum Images
  {
    SupportSkillImage,
    SupportSkillCardBackgroundImage,
    SupportCardTitleImage,
  }
  enum Toggles
  {
    LockToggle,
  }
  #endregion
  
  private SupportSkillData _supportSkilllData;
  
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
    BindToggle(typeof(Toggles));

    GetToggle((int)Toggles.LockToggle).gameObject.BindEvent(OnClickLockToggle);
    gameObject.BindEvent(OnClickBuy);
    GetToggle((int)Toggles.LockToggle).GetOrAddComponent<UI_ButtonAnimation>();

    return true;
  }
  
  public void SetInfo(SupportSkillData supportSkill)
  {
    transform.localScale = Vector3.one;
    _supportSkilllData = supportSkill;
    GetObject((int)GameObjects.SoldOutObject).SetActive(false);

    Refresh();
  }

  private void Refresh() { }

  private void OnClickLockToggle()
  {
    Managers.Sound.PlayButtonClick();
    
    if (_supportSkilllData.isPurchased) return;

    if (GetToggle((int)Toggles.LockToggle).isOn == true)
    {
      _supportSkilllData.isLocked = true;
      Managers.Game.Player.Skills.LockedSupportSkills.Add(_supportSkilllData);
    }
    else
    {
      _supportSkilllData.isLocked = false;
      Managers.Game.Player.Skills.LockedSupportSkills.Remove(_supportSkilllData);
    }
  }
  private void OnClickBuy()
  {
    if (GetObject((int)GameObjects.SoldOutObject).activeInHierarchy == true) return;
   
    if (Managers.Game.Player.SoulCount >= _supportSkilllData.price)
    {
      Managers.Game.Player.SoulCount -= _supportSkilllData.price;
      
      if(Managers.Game.Player.Skills.LockedSupportSkills.Contains(_supportSkilllData))
        Managers.Game.Player.Skills.LockedSupportSkills.Remove(_supportSkilllData);

      Managers.Game.Player.Skills.AddSupportSkill(_supportSkilllData);
      GetObject((int)GameObjects.SoldOutObject).SetActive(true);
      
      //구매완료
      GetObject((int)GameObjects.SoldOutObject).SetActive(true);
    }
  }
}
