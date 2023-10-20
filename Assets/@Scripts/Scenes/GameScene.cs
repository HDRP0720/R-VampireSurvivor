using System;
using Data;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

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
  private void OnDestroy()
  {
    if (Managers.Game != null)
    {
      Managers.Game.OnGemCountChanged -= HandleOnGemCountChanged;
      Managers.Game.OnKillCountChanged -= HandleOnKillCountChanged;
    }
  }

  private void StartLoaded()
  {
    Managers.Data.Init();
    Managers.UI.ShowSceneUI<UI_GameScene>();
    
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
    foreach (var playerData in Managers.Data.PlayerDic.Values)
    {
      Debug.Log($"Lvl : {playerData.level}, HP : {playerData.maxHp}");
    }
    
    // UI refresh
    Managers.Game.OnKillCountChanged -= HandleOnKillCountChanged;
    Managers.Game.OnKillCountChanged += HandleOnKillCountChanged;
    Managers.Game.OnGemCountChanged -= HandleOnGemCountChanged;
    Managers.Game.OnGemCountChanged += HandleOnGemCountChanged;
  }

  // TODO: temporal variables for testing ui
  private int _collectedGemCount = 0;
  private int _remainingTotalGemCount = 10;
  private void HandleOnGemCountChanged(int gemCount)
  {
    _collectedGemCount++;
    if (_collectedGemCount == _remainingTotalGemCount)
    {
      Managers.UI.ShowPopup<UI_SkillSelectPopup>();
      _collectedGemCount = 0;
      _remainingTotalGemCount *= 2;
    }
    
    Managers.UI.GetSceneUI<UI_GameScene>().SetGemCountRatio((float)_collectedGemCount / _remainingTotalGemCount);
  }

  private void HandleOnKillCountChanged(int killCount)
  {
    Managers.UI.GetSceneUI<UI_GameScene>().SetKillCount(killCount);
  }
}
