using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using DG.Tweening;

public class UI_Joystick : UI_Scene
{
  enum GameObjects
  {
    JoystickBG,
    Handler,
  }
  
  private GameObject _joystickBG;
  private GameObject _handler;
  private Vector2 _moveDir;
  private float _joystickRadius;
  private Vector2 _joystickOriginalPos;
  private Vector2 _joystickTouchPos;

  private void OnDestroy()
  {
    Managers.UI.OnTimeScaleChanged -= HandleOnTimeScaleChanged;
  }

  protected override bool Init()
  {
    if (base.Init() == false) return false;

    Managers.UI.OnTimeScaleChanged += HandleOnTimeScaleChanged;

    BindObject(typeof(GameObjects));
    _handler = GetObject((int)GameObjects.Handler);

    _joystickBG = GetObject((int)GameObjects.JoystickBG);
    _joystickOriginalPos = _joystickBG.transform.position;
    _joystickRadius = _joystickBG.GetComponent<RectTransform>().sizeDelta.y / 5;
    gameObject.BindEvent(OnPointerDown, null, type: Define.UIEvent.PointerDown);
    gameObject.BindEvent(OnPointerUp, null,  type: Define.UIEvent.PointerUp);
    gameObject.BindEvent(null, OnDrag, type: Define.UIEvent.Drag);

    SetActiveJoystick(false);

    return true;
  }
  
  private void SetActiveJoystick(bool isActive)
  {
    if (isActive == true)
    {
      _handler.GetComponent<Image>().DOFade(1, 0.5f);
      _joystickBG.GetComponent<Image>().DOFade(1, 0.5f);
    }
    else
    {
      _handler.GetComponent<Image>().DOFade(0, 0.5f);
      _joystickBG.GetComponent<Image>().DOFade(0, 0.5f);
    }
  }

  private void HandleOnTimeScaleChanged(int timeScale)
  {
    if (timeScale == 1)
    { 
      gameObject.SetActive(true);
      OnPointerUp();
    }
    else
      gameObject.SetActive(false);
  }

  private void OnPointerUp()
  {
    _moveDir = Vector2.zero;
    _handler.transform.position = _joystickOriginalPos;
    _joystickBG.transform.position = _joystickOriginalPos;
    Managers.Game.MoveDir = _moveDir;
    SetActiveJoystick(false);
  }
  private void OnPointerDown()
  {
    SetActiveJoystick(true);

    _joystickTouchPos = Input.mousePosition;

    if (Managers.Game.JoystickType == Define.EJoystickType.Flexible)
    {
      _handler.transform.position = Input.mousePosition;
      _joystickBG.transform.position = Input.mousePosition;
    }
  }
  private void OnDrag(BaseEventData baseEventData)
  {
    PointerEventData pointerEventData = baseEventData as PointerEventData;
    Vector2 dragPos = pointerEventData.position;

    _moveDir = Managers.Game.JoystickType == Define.EJoystickType.Fixed
      ? (dragPos - _joystickOriginalPos).normalized
      : (dragPos - _joystickTouchPos).normalized;
  
    float joystickDist = (dragPos - _joystickOriginalPos).sqrMagnitude;

    Vector3 newPos;
    if (joystickDist < _joystickRadius)
    {
      newPos = _joystickTouchPos + _moveDir * joystickDist;
    }
    else
    {
      newPos = Managers.Game.JoystickType == Define.EJoystickType.Fixed
        ? _joystickOriginalPos + _moveDir * _joystickRadius
        : _joystickTouchPos + _moveDir * _joystickRadius;
    }

    _handler.transform.position = newPos;
    Managers.Game.MoveDir = _moveDir;
  }
}
