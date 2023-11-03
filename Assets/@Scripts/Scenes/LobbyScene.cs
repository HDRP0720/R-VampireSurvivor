using UnityEngine;

using static Define;

public class LobbyScene : BaseScene
{
  protected override void Init()
  {
    base.Init();

    SceneType = EScene.LobbyScene;

    //TitleUI
    Managers.UI.ShowSceneUI<UI_LobbyScene>();
    Screen.sleepTimeout = SleepTimeout.SystemSetting;

    Managers.Sound.Play(ESound.BGM, "Bgm_Lobby");
  }
  
  public override void Clear() {}
}
