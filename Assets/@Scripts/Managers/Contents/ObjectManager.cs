using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager
{
  public List<GameObject> _game = new List<GameObject>();

  #region Properties
  public PlayerController Player { get; private set; }
  public HashSet<MonsterController> Monsters { get; } = new HashSet<MonsterController>();
  public HashSet<GemController> Gems { get; } = new HashSet<GemController>();
  public HashSet<SoulController> Souls { get; } = new HashSet<SoulController>();
  public HashSet<DropItemController> DropItems { get; } = new HashSet<DropItemController>();
  public HashSet<ProjectileController> Projectiles { get; } = new HashSet<ProjectileController>();
  #endregion
  


  public T Spawn<T>(Vector3 position, int templateID = 0) where T : BaseController
  {
    System.Type type = typeof(T);
    if (type == typeof(PlayerController))
    {
      // TODO: Change Slime_01.prefab to Data
      GameObject go = Managers.Resource.Instantiate("Slime_01.prefab");
      go.name = "Player";
      go.transform.position = position;

      PlayerController pc = go.GetOrAddComponent<PlayerController>();
      Player = pc;
      pc.Init();
      
      return pc as T;
    }
    else if (type == typeof(MonsterController))
    {
      string name = "";
      switch (templateID)
      {
        case Define.GOBLIN_ID:
          name = "Goblin_01";
          break;
        case Define.SNAKE_ID:
          name = "Snake_01";
          break;
        case Define.BOSS_ID:
          name = "Boss_01";
          break;
      }
      
      GameObject go = Managers.Resource.Instantiate(name + ".prefab", pooling:true);
      go.transform.position = position;

      MonsterController mc = go.GetOrAddComponent<MonsterController>();
      Monsters.Add(mc);
      mc.Init();
      
      return mc as T;
    }
    else if (type == typeof(GemController))
    {
      GameObject go = Managers.Resource.Instantiate(Define.EXP_GEM_PREFAB, pooling:true);
      go.transform.position = position;

      GemController gc = go.GetOrAddComponent<GemController>();
      Gems.Add(gc);
      gc.Init();
      
      // TODO: temporal test code be changed
      string key = Random.Range(0, 2) == 0 ? "EXPGem_01.sprite" : "EXPGem_02.sprite";
      Sprite sprite = Managers.Resource.Load<Sprite>(key);
      go.GetComponent<SpriteRenderer>().sprite = sprite;
      
      // TODO: This is temporal test code
      GameObject.Find("Grid").GetComponent<GridController>().Add(go);
      
      return gc as T;
    }
    // else if (typeof(T).IsSubclassOf(typeof(ProjectileController)))
    else if (type == typeof(ProjectileController))
    {
      GameObject go = Managers.Resource.Instantiate(Define.FIRE_PROJECTILE, pooling:true);
      go.transform.position = position;

      ProjectileController pc = go.GetOrAddComponent<ProjectileController>();
      Projectiles.Add(pc);
      pc.Init();

      return pc as T;
    }
    else if (typeof(T).IsSubclassOf(typeof(SkillBase)))
    {
      if (Managers.Data.SkillDic.TryGetValue(templateID, out Data.SkillData skillData) == false)
      {
        Debug.LogError($"ObjectManager Spawn Skill Failed {templateID}");
        return null;
      }

      GameObject go = Managers.Resource.Instantiate(skillData.prefab, pooling: true);
      go.transform.position = position;
      T t = go.GetOrAddComponent<T>();
      t.Init();

      return t;
    }

    return null;
  }

  public void Despawn<T>(T obj) where T : BaseController
  {
    // TODO: This is a test code for detecting pool despawn
    if (obj.IsValid() == false) return;
    
    System.Type type = typeof(T);
    if (type == typeof(PlayerController))
    {
      // TODO:
    }
    else if (type == typeof(MonsterController))
    {
      Monsters.Remove(obj as MonsterController);
      Managers.Resource.Destroy(obj.gameObject);
    }
    else if (type == typeof(GemController))
    {
      Gems.Remove(obj as GemController);
      Managers.Resource.Destroy(obj.gameObject);
      
      // TODO: This is temporal test code
      GameObject.Find("Grid").GetComponent<GridController>().Remove(obj.gameObject);
    }
    else if(type == typeof(ProjectileController))
    {
      Projectiles.Remove(obj as ProjectileController);
      Managers.Resource.Destroy(obj.gameObject);
    }
    // else if (typeof(T).IsSubclassOf(typeof(ProjectileController)))
    // {
    //   Projectiles.Remove(obj as ProjectileController);
    //   Managers.Resource.Destroy(obj.gameObject);
    // }
  }

  public void DespawnAllMonsters()
  {
    var monsters = Monsters.ToList();
    foreach (var monster in monsters)
      Despawn<MonsterController>(monster);
  }
  
  public void Clear()
  {
    Monsters.Clear();
    Gems.Clear();
    Souls.Clear();
    Projectiles.Clear();
  }
}
