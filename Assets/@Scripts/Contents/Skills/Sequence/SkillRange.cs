using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillRange : MonoBehaviour
{
  private SpriteRenderer _alertIndicator;
  private ParticleSystem _particle;
  private ParticleSystem _subParticle;
  
  private void Awake()
  {
    _alertIndicator = transform.GetChild(0).GetComponent<SpriteRenderer>();
    _particle = Utils.FindChild<ParticleSystem>(gameObject, recursive: true);
    _subParticle = _particle.transform.GetChild(0).GetComponent<ParticleSystem>();
  }
  private void OnEnable()
  {
    _alertIndicator.size = Vector2.zero;
  }
  
  public void SetInfo(Vector2 dir, Vector2 target ,float dist)
  {
    float distance = dist;
    _alertIndicator.size = new Vector2(1.3f, distance);

    Vector3 nomalDir = dir.normalized;
    float angle = Mathf.Atan2(nomalDir.y, nomalDir.x) * Mathf.Rad2Deg - 90;
    transform.rotation = Quaternion.Euler(0, 0, angle); 
  }
  
  public float SetCircleAlert(float startSize)
  {
    _particle.Play();
    var main = _particle.main;
    var main2 = _subParticle.main;
    main.startSize = startSize;
    main2.startSize = startSize;

    return _particle.main.duration;
  }
}
