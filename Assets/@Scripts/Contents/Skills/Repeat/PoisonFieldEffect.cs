using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using DG.Tweening;

public class PoisonFieldEffect : MonoBehaviour
{
  private CreatureController _owner;
  private SkillBase _skill;
  private HashSet<CreatureController> _targets = new HashSet<CreatureController>();
  private Coroutine _coDotDamage;
  private Coroutine _coApplyDamage;
  
  public void OnDisable()
  {
    StopApplyDamage();
  }
  public void OnTriggerEnter2D(Collider2D collision)
  {
    MonsterController target = collision.transform.GetComponent<MonsterController>();
    if (target.IsValid() == false) return;
    
    _targets.Add(target);
  }
  public void OnTriggerExit2D(Collider2D collision)
  {
    MonsterController target = collision.transform.GetComponent<MonsterController>();
    if (target == null) return;
 
    _targets.Remove(target);
  }
  
  public void SetInfo(CreatureController owner, SkillBase skill)
  {
    _owner = owner;
    _skill = skill;
    _targets.Clear();
    
    PlayEnableAnimation(() =>
    {
      if (gameObject.activeInHierarchy)
        StartCoroutine(CoDestroy(gameObject, _skill.SkillData.duration, true));
      StartApplyDamage();
    });
  }
  private void PlayEnableAnimation(Action action)
  {
    transform.localScale = Vector3.zero;
    Vector3 targetScale = Vector3.one * _skill.SkillData.scaleMultiplier;
    transform.DOScale(targetScale, 0.5f).OnComplete(() => action.Invoke()); ;
  }
  private void StartApplyDamage()
  {
    StopApplyDamage();
    _coApplyDamage = StartCoroutine(CoApplyDamage());
  }
  private void StopApplyDamage()
  {
    if (_coApplyDamage != null)
      StopCoroutine(_coApplyDamage);
    _coApplyDamage = null;
  }
  private IEnumerator CoApplyDamage()
  {
    while (true)
    {
      var targets = _targets.ToList();
      foreach (var target in targets)
      {
        if (target.IsValid() == false)
        {
          _targets.Remove(target);
          continue;
        }
        target.OnDamaged(_owner, _skill);
      }
      yield return new WaitForSeconds(1.0f);
    }
  }
  private IEnumerator CoDestroy(GameObject go, float time, bool isTween = true)
  {
    yield return new WaitForSeconds(time);
    transform.localScale = Vector3.one * _skill.SkillData.scaleMultiplier;
    transform.DOScale(0, 0.5f).OnComplete(() =>
    {
      Managers.Resource.Destroy(go);
      if (_coDotDamage != null)
        StopCoroutine(_coDotDamage);
      _coDotDamage = null;
    });
  }
}
