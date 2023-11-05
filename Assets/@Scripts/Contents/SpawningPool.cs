using System.Collections;
using UnityEngine;

public class SpawningPool : MonoBehaviour
{
  private Coroutine _coUpdateSpawningPool;
  private GameManager _game;

  public void StartSpawn()
  {
    _game = Managers.Game;
    if (_coUpdateSpawningPool == null)
      _coUpdateSpawningPool = StartCoroutine(CoUpdateSpawningPool());
  }

  private IEnumerator CoUpdateSpawningPool()
  {
    while (true)
    {
      if (_game.CurrentWaveData.monsterId.Count == 1)
      {
        for (int i = 0; i < _game.CurrentWaveData.onceSpawnCount; i++)
        {
          Vector2 spawnPos = Utils.GenerateMonsterSpawnPosition(Managers.Game.Player.PlayerCenterPos);
          Managers.Object.Spawn<MonsterController>(spawnPos, _game.CurrentWaveData.monsterId[0]);
        }
        yield return new WaitForSeconds(_game.CurrentWaveData.spawnInterval);
      }
      else
      {
        for (int i = 0; i < _game.CurrentWaveData.onceSpawnCount; i++)
        {
          Vector2 spawnPos = Utils.GenerateMonsterSpawnPosition(Managers.Game.Player.PlayerCenterPos);

          if (Random.value <= Managers.Game.CurrentWaveData.firstMonsterSpawnRate) // 90%의 확률로 첫번째 MonsterId 사용
          {
            Managers.Object.Spawn<MonsterController>(spawnPos, _game.CurrentWaveData.monsterId[0]);
          }
          else // 10%의 확률로 다른 MonsterId 사용
          {
            int randomIndex = Random.Range(1, _game.CurrentWaveData.monsterId.Count);
            Managers.Object.Spawn<MonsterController>(spawnPos, _game.CurrentWaveData.monsterId[randomIndex]);
          }
        }
        yield return new WaitForSeconds(_game.CurrentWaveData.spawnInterval);
      }
    }
  }
}
