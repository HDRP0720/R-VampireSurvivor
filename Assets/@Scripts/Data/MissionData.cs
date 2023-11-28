using System;
using System.Collections.Generic;

namespace Data
{
  [Serializable]
  public class MissionData
  {
    public int missionId;
    public Define.EMissionType missionType;
    public string descriptionTextID;
    public Define.EMissionTarget missionTarget;
    public int missionTargetValue;
    public int clearRewardItmeId;
    public int rewardValue;
  }

  [Serializable]
  public class MissionDataLoader : ILoader<int, MissionData>
  {
    public List<MissionData> missions = new List<MissionData>();
    
    public Dictionary<int, MissionData> MakeDict()
    {
      Dictionary<int, MissionData> dict = new Dictionary<int, MissionData>();
      foreach (MissionData mis in missions)
        dict.Add(mis.missionId, mis);
      return dict;
    }
  }
}
