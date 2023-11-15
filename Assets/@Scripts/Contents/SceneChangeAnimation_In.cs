using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static Define;

public class SceneChangeAnimation_In : UI_Popup
{
  private Animator _anim;
  private Action _action;
  private EScene _nextScene;

  private void Awake()
  {
    _anim= GetComponent<Animator>();   
  }
  
  public void SetInfo(EScene nextScene, Action callback)
  {
    transform.localScale = Vector3.one;
    _action = callback;
    _nextScene = nextScene;
    StartCoroutine(CoAnimationComplete());
  }
  private IEnumerator CoAnimationComplete()
  {
    yield return new WaitForSeconds(1f);
    _action.Invoke();
  }
}
