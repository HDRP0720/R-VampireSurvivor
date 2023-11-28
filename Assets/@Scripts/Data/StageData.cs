using System;
using System.Collections.Generic;

namespace Data
{
  [Serializable]
  public class StageData
  {
    public int stageIndex = 1;
    public string stageName;
    public int stageLevel = 1;
    public string mapName;
    public int stageSkill;

    public int firstWaveCountValue;
    public int firstWaveClearRewardItemId;
    public int firstWaveClearRewardItemValue;

    public int secondWaveCountValue;
    public int secondWaveClearRewardItemId;
    public int secondWaveClearRewardItemValue;

    public int thirdWaveCountValue;
    public int thirdWaveClearRewardItemId;
    public int thirdWaveClearRewardItemValue;

    public int clearRewardGold;
    public int clearRewardExp;
    public string stageImage;
    public List<int> appearingMonsters;
    public List<WaveData> waveArray;
  }
  
  [Serializable]
  public class StageDataLoader : ILoader<int, StageData>
  {
    public List<StageData> stages = new List<StageData>();

    public Dictionary<int, StageData> MakeDict()
    {
      Dictionary<int, StageData> dict = new Dictionary<int, StageData>();
      foreach (StageData stage in stages)
        dict.Add(stage.stageIndex, stage);
      return dict;
    }
  }
}