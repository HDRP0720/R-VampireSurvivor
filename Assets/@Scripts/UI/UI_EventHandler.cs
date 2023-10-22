using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_EventHandler : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
{
  private bool _isPressed = false;
  
  public Action OnClickHandler = null;
  public Action OnPressedHandler = null;
  public Action OnPointerDownHandler = null;
  public Action OnPointerUpHandler = null;
  public Action<BaseEventData> OnDragHandler = null;
  public Action<BaseEventData> OnBeginDragHandler = null;
  public Action<BaseEventData> OnEndDragHandler = null;

  private void Update()
  {
    if(_isPressed)
      OnPressedHandler?.Invoke();
  }

  public void OnPointerClick(PointerEventData eventData)
  {
    OnClickHandler?.Invoke();
  }

  public void OnPointerDown(PointerEventData eventData)
  {
    _isPressed = true;
    OnPointerDownHandler?.Invoke();
  }

  public void OnPointerUp(PointerEventData eventData)
  {
    _isPressed = true;
    OnPointerUpHandler?.Invoke();
  }

  public void OnDrag(PointerEventData eventData)
  {
    _isPressed = true;
    OnDragHandler?.Invoke(eventData);
  }

  public void OnBeginDrag(PointerEventData eventData)
  {
    OnBeginDragHandler?.Invoke(eventData);
  }

  public void OnEndDrag(PointerEventData eventData)
  {
    OnEndDragHandler?.Invoke(eventData);
  }
}
