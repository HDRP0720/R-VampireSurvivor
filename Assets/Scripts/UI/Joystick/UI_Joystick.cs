using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Joystick : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
  [SerializeField] private Image background;
  [SerializeField] private Image handler;

  private float _joystickRadius;
  private Vector2 _touchPosition;
  private Vector2 _moveDir;

  private void Start()
  {
    _joystickRadius = background.gameObject.GetComponent<RectTransform>().sizeDelta.y / 2;
  }

  public void OnPointerClick(PointerEventData eventData)
  {

  }

  public void OnPointerDown(PointerEventData eventData)
  {
    background.transform.position = eventData.position;
    handler.transform.position = eventData.position;
    _touchPosition = eventData.position;
  }

  public void OnPointerUp(PointerEventData eventData)
  {
    handler.transform.position = _touchPosition;
    _moveDir = Vector2.zero;
  }

  public void OnDrag(PointerEventData eventData)
  {
    Vector2 touchDir = eventData.position - _touchPosition;
    float moveDist = Mathf.Min(touchDir.magnitude, _joystickRadius);
    _moveDir = touchDir.normalized;
    Vector2 newPosition = _touchPosition + _moveDir * moveDist;
    handler.transform.position = newPosition;
  }
}
