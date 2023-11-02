using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager
{
  #region Properties
  public PlayerController Player { get; private set; }
  public HashSet<MonsterController> Monsters { get; } = new HashSet<MonsterController>();
  public HashSet<GemController> Gems { get; } = new HashSet<GemController>();
  public HashSet<SoulController> Souls { get; } = new HashSet<SoulController>();
  public HashSet<DropItemController> DropItems { get; } = new HashSet<DropItemController>();
  public HashSet<ProjectileController> Projectiles { get; } = new HashSet<ProjectileController>();
  #endregion
  
  // Constructor
  public ObjectManager()
  {
    Init();
  }
  
  public void Init() { }

  public void LoadMap(string mapName)
  {
    GameObject objMap = Managers.Resource.Instantiate(mapName);
    objMap.transform.position = Vector3.zero;
    objMap.name = "Map";

    objMap.GetComponent<Map>().Init();
  }
  
  public void ShowDamageFont(Vector2 pos, float damage, float healAmount, Transform parent, bool isCritical = false)
  {
    string prefabName;
    if (isCritical)
      prefabName = "CriticalDamageFont";
    else
      prefabName = "DamageFont";

    GameObject go = Managers.Resource.Instantiate(prefabName, pooling: true);
    DamageFont damageText = go.GetOrAddComponent<DamageFont>();
    damageText.SetInfo(pos, damage, healAmount, parent, isCritical);
  }

  public T Spawn<T>(Vector3 position, int templateID = 0, string prefabName = "") where T : BaseController
    {
        System.Type type = typeof(T);

        if (type == typeof(PlayerController))
        {
            GameObject go = Managers.Resource.Instantiate(Managers.Data.CreatureDic[templateID].prefabLabel);
            go.transform.position = position;
            PlayerController pc = go.GetOrAddComponent<PlayerController>();
            pc.SetInfo(templateID);
            Player = pc;
            Managers.Game.Player = pc;

            return pc as T;
        }
        else if (type == typeof(MonsterController))
        {
            Data.CreatureData cd = Managers.Data.CreatureDic[templateID];
            GameObject go = Managers.Resource.Instantiate($"{cd.PrefabLabel}", pooling: true);
            MonsterController mc = go.GetOrAddComponent<MonsterController>();
            go.transform.position = position;
            mc.SetInfo(templateID);
            go.name = cd.PrefabLabel;
            Monsters.Add(mc);

            return mc as T;
        }
        else if (type == typeof(EliteController))
        {
            Data.CreatureData cd = Managers.Data.CreatureDic[templateID];
            GameObject go = Managers.Resource.Instantiate($"{cd.PrefabLabel}", pooling: true);
            EliteController mc = go.GetOrAddComponent<EliteController>();
            go.transform.position = position;
            mc.SetInfo(templateID);
            go.name = cd.PrefabLabel;
            Monsters.Add(mc);

            return mc as T;
        }

        else if (type == typeof(BossController))
        {
            Data.CreatureData cd = Managers.Data.CreatureDic[templateID];

            GameObject go = Managers.Resource.Instantiate($"{cd.PrefabLabel}");
            BossController mc = go.GetOrAddComponent<BossController>();
            mc.enabled = true; // Disabled 상태로 Attatch됨
            go.transform.position = position;
            mc.SetInfo(templateID);
            go.name = cd.PrefabLabel;
            Monsters.Add(mc);

            return mc as T;
        }
        else if (type == typeof(GemController))
        {
            GameObject go = Managers.Resource.Instantiate("ExpGem", pooling: true);
            GemController gc = go.GetOrAddComponent<GemController>();
            go.transform.position = position;
            Gems.Add(gc);
            Managers.Game.CurrentMap.Grid.Add(gc);

            return gc as T;
        }
        else if (type == typeof(SoulController))
        {
            GameObject go = Managers.Resource.Instantiate("Soul", pooling: true);
            SoulController gc = go.GetOrAddComponent<SoulController>();
            go.transform.position = position;
            Souls.Add(gc);
            Managers.Game.CurrentMap.Grid.Add(gc);

            return gc as T;
        }
        else if (type == typeof(PotionController))
        {
            GameObject go = Managers.Resource.Instantiate("Potion", pooling: true);
            PotionController pc = go.GetOrAddComponent<PotionController>();
            go.transform.position = position;
            DropItems.Add(pc);
            Managers.Game.CurrentMap.Grid.Add(pc);

            return pc as T;
        }
        else if (type == typeof(BombController))
        {
            GameObject go = Managers.Resource.Instantiate("Bomb", pooling: true);
            BombController bc = go.GetOrAddComponent<BombController>();
            go.transform.position = position;
            DropItems.Add(bc);
            Managers.Game.CurrentMap.Grid.Add(bc);

            return bc as T;
        }
        else if (type == typeof(MagnetController))
        {
            GameObject go = Managers.Resource.Instantiate("Magnet", pooling: true);
            MagnetController mc = go.GetOrAddComponent<MagnetController>();
            go.transform.position = position;
            DropItems.Add(mc);
            Managers.Game.CurrentMap.Grid.Add(mc);

            return mc as T;
        }
        else if (type == typeof(EliteBoxController))
        {
            GameObject go = Managers.Resource.Instantiate("DropBox", pooling: true);
            EliteBoxController bc = go.GetOrAddComponent<EliteBoxController>();
            go.transform.position = position;
            DropItems.Add(bc);
            Managers.Game.CurrentMap.Grid.Add(bc);
            Managers.Sound.Play(Sound.Effect, "Drop_Box");
            return bc as T;
        }
        else if (type == typeof(ProjectileController))
        {
            GameObject go = Managers.Resource.Instantiate(prefabName, pooling: true);
            ProjectileController pc = go.GetOrAddComponent<ProjectileController>();
            go.transform.position = position;
            Projectiles.Add(pc);

            return pc as T;
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
