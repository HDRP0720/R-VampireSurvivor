using UnityEngine;

public class GameScene : MonoBehaviour
{
  private SpawningPool _spawningPool;

  private void Start()
  {
    // Load resources through addressable
    Managers.Resource.LoadAllAsync<Object>("Preload", (key, count, totalCount) =>
    {
      Debug.Log($"{key} : {count}/{totalCount}");
      
      if (count == totalCount)
        StartLoaded();
    });
  }
  
  private void StartLoaded()
  {
    _spawningPool = gameObject.AddComponent<SpawningPool>();
    
    // Map
    var map = Managers.Resource.Instantiate("Map.prefab");
    
    // Player
    var player = Managers.Object.Spawn<PlayerController>(Vector3.zero);
    
    // Monster
    for (int i = 0; i < 100; i++)
    {
      Vector3 randPos = new Vector2(UnityEngine.Random.Range(-5, 5), UnityEngine.Random.Range(-5, 5));
      MonsterController mc = Managers.Object.Spawn<MonsterController>(randPos,Random.Range(0, 2));
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
