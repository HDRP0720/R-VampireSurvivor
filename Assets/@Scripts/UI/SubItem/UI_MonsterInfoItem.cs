using System.Collections;
using System.Collections.Generic;
using Data;
using UnityEngine;

public class UI_MonsterInfoItem : UI_Base
{
  #region UI Feature List
  // 정보 갱신
  // MonsterImage : 몬스터 이미지
  // MonsterLevelValueText : 몬스터 레벨 ( "Lv." + 레벨)
  // 추후 후광 효과같은 그래픽 강조 포인트가 추가될 수 있음
  #endregion

  #region Enum For Binding UI Automatically
  enum Buttons
  {
    MonsterInfoButton
  }
  enum Texts
  {
    MonsterLevelValueText,
  }
  enum Images
  {
    MonsterImage,
  }
  #endregion
  
  private CreatureData _creature;
  private Transform _makeSubItemParents;
  private int _level;
  
  private void Awake()
  {
    Init();
  }

  protected override bool Init()
  {
    if (base.Init() == false) return false;

    BindButton(typeof(Buttons));
    BindText(typeof(Texts));
    BindImage(typeof(Images));

    GetButton((int)Buttons.MonsterInfoButton).gameObject.BindEvent(OnClickMonsterInfoButton);
    
    return true;
  }
  
  public void SetInfo(int monsterId, int level, Transform makeSubItemParents)
  {
    _makeSubItemParents = makeSubItemParents;
    transform.localScale = Vector3.one;

    if (Managers.Data.CreatureDic.TryGetValue(monsterId, out _creature))
    {
      _creature = Managers.Data.CreatureDic[monsterId];
      _level = level;
    }

    Refresh();
  }
  
  private void Refresh()
  {
    if (_init == false) return;
   
    if (_creature == null)
    {
      gameObject.SetActive(false);
      return;
    }

    GetText((int)Texts.MonsterLevelValueText).text = $"Lv. {_level}";
    GetImage((int)Images.MonsterImage).sprite = Managers.Resource.Load<Sprite>(_creature.iconLabel);
  }
  
  // 툴팁 호출
  private void OnClickMonsterInfoButton()
  {
    Managers.Sound.PlayButtonClick();
    // UI_ToolTipItem 프리팹 생성
    UI_ToolTipItem item = Managers.UI.MakeSubItem<UI_ToolTipItem>(_makeSubItemParents);
    item.transform.localScale = Vector3.one;
    RectTransform targetPos = this.gameObject.GetComponent<RectTransform>();
    RectTransform parentsCanvas = _makeSubItemParents.gameObject.GetComponent<RectTransform>();
    item.SetInfo(_creature, targetPos, parentsCanvas);
    item.transform.SetAsLastSibling();
  }
}
