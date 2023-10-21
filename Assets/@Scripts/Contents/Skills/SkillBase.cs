using System.Collections;
using UnityEngine;

public class SkillBase : BaseController
{
  private Coroutine _coDestroy;
  
  // Property
  public CreatureController Owner { get; set; }
  public Define.ESkillType SkillType { get; set; } = Define.ESkillType.None;
  public Data.SkillData SkillData { get; protected set; }
  public int SkillLevel { get; set; } = 0;
  public bool IsLearnedSkill { get => SkillLevel > 0; }
  public int Damage { get; set; } = 100;
  
  // Constructor
  public SkillBase(Define.ESkillType skillType)
  {
    SkillType = skillType;
  }
  
  public virtual void ActivateSkill() { }
  protected virtual void GenerateProjectile(int templateID, CreatureController owner, Vector3 startPos, Vector3 dir, Vector3 targetPos)
  {
    ProjectileController pc = Managers.Object.Spawn<ProjectileController>(startPos, templateID);
    pc.SetInfo(templateID, owner, dir);
  }

  public void StartDestroy(float delaySeconds)
  {
    StopDestroy();
    _coDestroy = StartCoroutine(CoDestroy(delaySeconds));
  }

  public void StopDestroy()
  {
    if (_coDestroy != null)
    {
      StopCoroutine(_coDestroy);
      _coDestroy = null;
    }
  }

  private IEnumerator CoDestroy(float delaySeconds)
  {
    yield return new WaitForSeconds(delaySeconds);
    
    if(this.IsValid())
      Managers.Object.Despawn(this);
  }
}
