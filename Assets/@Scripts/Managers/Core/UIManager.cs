using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class UIManager
{
  private int _order = 10;
  private int _toastOrder = 500;
  
  private UI_Scene _sceneUI = null;
  private Stack<UI_Popup> _popupStack = new Stack<UI_Popup>();
  private Stack<UI_Toast> _toastStack = new Stack<UI_Toast>();

  #region Properties
  public UI_Scene SceneUI => _sceneUI;
  private GameObject Root
  {
    get
    {
      GameObject root = GameObject.Find("UI_Root");
      if (root == null)
        root = new GameObject { name = "UI_Root" };
      return root;
    }
  }
  #endregion
  
  // Action
  public event Action<int> OnTimeScaleChanged;
  
  public void SetCanvas(GameObject go, bool sort = true, int sortOrder = 0, bool isToast = false)
  {
    Canvas canvas = Utils.GetOrAddComponent<Canvas>(go);
    if (canvas == null)
    {
      canvas.renderMode = RenderMode.ScreenSpaceOverlay;
      canvas.overrideSorting = true;
    }

    CanvasScaler cs = go.GetOrAddComponent<CanvasScaler>();
    if (cs != null)
    {
      cs.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
      cs.referenceResolution = new Vector2(1080, 1920);
    }

    go.GetOrAddComponent<GraphicRaycaster>();

    if (sort)
    {
      canvas.sortingOrder = _order;
      _order++;
    }
    else
    {
      canvas.sortingOrder = sortOrder;
    }

    if (isToast)
    {
      _toastOrder++;
      canvas.sortingOrder = _toastOrder;
    }

  }

  public T GetSceneUI<T>() where T : UI_Base
  {
    return _sceneUI as T;
  }
  public T ShowSceneUI<T>(string name = null) where T : UI_Scene
  {
    if (string.IsNullOrEmpty(name)) name = typeof(T).Name;

    GameObject go = Managers.Resource.Instantiate($"{name}");
    T sceneUI = Utils.GetOrAddComponent<T>(go);
    _sceneUI = sceneUI;
    
    go.transform.SetParent(Root.transform);
    
    return sceneUI;
  }

  public T ShowPopupUI<T>(string name = null) where T : UI_Popup
  {
    if (string.IsNullOrEmpty(name)) name = typeof(T).Name;

    GameObject go = Managers.Resource.Instantiate($"{name}");
    T popup = Utils.GetOrAddComponent<T>(go);
    _popupStack.Push(popup);

    go.transform.SetParent(Root.transform);

    RefreshTimeScale();

    return popup;
  }
  
  public T MakeSubItem<T>(Transform parent = null, string name = null, bool pooling = true) where T : UI_Base
  {
    if (string.IsNullOrEmpty(name))
      name = typeof(T).Name;

    GameObject go = Managers.Resource.Instantiate($"{name}", parent, pooling);
    go.transform.SetParent(parent);
    return Utils.GetOrAddComponent<T>(go);
  }
  
  public void ClosePopupUI(UI_Popup popup)
  {
    if (_popupStack.Count == 0) return;

    if (_popupStack.Peek() != popup)
    {
      Debug.Log("Close Popup Failed!");
      return;
    }
    Managers.Sound.PlayPopupClose();
    ClosePopupUI();
  }
  public void ClosePopupUI()
  {
    if (_popupStack.Count == 0)
        return;

    UI_Popup popup = _popupStack.Pop();
    Managers.Resource.Destroy(popup.gameObject);
    popup = null;
    _order--;
    RefreshTimeScale();
  }
  
  public void CloseAllPopupUI()
  {
    while (_popupStack.Count > 0)
      ClosePopupUI();
  }

  public void Clear()
  {
    CloseAllPopupUI();
    Time.timeScale = 1;
    _sceneUI = null;
  }

  private void RefreshTimeScale()
  {
    if (SceneManager.GetActiveScene().name != Define.EScene.GameScene.ToString())
    {
      Time.timeScale = 1;
      return;
    }
    
    if (_popupStack.Count > 0 || IsActiveSoulShop)
      Time.timeScale = 0;
    else
      Time.timeScale = 1;
    
    DOTween.timeScale = 1;
    OnTimeScaleChanged?.Invoke((int)Time.timeScale);
  }
  
  // TODO : temporal code
  #region Temporal code
  bool _isActiveSoulShop = false;
  public bool IsActiveSoulShop 
  {
    get => _isActiveSoulShop;
    set
    {
      _isActiveSoulShop = value;

      if (_isActiveSoulShop == true)
        Time.timeScale = 0;
      else
        Time.timeScale = 1;

      DOTween.timeScale = 1;
      OnTimeScaleChanged?.Invoke((int)Time.timeScale);
    }
  }

  #endregion
}
