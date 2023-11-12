using UnityEngine;
using UnityEngine.UI.Extensions;

using DG.Tweening;

public class UI_ParticlePlay : MonoBehaviour
{
  [SerializeField] private UIParticleSystem _particleSystem;
  
  public void OnEnable()
  {
    _particleSystem.DOPlay();
  }
}
