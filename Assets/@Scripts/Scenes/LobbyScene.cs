using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyScene : BaseScene
{
  protected override void Init()
  {
    base.Init();

    SceneType = Define.EScene.LobbyScene;

    //TitleUI
    Managers.UI.ShowSceneUI<UI_LobbyScene>();
    Screen.sleepTimeout = SleepTimeout.SystemSetting;

    Managers.Sound.Play(Define.ESound.BGM, "Bgm_Lobby");
  }
  
  public override void Clear() {}
 
}
