using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using Object = UnityEngine.Object;

public class UI_Base : MonoBehaviour
{
  private Dictionary<Type, Object[]> _objects = new Dictionary<Type, Object[]>();
  protected bool _init = false;

  private void Start()
  {
    Init();
  }
  
  protected virtual bool Init()
  {
    if (_init) return false;

    _init = true;
    
    return true;
  }

  protected void Bind<T>(Type type) where T : Object
  {
    string[] names = Enum.GetNames(type);
    Object[] objects = new Object[names.Length];
    _objects.Add(typeof(T), objects);

    for (int i = 0; i < names.Length; i++)
    {
      if (typeof(T) == typeof(GameObject))
        objects[i] = Utils.FindChild(gameObject, names[i], true);
      else
        objects[i] = Utils.FindChild<T>(gameObject, names[i], true);
      
      if(objects[i] == null)
        Debug.Log($"Failed to bind({gameObject},{names[i]})");
    }
  }
  protected void BindObject(Type type) { Bind<GameObject>(type); }
  protected void BindImage(Type type) { Bind<Image>(type); }
  protected void BindText(Type type) { Bind<TMP_Text>(type); }
  protected void BindButton(Type type) { Bind<Button>(type); }
  protected void BindToggle(Type type) { Bind<Toggle>(type); }

  private T Get<T>(int idx) where T : Object
  {
    Object[] objects = null;
    if (_objects.TryGetValue(typeof(T), out objects) == false) return null;

    return objects[idx] as T;
  }
  protected GameObject GetObject(int idx) { return Get<GameObject>(idx); }
  protected TMP_Text GetText(int idx) { return Get<TMP_Text>(idx); }
  protected Button GetButton(int idx) { return Get<Button>(idx); }
  protected Image GetImage(int idx) { return Get<Image>(idx); }
  protected Toggle GetToggle(int idx) { return Get<Toggle>(idx); }
  
  public static void BindEvent(GameObject go, Action action = null, Action<BaseEventData> dragAction = null, Define.UIEvent type = Define.UIEvent.Click)
  {
    UI_EventHandler evt = Utils.GetOrAddComponent<UI_EventHandler>(go);

    switch (type)
    {
      case Define.UIEvent.Click:
        evt.OnClickHandler -= action;
        evt.OnClickHandler += action;
        break;
      case Define.UIEvent.Pressed:
        evt.OnPressedHandler -= action;
        evt.OnPressedHandler += action;
        break;
      case Define.UIEvent.PointerDown:
        evt.OnPointerDownHandler -= action;
        evt.OnPointerDownHandler += action;
        break;
      case Define.UIEvent.PointerUp:
        evt.OnPointerUpHandler -= action;
        evt.OnPointerUpHandler += action;
        break;
      case Define.UIEvent.Drag:
        evt.OnDragHandler -= dragAction;
        evt.OnDragHandler += dragAction;
        break;
      case Define.UIEvent.BeginDrag:
        evt.OnBeginDragHandler -= dragAction;
        evt.OnBeginDragHandler += dragAction;
        break;
      case Define.UIEvent.EndDrag:
        evt.OnEndDragHandler -= dragAction;
        evt.OnEndDragHandler += dragAction;
        break;
    }
  }

  protected void PopupOpenAnimation(GameObject contentObject)
  {
    contentObject.transform.localScale = new Vector3(0.8f, 0.8f, 1);
    contentObject.transform.DOScale(1f, 0.1f).SetEase(Ease.InOutBack).SetUpdate(true);
  }
}
