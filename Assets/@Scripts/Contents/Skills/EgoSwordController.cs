using System.Collections;
using System.Collections.Generic;
using Data;
using Unity.VisualScripting;
using UnityEngine;

public class EgoSwordController : SkillController
{
  protected enum ESwingType { First, Second, Third, Fourth }
  [SerializeField] private ParticleSystem[] swingParticles;

  private float coolTime = 2.0f;

  public override bool Init()
  {
    base.Init();
    
    for (int i = 0; i < swingParticles.Length; i++)
      swingParticles[i].GetComponent<Rigidbody2D>().simulated = false;

    for (int i = 0; i < swingParticles.Length; i++)
      swingParticles[i].GetOrAddComponent<EgoSwordChild>().SetInfo(Managers.Object.Player, 100);

    return true;
  }

  public void ActivateSkill()
  {
    StartCoroutine(CoSwingSword());
  }

  private IEnumerator CoSwingSword()
  {
    while (true)
    {
      yield return new WaitForSeconds(coolTime);

      SetParticles(ESwingType.First);
      swingParticles[(int)ESwingType.First].Play();
      TurnOnPhysics(ESwingType.First, true);
      yield return new WaitForSeconds(swingParticles[(int)ESwingType.First].main.duration);
      TurnOnPhysics(ESwingType.First, false);
      
      SetParticles(ESwingType.Second);
      swingParticles[(int)ESwingType.Second].Play();
      TurnOnPhysics(ESwingType.Second, true);
      yield return new WaitForSeconds(swingParticles[(int)ESwingType.Second].main.duration);
      TurnOnPhysics(ESwingType.Second, false);
      
      SetParticles(ESwingType.Third);
      swingParticles[(int)ESwingType.Third].Play();
      TurnOnPhysics(ESwingType.Third, true);
      yield return new WaitForSeconds(swingParticles[(int)ESwingType.Third].main.duration);
      TurnOnPhysics(ESwingType.Third, false);
      
      SetParticles(ESwingType.Fourth);
      swingParticles[(int)ESwingType.Fourth].Play();
      TurnOnPhysics(ESwingType.Fourth, true);
      yield return new WaitForSeconds(swingParticles[(int)ESwingType.Fourth].main.duration);
      TurnOnPhysics(ESwingType.Fourth, false);
    }
  }

  private void SetParticles(ESwingType swingType)
  {
    float z = transform.parent.transform.eulerAngles.z;
    float radian = (Mathf.PI / 180) * z * -1;
    
    var main = swingParticles[(int)swingType].main;
    main.startRotation = radian;
  }

  private void TurnOnPhysics(ESwingType swingType, bool isSimulated)
  {
    for (int i = 0; i < swingParticles.Length; i++)
      swingParticles[i].GetComponent<Rigidbody2D>().simulated = false;

    swingParticles[(int)swingType].GetComponent<Rigidbody2D>().simulated = isSimulated;
  }
}
