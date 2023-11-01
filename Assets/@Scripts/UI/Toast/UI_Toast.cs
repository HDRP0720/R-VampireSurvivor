using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Toast : UI_Base
{
  #region Enum Bind
  enum Images
  {
    BackgroundImage
  }
  enum Texts
  {
    ToastMessageValueText
  }
  #endregion
  
  private void Awake()
  {
    Init();
  }
  private void OnEnable()
  {
    PopupOpenAnimation(gameObject);
  }

  protected override bool Init()
  {
    if (base.Init() == false)
      return false;
   
    BindImage(typeof(Images));
    BindText(typeof(Texts));

    Refresh();
    return true;
  }
  
  public void SetInfo(string msg)
  {
    // 메시지 변경
    transform.localScale = Vector3.one;
    GetText((int)Texts.ToastMessageValueText).text = msg;
    Refresh();
  }
  
  private void Refresh() {}
}
