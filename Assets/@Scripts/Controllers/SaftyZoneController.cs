using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaftyZoneController : BaseController
{
  private Coroutine _coDotDamage;
  
  public override bool Init()
  {
    base.Init();
    return true;
  }

  private void OnTriggerEnter2D(Collider2D other)
  {
    PlayerController player = other.GetComponent<PlayerController>();
    if (player.IsValid() == false) return;
    
    player.OnSafetyZoneEnter(this);
    
    if (_coDotDamage != null)
    {
      StopCoroutine(_coDotDamage);
      _coDotDamage = null;
    }
  }
  private void OnTriggerExit2D(Collider2D other)
  {
    PlayerController player = other.GetComponent<PlayerController>();
    if (player.IsValid() == false) return;
    
    player.OnSafetyZoneExit(this);
    
    if (_coDotDamage == null)
      _coDotDamage = StartCoroutine(CoStartDotDamage(player));
  }
  
  private IEnumerator CoStartDotDamage(PlayerController target)
  {
    while (true)
    {
      yield return new WaitForSeconds(1f);
      target.OnSafetyZoneExit(this);
    }
  }
}
