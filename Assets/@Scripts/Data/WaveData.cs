using System;
using System.Collections.Generic;

namespace Data
{
  [Serializable]
  public class WaveData
  {
    public int stageIndex = 1;
    public int waveIndex = 1;
    public float spawnInterval = 0.5f;
    public int onceSpawnCount;
    public List<int> monsterId;
    public List<int> eliteId;
    public List<int> bossId;
    public float remainsTime;
    public Define.EWaveType waveType;
    public float firstMonsterSpawnRate;
    public float hpIncreaseRate;
    public float nonDropRate;
    public float smallGemDropRate;
    public float greenGemDropRate;
    public float blueGemDropRate;
    public float yellowGemDropRate;
    public List<int> eliteDropItemId;
  }
  [Serializable]
  public class WaveDataLoader : ILoader<int, WaveData>
  {
    public List<WaveData> waves = new List<WaveData>();

    public Dictionary<int, WaveData> MakeDict()
    {
      Dictionary<int, WaveData> dict = new Dictionary<int, WaveData>();
      foreach (WaveData wave in waves)
        dict.Add(wave.waveIndex, wave);
      return dict;
    }
  }
}
