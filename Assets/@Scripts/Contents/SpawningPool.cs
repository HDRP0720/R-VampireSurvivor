using System.Collections;
using UnityEngine;

public class SpawningPool : MonoBehaviour
{
  private float _spawnInterval = 0.1f;
  private int _maxMonsterCount = 100;
  private Coroutine _coUpdateSpawningPool;
  
  // Property
  public bool IsStopped { get; set; } = false;

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
    if (IsStopped) return;
    
    int monsterCount = Managers.Object.Monsters.Count;
    if (monsterCount >= _maxMonsterCount) return;
    
    // TODO: change temp ID to Data ID
    Vector3 randPos = Utils.GenerateMonsterSpawnPoint(Managers.Game.Player.transform.position, 10, 15);
    MonsterController mc = Managers.Object.Spawn<MonsterController>(randPos, Random.Range(0, 2) + 1);
  }
}
