using UnityEngine;

public class GameScene : MonoBehaviour
{
  private SpawningPool _spawningPool;

  private void Start()
  {
    Managers.Resource.LoadAllAsync<GameObject>("Prefabs", (key, count, totalCount) =>
    {
      Debug.Log($"{key} : {count}/{totalCount}");

      if (count == totalCount)
      {
        Managers.Resource.LoadAllAsync<TextAsset>("Data", (key3, count3, totalCount3) =>
        {
          if (count3 == totalCount3)
            StartLoaded2();
        });
      }
    });
  }

  private void StartLoaded()
  {
    // Map
    var map = Managers.Resource.Instantiate("Map.prefab");
    
    // Monster
    GameObject go = new GameObject() { name = "Monsters" };
    var snake = Managers.Resource.Instantiate("Snake_01.prefab", go.transform);
    var goblin = Managers.Resource.Instantiate("Goblin_01.prefab", go.transform);
    
    // Player
    var slime = Managers.Resource.Instantiate("Slime_01.prefab");
    slime.AddComponent<PlayerController>();
    
    // Input
    var joystick = Managers.Resource.Instantiate("UI_Joystick.prefab");
    
    // Camera
    if (Camera.main != null) 
      Camera.main.GetComponent<CameraController>().target = slime;
  }

  private void StartLoaded2()
  {
    _spawningPool = gameObject.AddComponent<SpawningPool>();
    
    // Map
    var map = Managers.Resource.Instantiate("Map.prefab");
    
    // Player
    var player = Managers.Object.Spawn<PlayerController>();
    
    // Monster
    for (int i = 0; i < 100; i++)
    {
      MonsterController mc = Managers.Object.Spawn<MonsterController>(Random.Range(0, 2));
      mc.transform.position = new Vector2(Random.Range(-5, 5), Random.Range(-5, 5));
    }
    
    // Input
    var joystick = Managers.Resource.Instantiate("UI_Joystick.prefab");
    
    // Camera
    if (Camera.main != null) 
      Camera.main.GetComponent<CameraController>().target = player.gameObject;
    
    // Data test
    Managers.Data.Init();
    foreach (var playerData in Managers.Data.PlayerDic.Values)
    {
      Debug.Log($"Lvl : {playerData.level}, HP{playerData.maxHp}");
    }
  }
}
