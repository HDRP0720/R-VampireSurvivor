using System;
using UnityEngine;

public class AnimationEventDetector : MonoBehaviour
{
  public event Action OnEvent;
    
  public void OnAnimEvent()
  {
    OnEvent?.Invoke();  
  }
}
