using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_SkillSlotItem : UI_Base
{
  #region Enums For Binding UI Automatically
  enum SkillLevelObjects
  {
    SkillLevel_1,
    SkillLevel_2,
    SkillLevel_3,
    SkillLevel_4,
    SkillLevel_5,
    SkillLevel_6,
  }
  enum Texts
  {
    SkillDescriptionText
  }
  enum Images
  {
    BattleSkillImage,
  }
  #endregion
  
  private string _iconLabel;

  private void Awake()
  {
    Init();
  }

  protected override bool Init()
  {
    if (base.Init() == false) return false;
    
    BindObject(typeof(SkillLevelObjects));
    BindImage(typeof(Images));
    
    gameObject.GetComponent<RectTransform>().localScale = Vector3.one;

    return true;
  }
  
  public void SetUI(string iconLabel, int skillLevel = 1)
  {
    GetImage((int)Images.BattleSkillImage).sprite = Managers.Resource.Load<Sprite>(iconLabel);

    for (int i = 0; i < 6; i++)
      GetObject(i).SetActive(false);

    for (int i = 0; i < skillLevel; i++)
      GetObject(i).SetActive(true);
  }
}
