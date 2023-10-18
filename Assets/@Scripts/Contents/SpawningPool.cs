using System;
using System.Collections;
using UnityEngine;

public class SpawningPool : MonoBehaviour
{
  private float _spawnInterval = 1.0f;
  private int _maxMonsterCount = 100;
  private Coroutine _coUpdateSpawningPool;

  private void Start()
  {
    _coUpdateSpawningPool = StartCoroutine(CoUpdateSpawningPool());
  }

  private IEnumerator CoUpdateSpawningPool()
  {
    while (true)
    {
      TrySpawn();
      yield return new WaitForSeconds(_spawnInterval);
    }
  }

  private void TrySpawn()
  {
    int monsterCount = Managers.Object.Monsters.Count;
    if (monsterCount >= _maxMonsterCount) return;
    
    // TODO: change temp ID to Data ID
    MonsterController mc = Managers.Object.Spawn<MonsterController>(UnityEngine.Random.Range(0, 2));
    mc.transform.position = new Vector2(UnityEngine.Random.Range(-5, 5), UnityEngine.Random.Range(-5, 5));
  }
}
