using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class SpinShot : SequenceSkill
{
  public float spawnInterval = 0.02f;
  
  private CreatureController _owner;
  private Vector3 _dir;
  private float _spawnTimer = 0;
  private float _launchCount = 0;
  private Coroutine _coroutine;
  
  private void Awake()
  {
    SkillType = ESkillType.SpinShot;
    animationName = "Attack";
    _owner = GetComponent<CreatureController>();
  }
  
  private IEnumerator CoSkill(Action callback = null)
  {
    while (true)
    {
      _dir = Quaternion.Euler(0, 0, SkillData.rotateSpeed ) * _dir;
      _spawnTimer += Time.deltaTime;
      if (_spawnTimer < spawnInterval) continue;

      _spawnTimer = 0f;

      Vector3 startPos = _owner.CenterPosition;
      GenerateProjectile(_owner, SkillData.prefabLabel, startPos, _dir.normalized, Vector3.zero, this);
      _launchCount++;

      if (_launchCount > SkillData.numProjectiles)
      {
        _launchCount = 0;
        break;
      }

      yield return new WaitForFixedUpdate();
    }

    yield return new WaitForSeconds(SkillData.attackInterval);
    callback?.Invoke();
  }
  
  public override void DoSkill(Action callback = null)
  {
    CreatureController owner = GetComponent<CreatureController>();
    if (owner.CreatureState != ECreatureState.Skill) return;
    
    UpdateSkillData(dataId);

    _dir = Managers.Game.Player.CenterPosition - _owner.CenterPosition;
    GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    transform.GetChild(0).GetComponent<Animator>().Play(animationName);
    _coroutine = null;
    _coroutine = StartCoroutine(CoSkill(callback));
  }
}
