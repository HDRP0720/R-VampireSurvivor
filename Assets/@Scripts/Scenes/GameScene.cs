using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using Object = UnityEngine.Object;

using Data;
using static Define;
using static GemInfo;

public class GameScene : BaseScene
{
  enum EDropType { Potion, Magnet, Bomb }
  
  private GameManager _game;
  private PlayerController _player;
  private BossController _boss;
  private SpawningPool _spawningPool;
  private int _lastSecond = 30;
  private bool _isGameEnd = false;
  private UI_GameScene _ui;
  
  public Action<int> OnWaveStart;
  public Action<int> OnSecondChange;
  public Action OnWaveEnd;

  private void Awake()
  {
    Init();
    SceneChangeAnimation_Out anim = Managers.Resource.Instantiate("SceneChangeAnimation_Out").GetOrAddComponent<SceneChangeAnimation_Out>();
    anim.SetInfo(SceneType, () => { });
  }
  private void Update()
  {
    // TODO : This is test code, need to be removed after test
    #region TestCode
    if (Input.GetKeyDown(KeyCode.F1))
    {
      Managers.Game.Player.SoulCount += 100;
    }
    else if (Input.GetKeyDown(KeyCode.F2))
    {
      Managers.Game.Player.Exp += 5;
    }
    else if (Input.GetKeyDown(KeyCode.F3))
    {
      Managers.Object.KillAllMonsters();
    }
    else if (Input.GetKeyDown(KeyCode.F10))
    {
      WaveEnd();
    }
    #endregion
    
    if (_isGameEnd || _game.CurrentWaveData == null) return;
    
    if (_boss == null)
      _game.timeRemaining -= Time.deltaTime;
    else
      _game.timeRemaining = 0;
    
    int currentMinute = Mathf.FloorToInt(_game.timeRemaining / _game.CurrentWaveData.remainsTime);
    int currentSecond =(int)_game.timeRemaining;

    if (currentSecond != _lastSecond)
    {
      OnSecondChange?.Invoke(currentSecond);

      if (currentSecond == 30)
        SpawnWaveReward();
    }

    if (_game.timeRemaining < 0)
      WaveEnd();
    
    _lastSecond = currentSecond;
  }

  protected override void Init()
  {
    base.Init();
    
    SceneType = EScene.GameScene;
    Screen.sleepTimeout = SleepTimeout.NeverSleep;
    _game = Managers.Game;
    Managers.UI.ShowSceneUI<UI_Joystick>();

    if (_game.ContinueInfo.IsContinue)
    {
      _player = Managers.Object.Spawn<PlayerController>(Vector3.zero, _game.ContinueInfo.playerDataId);
    }
    else
    {
      _game.ClearContinueData();
      _player = Managers.Object.Spawn<PlayerController>(Vector3.zero, 201000);
    }

    LoadStage();

    _player.OnPlayerDead = HandleOnPlayerDead;

    Managers.Game.CameraController = FindObjectOfType<CameraController>();

    _ui = Managers.UI.ShowSceneUI<UI_GameScene>();
    
    //UI_GameScene만 따로 SortOrder을 줌 ( 영혼 획득 처리 떄문에 렌더모드를 ScreenSpace-Camera로 뒀기때문)
    _ui.GetComponent<Canvas>().sortingOrder = UI_GAMESCENE_SORT_CLOSED;

    OnWaveStart = _ui.HandleOnWaveStart;
    OnSecondChange = _ui.HandleOnSecondChange;
    OnWaveEnd = _ui.HandleOnWaveEnd;
    Managers.Sound.Play(ESound.BGM, "BGM_Game");
  }
  
  private void WaveEnd()
  {
    OnWaveEnd?.Invoke();
    if (_game.CurrentWaveIndex < _game.CurrentStageData.waveArray.Count - 1)
    {
      _game.CurrentWaveIndex++;

      StopAllCoroutines();
      StartCoroutine(CoStartWave(_game.CurrentStageData.waveArray[_game.CurrentWaveIndex]));
    }
    else
    {
      Vector2 spawnPos = Utils.GenerateMonsterSpawnPosition(_game.Player.PlayerCenterPos, 10, 15);

      //보스 출현
      for (int i = 0; i < _game.CurrentWaveData.bossId.Count; i++)
      {
        _boss = Managers.Object.Spawn<BossController>(spawnPos, _game.CurrentWaveData.bossId[i]);
        _boss.OnMonsterInfoUpdate -= _ui.HandleMonsterInfoUpdate;
        _boss.OnMonsterInfoUpdate += _ui.HandleMonsterInfoUpdate;
        _boss.OnBossDead -= HandleOnBossDead;
        _boss.OnBossDead += HandleOnBossDead;
      }
    }
  }
  
  private void HandleOnPlayerDead()
  {
    if (Managers.Game.isGameEnd == false)
    { 
      UI_ContinuePopup gp = Managers.UI.ShowPopupUI<UI_ContinuePopup>();
      gp.SetInfo();
    }
  }
  private void HandleOnBossDead()
  {
    StartCoroutine(CoGameEnd());
  }
  private IEnumerator CoGameEnd()
  {
    yield return new WaitForSeconds(1f);
    
    _isGameEnd = true;
    if (Managers.Game.DicMission.TryGetValue(EMissionTarget.StageClear, out MissionInfo mission))
      mission.progress++;

    Managers.Game.isGameEnd = true;
    UI_GameResultPopup cp = Managers.UI.ShowPopupUI<UI_GameResultPopup>();
    cp.SetInfo();
  }
  
  public void LoadStage()
  {
    if (_spawningPool == null)
      _spawningPool = gameObject.AddComponent<SpawningPool>();

    Managers.Object.LoadMap(_game.CurrentStageData.mapName);

    StopAllCoroutines();
    StartCoroutine(CoStartWave(_game.CurrentStageData.waveArray[_game.CurrentWaveIndex]));
  }
  private IEnumerator CoStartWave(WaveData wave)
  {
    yield return new WaitForEndOfFrame();
    OnWaveStart?.Invoke(wave.waveIndex);

    if (wave.waveIndex == 1)
    {
      GenerateRandomExperience(30);
    }

    SpawnWaveReward();
    _game.timeRemaining = _game.CurrentStageData.waveArray[_game.CurrentWaveIndex].remainsTime;
    _game.CurrentMap.ChangeMapSize(Define.MAPSIZE_REDUCTION_VALUE);

    Vector2 spawnPos = Utils.GenerateMonsterSpawnPosition(_game.Player.PlayerCenterPos);

    _spawningPool.StartSpawn();

    _game.SaveGame();

    //엘리트 몬스터 소환
    EliteController elite;
    for (int i = 0; i < _game.CurrentWaveData.eliteId.Count; i++)
    {
      elite = Managers.Object.Spawn<EliteController>(spawnPos, _game.CurrentWaveData.eliteId[i]);
      elite.OnMonsterInfoUpdate -= _ui.HandleMonsterInfoUpdate;
      elite.OnMonsterInfoUpdate += _ui.HandleMonsterInfoUpdate;
    }

    //yield return new WaitForSeconds(Define.BOSS_GEN_TIME);


    yield break;
  }

  private void SpawnWaveReward()
  {
    EDropType spawnType = (EDropType)UnityEngine.Random.Range(0, 3);

    Vector3 spawnPos = Utils.RandomPointInAnnulus(Managers.Game.Player.CenterPosition, 3, 6);
    Data.DropItemData dropItem;
    switch (spawnType)
    {
      case EDropType.Potion:
        if (Managers.Data.DropItemDataDic.TryGetValue(ID_POTION, out dropItem))
          Managers.Object.Spawn<PotionController>(spawnPos).SetInfo(dropItem);
        break;
      case EDropType.Magnet:
        if (Managers.Data.DropItemDataDic.TryGetValue(ID_MAGNET, out dropItem))
          Managers.Object.Spawn<MagnetController>(spawnPos).SetInfo(dropItem);
        break;
      case EDropType.Bomb:
        if (Managers.Data.DropItemDataDic.TryGetValue(ID_BOMB, out dropItem))
          Managers.Object.Spawn<BombController>(spawnPos).SetInfo(dropItem);
        break;
    }
  }
  
  private void GenerateRandomExperience(int n)
  {
    int[] coins = new int[] { 1, 2, 5, 10 };
    List<EGemType> combination = new List<EGemType>();

    int remainingValue = n;

    while (remainingValue > 0)
    {
      int coinIndex = UnityEngine.Random.Range(0, coins.Length);
      int coinValue = coins[coinIndex];

      if (remainingValue >= coinValue)
      {
        EGemType gemType = (EGemType)coinIndex; 
        combination.Add(gemType);
        remainingValue -= coinValue;
      }
    }

    foreach (EGemType type in combination) 
    {
      GemController gem = Managers.Object.Spawn<GemController>(Utils.RandomPointInAnnulus(Managers.Game.Player.CenterPosition,6,10));
      gem.SetInfo(Managers.Game.GetGemInfo(type));
    }

  }
  
  public override void Clear() { }
}
