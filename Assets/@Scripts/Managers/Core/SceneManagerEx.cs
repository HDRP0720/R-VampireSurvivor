using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManagerEx
{
  public BaseScene CurrentScene => GameObject.FindObjectOfType<BaseScene>();

  public void LoadScene(Define.EScene type, Transform parent = null)
  {
    switch (CurrentScene.SceneType)
    {
      case Define.EScene.TitleScene:
        Managers.Clear();
        
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
