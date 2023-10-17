using System;
using UnityEngine;

public class GameScene : MonoBehaviour
{
  [SerializeField] private GameObject snakePrefab;
  [SerializeField] private GameObject slimePrefab;
  [SerializeField] private GameObject goblinPrefab;

  private GameObject _snake;
  private GameObject _slime;
  private GameObject _goblin;

  private void Start()
  {
    GameObject go = new GameObject() { name = "Monsters" };
    _snake = GameObject.Instantiate(snakePrefab, go.transform);
    _goblin = GameObject.Instantiate(goblinPrefab, go.transform);
    _snake.name = snakePrefab.name;
    _goblin.name = goblinPrefab.name;
    
    // Player
    _slime = GameObject.Instantiate(slimePrefab);
    _slime.name = slimePrefab.name;
    _slime.AddComponent<PlayerController>();
    
    // Camera
    if (Camera.main != null) 
      Camera.main.GetComponent<CameraController>().target = _slime;
  }
}
