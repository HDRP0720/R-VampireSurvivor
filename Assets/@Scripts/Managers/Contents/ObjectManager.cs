using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using static Define;

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
  public void Clear()
  {
    Monsters.Clear();
    Gems.Clear();
    Souls.Clear();
    Projectiles.Clear();
  }

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
            GameObject go = Managers.Resource.Instantiate($"{cd.prefabLabel}", pooling: true);
            MonsterController mc = go.GetOrAddComponent<MonsterController>();
            go.transform.position = position;
            mc.SetInfo(templateID);
            go.name = cd.prefabLabel;
            Monsters.Add(mc);

            return mc as T;
        }
        else if (type == typeof(EliteController))
        {
            Data.CreatureData cd = Managers.Data.CreatureDic[templateID];
            GameObject go = Managers.Resource.Instantiate($"{cd.prefabLabel}", pooling: true);
            EliteController mc = go.GetOrAddComponent<EliteController>();
            go.transform.position = position;
            mc.SetInfo(templateID);
            go.name = cd.prefabLabel;
            Monsters.Add(mc);

            return mc as T;
        }

        else if (type == typeof(BossController))
        {
            Data.CreatureData cd = Managers.Data.CreatureDic[templateID];

            GameObject go = Managers.Resource.Instantiate($"{cd.prefabLabel}");
            BossController mc = go.GetOrAddComponent<BossController>();
            mc.enabled = true; // Disabled 상태로 Attatch됨
            go.transform.position = position;
            mc.SetInfo(templateID);
            go.name = cd.prefabLabel;
            Monsters.Add(mc);

            return mc as T;
        }
        else if (type == typeof(GemController))
        {
            GameObject go = Managers.Resource.Instantiate("ExpGem", pooling: true);
            GemController gc = go.GetOrAddComponent<GemController>();
            go.transform.position = position;
            Gems.Add(gc);
            Managers.Game.CurrentMap.grid.Add(gc);

            return gc as T;
        }
        else if (type == typeof(SoulController))
        {
            GameObject go = Managers.Resource.Instantiate("Soul", pooling: true);
            SoulController gc = go.GetOrAddComponent<SoulController>();
            go.transform.position = position;
            Souls.Add(gc);
            Managers.Game.CurrentMap.grid.Add(gc);

            return gc as T;
        }
        else if (type == typeof(PotionController))
        {
            GameObject go = Managers.Resource.Instantiate("Potion", pooling: true);
            PotionController pc = go.GetOrAddComponent<PotionController>();
            go.transform.position = position;
            DropItems.Add(pc);
            Managers.Game.CurrentMap.grid.Add(pc);

            return pc as T;
        }
        else if (type == typeof(BombController))
        {
            GameObject go = Managers.Resource.Instantiate("Bomb", pooling: true);
            BombController bc = go.GetOrAddComponent<BombController>();
            go.transform.position = position;
            DropItems.Add(bc);
            Managers.Game.CurrentMap.grid.Add(bc);

            return bc as T;
        }
        else if (type == typeof(MagnetController))
        {
            GameObject go = Managers.Resource.Instantiate("Magnet", pooling: true);
            MagnetController mc = go.GetOrAddComponent<MagnetController>();
            go.transform.position = position;
            DropItems.Add(mc);
            Managers.Game.CurrentMap.grid.Add(mc);

            return mc as T;
        }
        else if (type == typeof(EliteBoxController))
        {
            GameObject go = Managers.Resource.Instantiate("DropBox", pooling: true);
            EliteBoxController bc = go.GetOrAddComponent<EliteBoxController>();
            go.transform.position = position;
            DropItems.Add(bc);
            Managers.Game.CurrentMap.grid.Add(bc);
            Managers.Sound.Play(ESound.Effect, "Drop_Box");
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
    else if (type == typeof(BossController))
    {
      Monsters.Remove(obj as MonsterController);
      Managers.Resource.Destroy(obj.gameObject);
    }
    else if (type == typeof(EliteController))
    {
      Monsters.Remove(obj as EliteController);
      Managers.Resource.Destroy(obj.gameObject);
    }
    else if (type == typeof(GemController))
    {
      Gems.Remove(obj as GemController);
      Managers.Resource.Destroy(obj.gameObject);
      Managers.Game.CurrentMap.grid.Remove(obj as GemController);
    }
    else if (type == typeof(SoulController))
    {
      Souls.Remove(obj as SoulController);
      Managers.Resource.Destroy(obj.gameObject);
      Managers.Game.CurrentMap.grid.Remove(obj as SoulController);
    }
    else if (type == typeof(PotionController))
    {
      Managers.Resource.Destroy(obj.gameObject);
      Managers.Game.CurrentMap.grid.Remove(obj as PotionController);
    }
    else if (type == typeof(MagnetController))
    {
      Managers.Resource.Destroy(obj.gameObject);
      Managers.Game.CurrentMap.grid.Remove(obj as MagnetController);
    }
    else if (type == typeof(BombController))
    {
      Managers.Resource.Destroy(obj.gameObject);
      Managers.Game.CurrentMap.grid.Remove(obj as BombController);
    }
    else if (type == typeof(EliteBoxController))
    {
      Managers.Resource.Destroy(obj.gameObject);
      Managers.Game.CurrentMap.grid.Remove(obj as EliteBoxController);
    }
    else if(type == typeof(ProjectileController))
    {
      Projectiles.Remove(obj as ProjectileController);
      Managers.Resource.Destroy(obj.gameObject);
    }
  }
  
  public List<MonsterController> GetNearestMonsters(int count = 1, int distanceThreshold = 0)
  {
    List<MonsterController> monsterList = Monsters.OrderBy(monster => (Player.CenterPosition - monster.CenterPosition).sqrMagnitude).ToList();

    if(distanceThreshold > 0)
      monsterList = monsterList.Where(monster => (Player.CenterPosition - monster.CenterPosition).magnitude > distanceThreshold).ToList();

    int min = Mathf.Min(count, monsterList.Count);
    List<MonsterController> nearestMonsters = monsterList.Take(min).ToList();
    if (nearestMonsters.Count == 0) return null;

    // 요소 개수가 count와 다른 경우 마지막 요소 반복해서 추가
    while (nearestMonsters.Count < count)
    {
      nearestMonsters.Add(nearestMonsters.Last());
    }

    return nearestMonsters;
  }
  
  public List<MonsterController> GetMonsterWithinCamera(int count = 1)
  {
    List<MonsterController> monsterList = Monsters.ToList().Where(monster => IsWithInCamera(Camera.main.WorldToViewportPoint(monster.CenterPosition)) == true).ToList();
    monsterList.Shuffle();

    int min = Mathf.Min(count, monsterList.Count);
    List<MonsterController> monsters = monsterList.Take(min).ToList();

    if(monsters.Count == 0) return null;

    while (monsters.Count < count)
      monsters.Add(monsters.Last());

    return monsterList.Take(count).ToList();
  }
  private bool IsWithInCamera(Vector3 pos)
  {
    if(pos.x >= 0 && pos.x <=1 && pos.y >= 0 && pos.y <= 1) return true;
    
    return false;
  }
  
  public List<Transform> GetFindMonstersInFanShape(Vector3 origin, Vector3 forward, float radius = 2, float angleRange = 80)
  {
    List<Transform> listMonster = new List<Transform>();
    LayerMask targetLayer = LayerMask.GetMask("Monster", "Boss");
    RaycastHit2D[] targets = Physics2D.CircleCastAll(origin, radius, Vector2.zero, 0, targetLayer);
    foreach (RaycastHit2D target in targets)
    {
      float dot = Vector3.Dot((target.transform.position - origin).normalized, forward);
      float theta = Mathf.Acos(dot);
      float degree = Mathf.Rad2Deg * theta;
      if (degree <= angleRange / 2f)
        listMonster.Add(target.transform);
    }

    return listMonster;
  }

  public void KillAllMonsters()
  {
    UI_GameScene scene = Managers.UI.SceneUI as UI_GameScene;

    if(scene != null)
      scene.DoWhiteFlash(); 
    foreach (MonsterController monster in Monsters.ToList())
    {
      if (monster.ObjectType == EObjectType.Monster)
        monster.OnDead();
    }
    DespawnAllMonsterProjectiles();
  }
  private void DespawnAllMonsterProjectiles()
  {
    foreach (ProjectileController proj in Projectiles.ToList())
    {
      if (proj.skill.SkillType == ESkillType.MonsterSkill_01)
        Despawn(proj);
    }
  }
  
  public void CollectAllItems()
  {
    foreach (GemController gem in Gems.ToList())
    {
      gem.GetItem();
    }

    foreach (SoulController soul in Souls.ToList())
    {
      soul.GetItem();
    }
  }
  
  public void DespawnAllMonsters()
  {
    var monsters = Monsters.ToList();
    foreach (var monster in monsters)
      Despawn<MonsterController>(monster);
  }
  

}
