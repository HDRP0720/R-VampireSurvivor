using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ChallengePopup : UI_Popup
{
  enum Texts
  {
    UnlockInfoText,
  }

  protected override bool Init()
  {
    if (base.Init() == false) return false;

    BindText(typeof(Texts));

    Refresh();
    return true;
  }
  
  private void Refresh() { }
}
