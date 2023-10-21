using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBook : MonoBehaviour
{
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
    else
    {
      
    }

    return null;
  }
}
