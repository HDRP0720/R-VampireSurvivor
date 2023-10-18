using UnityEngine;

public class GameScene : MonoBehaviour
{
  // [SerializeField] private GameObject snakePrefab;
  // [SerializeField] private GameObject slimePrefab;
  // [SerializeField] private GameObject goblinPrefab;
  // [SerializeField] private GameObject joystickPrefab;

  // private GameObject _map;
  // private GameObject _snake;
  // private GameObject _slime;
  // private GameObject _goblin;
  // private GameObject _joystick;

  private void Start()
  {
    Managers.Resource.LoadAllAsync<GameObject>("Prefabs", (key, count, totalCount) =>
    {
      Debug.Log($"{key} : {count}/{totalCount}");

      if (count == totalCount)
        StartLoaded2();
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
    // Map
    var map = Managers.Resource.Instantiate("Map.prefab");
    
    // Player
    var player = Managers.Object.Spawn<PlayerController>();
    
    // Monster
    for (int i = 0; i < 1000; i++)
    {
      MonsterController mc = Managers.Object.Spawn<MonsterController>(Random.Range(0, 2));
      mc.transform.position = new Vector2(Random.Range(-5, 5), Random.Range(-5, 5));
    }
    
    // Input
    var joystick = Managers.Resource.Instantiate("UI_Joystick.prefab");
    
    // Camera
    if (Camera.main != null) 
      Camera.main.GetComponent<CameraController>().target = player.gameObject;
  }
}
