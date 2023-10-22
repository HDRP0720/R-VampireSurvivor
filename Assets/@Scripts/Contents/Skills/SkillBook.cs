using System.Collections.Generic;
using UnityEngine;

public class SkillBook : MonoBehaviour
{
  private int _sequenceIndex = 0;
  private bool _isStoppedSkill = false;
  
  // Properties
  public List<SkillBase> Skills { get; } = new List<SkillBase>();
  public List<SkillBase> RepeatedSkills { get; } = new List<SkillBase>();
  public List<SequenceSkill> SequenceSkills { get; } = new List<SequenceSkill>();

  public T AddSkill<T>(Vector3 position, Transform parent = null) where T : SkillBase
  {
    System.Type type = typeof(T);
    if (type == typeof(EgoSword))
    {
      var egoSword = Managers.Object.Spawn<EgoSword>(position, Define.EGO_SWORD_ID);
      egoSword.transform.SetParent(parent);
      egoSword.ActivateSkill();
      
      Skills.Add(egoSword);
      RepeatedSkills.Add(egoSword);

      return egoSword as T;
    }
    else if (type == typeof(FireballSkill))
    {
      var fireball = Managers.Object.Spawn<FireballSkill>(position, Define.FIRE_BALL_ID);
      fireball.transform.SetParent(parent);
      fireball.ActivateSkill();
      
      Skills.Add(fireball);
      RepeatedSkills.Add(fireball);

      return fireball as T;
    }
    else if (type.IsSubclassOf(typeof(SequenceSkill)))
    {
      var skill = gameObject.GetOrAddComponent<T>();
      
      Skills.Add(skill);
      SequenceSkills.Add(skill as SequenceSkill);

      return skill as T;
    }

    return null;
  }

  public void StartNextSequenceSkill()
  {
    if (_isStoppedSkill) return;

    if (SequenceSkills.Count == 0) return;
    
    SequenceSkills[_sequenceIndex].DoSkill(OnFinishedSequenceSkill);
  }
  private void OnFinishedSequenceSkill()
  {
    _sequenceIndex = (_sequenceIndex + 1) % SequenceSkills.Count;
    StartNextSequenceSkill();
  }
  
  public void StopSkills()
  {
    _isStoppedSkill = true;
    foreach (var skill in RepeatedSkills)
    {
      skill.StopAllCoroutines();
    }
  }
}
