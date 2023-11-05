using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class SceneChangeAnimation_Out : UI_Popup
{
  private Animator _anim;
  private Action _action;
  private EScene _prevScene;
  
  private void Awake()
  {
    _anim= GetComponent<Animator>();   
  }
  
  public void SetInfo(EScene prevScene, Action callback)
  {
    transform.localScale = Vector3.one;
    _action = callback;
    _prevScene = prevScene;
  }
  
  public void OnAnimationComplete()
  {
    _action.Invoke();
  }
}
