using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Define;

public class ElectronicField : RepeatSkill
{
  [SerializeField] private GameObject _normalEffect;
  [SerializeField] private GameObject _finalEffect;
  
  private HashSet<CreatureController> _targets = new HashSet<CreatureController>();
  private Coroutine _coroutine;
  
  private void Awake()
  {
    SkillType = ESkillType.ElectronicField;
    gameObject.SetActive(false);
  }
  private void Update()
  {
    transform.position = Managers.Game.Player.PlayerCenterPos; 
  }
  
  public override void ActivateSkill()
  {
    base.ActivateSkill();
    gameObject.SetActive(true);

    _normalEffect.SetActive(true);
    _finalEffect.SetActive(false);

    transform.localScale = Vector3.one * SkillData.scaleMultiplier;
  }
  
  public override void OnLevelUp()
  {
    base.OnLevelUp();

    if (Level == 6)
    {
      _normalEffect.SetActive(false);
      _finalEffect.SetActive(true);
    }
    transform.localScale = Vector3.one * SkillData.scaleMultiplier;
  }

  protected override void OnChangedSkillData()
  {
    transform.localScale = Vector3.one * SkillData.scaleMultiplier;
  }
  
  private void OnTriggerEnter2D(Collider2D collision)
  {
    CreatureController target = collision.transform.GetComponent<CreatureController>();

    if (target.IsValid() == false) return;

    if (target?.IsMonster() == false) return;

    _targets.Add(target);
    
    if (target != null) 
      target.OnDamaged(Managers.Game.Player, this);
    
    if (_coroutine == null)
      _coroutine = StartCoroutine(CoStartDotDamage());
  }
  private void OnTriggerExit2D(Collider2D collision)
  {
    CreatureController target = collision.transform.GetComponent<CreatureController>();
    if (target.IsValid() == false) return;

    _targets.Remove(target);
    if (_targets.Count == 0 && _coroutine != null)
    {
      StopCoroutine(_coroutine);
      _coroutine = null;
    }
  }
  private IEnumerator CoStartDotDamage()
  {
    while (true)
    {
      yield return new WaitForSeconds(1f);
      List<CreatureController> list = _targets.ToList();

      foreach (CreatureController target in list)
      {
        if (target.IsValid() == false || target.gameObject.IsValid() == false)
        {
          _targets.Remove(target);
          continue;
        }
        target.OnDamaged(Managers.Game.Player, this);
      }
    }
  }
  
  protected override void DoSkillJob() { }
}
