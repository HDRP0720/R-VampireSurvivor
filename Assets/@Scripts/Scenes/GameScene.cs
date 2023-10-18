using System;
using UnityEngine;

public class GameScene : MonoBehaviour
{
  // [SerializeField] private GameObject snakePrefab;
  // [SerializeField] private GameObject slimePrefab;
  // [SerializeField] private GameObject goblinPrefab;
  // [SerializeField] private GameObject joystickPrefab;

  private GameObject _snake;
  private GameObject _slime;
  private GameObject _goblin;
  private GameObject _joystick;

  private void Start()
  {
    Managers.Resource.LoadAllAsync<GameObject>("Prefabs", (key, count, totalCount) =>
    {
      Debug.Log($"{key} : {count}/{totalCount}");

      if (count == totalCount)
        StartLoaded();
    });
  }

  private void StartLoaded()
  {
    GameObject slimePrefab = Managers.Resource.Load<GameObject>("Slime_01.prefab");
    GameObject snakePrefab = Managers.Resource.Load<GameObject>("Snake_01.prefab");
    GameObject goblinPrefab = Managers.Resource.Load<GameObject>("Goblin_01.prefab");
    
    GameObject go = new GameObject() { name = "Monsters" };
    _snake = GameObject.Instantiate(snakePrefab, go.transform);
    _goblin = GameObject.Instantiate(goblinPrefab, go.transform);
    _snake.name = snakePrefab.name;
    _goblin.name = goblinPrefab.name;
    
    // Input
    // _joystick = GameObject.Instantiate(joystickPrefab);
    // _joystick.name = joystickPrefab.name;
    
    // Player
    _slime = GameObject.Instantiate(slimePrefab);
    _slime.name = slimePrefab.name;
    _slime.AddComponent<PlayerController>();
    
    // Camera
    if (Camera.main != null) 
      Camera.main.GetComponent<CameraController>().target = _slime;
  }
}
