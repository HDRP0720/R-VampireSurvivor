using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using Unity.VisualScripting;
using UnityEngine;

public class EgoSword : RepeatSkill
{
  protected enum ESwingType { First, Second, Third, Fourth }
  
  [SerializeField] private ParticleSystem[] swingParticles;

  public override bool Init()
  {
    base.Init();

    return true;
  }

  protected override IEnumerator CoStartSkill()
  {
    WaitForSeconds wait = new WaitForSeconds(CoolTime);
    while (true)
    {
      SetParticles(ESwingType.First);
      swingParticles[(int)ESwingType.First].gameObject.SetActive(true);
      yield return new WaitForSeconds(swingParticles[(int)ESwingType.First].main.duration);
      
      SetParticles(ESwingType.Second);
      swingParticles[(int)ESwingType.Second].gameObject.SetActive(true);
      yield return new WaitForSeconds(swingParticles[(int)ESwingType.Second].main.duration);
      
      SetParticles(ESwingType.Third);
      swingParticles[(int)ESwingType.Third].gameObject.SetActive(true);
      yield return new WaitForSeconds(swingParticles[(int)ESwingType.Third].main.duration);
      
      SetParticles(ESwingType.Fourth);
      swingParticles[(int)ESwingType.Fourth].gameObject.SetActive(true);
      yield return new WaitForSeconds(swingParticles[(int)ESwingType.Fourth].main.duration);

      yield return wait;
    }
  }
  private void SetParticles(ESwingType swingType)
  {
    if (Managers.Game.Player == null) return;

    Vector3 tempAngle = Managers.Game.Player.Indicator.transform.eulerAngles;
    transform.localEulerAngles = tempAngle;
    transform.position = Managers.Game.Player.transform.position;

    float radian = Mathf.Deg2Rad * tempAngle.z * -1;
    var main = swingParticles[(int)swingType].main;
    main.startRotation = radian;
  }

  private void OnTriggerEnter2D(Collider2D other)
  {
    MonsterController mc = other.transform.GetComponent<MonsterController>();
    if (mc.IsValid() == false) return;
    
    mc.OnDamaged(Owner, Damage);
  }

  protected override void DoSkillJob() { }
}
