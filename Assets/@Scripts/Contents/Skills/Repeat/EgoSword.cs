using System.Collections;
using UnityEngine;

using static Define;

public class EgoSword : RepeatSkill
{
  private enum ESwingType { First, Second, Third, Fourth }
  
  [SerializeField] private ParticleSystem[] swingParticles;

  private float _radian;
  private int _attackCount = 0;
  
  private void Awake()
  {
    SkillType = ESkillType.EgoSword;
  }

  private IEnumerator CoSwingSword()
  {
    if (Managers.Game.Player != null)
    {
      Vector3 dir = Managers.Game.Player.PlayerDirection;
      _attackCount++;
      Shoot(dir);
    }

    yield return null;
  }
  
  private void Shoot(Vector3 dir)
  {
    string prefabName = SkillData.prefabLabel;
    Vector3 startPos = Managers.Game.Player.PlayerCenterPos;

    for (int i = 0; i < SkillData.numProjectiles; i++)
    {
      float angle = SkillData.angleBetweenProj * (i - (SkillData.numProjectiles - 1) / 2f);
      Vector3 res = Quaternion.AngleAxis(angle, Vector3.forward) * dir;
      GenerateProjectile(Managers.Game.Player, prefabName, startPos, res.normalized, Vector3.zero, this);
    }
  }
  private void OnTriggerEnter2D(Collider2D other)
  {
    if (this.IsLearnedSkill == false) return;

    CreatureController creature = other.gameObject.GetComponent<CreatureController>();
    if (creature != null && creature.IsMonster())
      creature.OnDamaged(Managers.Game.Player, this);
  }

  protected override void DoSkillJob()
  {
    StartCoroutine(CoSwingSword());
  }
}
