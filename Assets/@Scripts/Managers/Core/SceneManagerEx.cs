using System;

using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerEx
{
  public BaseScene CurrentScene => GameObject.FindObjectOfType<BaseScene>();

  public void LoadScene(Define.EScene type, Transform parent = null)
  {
    switch (CurrentScene.SceneType)
    {
      case Define.EScene.TitleScene:
        Managers.Clear();
        SceneManager.LoadScene(GetSceneName(type));
        break;
      case Define.EScene.LobbyScene:
        SceneChangeAnimation_In anim2 = Managers.Resource.Instantiate("SceneChangeAnimation_In").GetOrAddComponent<SceneChangeAnimation_In>();
        anim2.transform.SetParent(parent);
        Time.timeScale = 1;
        anim2.SetInfo(type, () =>
        {
          Managers.Resource.Destroy(Managers.UI.SceneUI.gameObject);
          Managers.Clear();
          SceneManager.LoadScene(GetSceneName(type));
        });
        break;
      case Define.EScene.GameScene:
        SceneChangeAnimation_In anim = Managers.Resource.Instantiate("SceneChangeAnimation_In").GetOrAddComponent<SceneChangeAnimation_In>();
        anim.transform.SetParent(parent);
        Time.timeScale = 1;
        anim.SetInfo(type, () =>
        {
          Managers.Resource.Destroy(Managers.UI.SceneUI.gameObject);
          Managers.Clear();
          SceneManager.LoadScene(GetSceneName(type));
        });
        break;
    }
  }
  private string GetSceneName(Define.EScene type)
  {
    string name = Enum.GetName(typeof(Define.EScene), type);
    return name;
  }

  public void Clear()
  {
    CurrentScene.Clear();
  }
}
