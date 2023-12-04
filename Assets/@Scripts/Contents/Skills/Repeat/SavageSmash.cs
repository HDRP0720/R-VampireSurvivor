using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Define;

public class SavageSmash : RepeatSkill
{
  [SerializeField] private ParticleSystem[] _swingParticle;
  [SerializeField] private ParticleSystem[] _swingParticleFinal;
  
  private float _radian;
  
  private void Awake()
  {
    SkillType = ESkillType.SavageSmash;
  }
  private void OnTriggerEnter2D(Collider2D collision)
  {
    CreatureController creature = collision.transform.GetComponent<CreatureController>();
    if (creature != null && creature.IsMonster())
      creature.OnDamaged(Managers.Game.Player, this);
  }
  
  public override void OnLevelUp()
  {
    base.OnLevelUp();
    transform.localScale = Vector3.one * SkillData.scaleMultiplier;
  }

  protected override void OnChangedSkillData()
  {
    transform.localScale = Vector3.one * SkillData.scaleMultiplier;
  }
  
  private IEnumerator CoSmashAxe()
  {
    if (Level == 6)
    {
      for (int i = 0; i < SkillData.numProjectiles; i++)
      {
        _swingParticleFinal[i].gameObject.SetActive(true);
        SetParticles(i);
      }
    }
    else 
    { 
      for (int i = 0; i < SkillData.numProjectiles; i++)
      {
        _swingParticle[i].gameObject.SetActive(true);
        SetParticles(i);
      }
    }
    yield return new WaitForSeconds(SkillData.coolTime);
  }
  private void SetParticles(int swingType)
  {
    Vector3 tempAngle = Managers.Game.Player.indicator.transform.eulerAngles;
    transform.localEulerAngles = tempAngle;
    transform.position = Managers.Game.Player.PlayerCenterPos;

    _radian = Mathf.Deg2Rad * tempAngle.z * -1;

    if (Level == 6)
    {
      var main = _swingParticleFinal[swingType].main;
      main.startRotation = _radian;
    }
    else
    {
      var main = _swingParticle[swingType].main;
      main.startRotation = _radian;
    }
  }
  
  protected override void DoSkillJob()
  {
    StartCoroutine(CoSmashAxe());
  }
}
