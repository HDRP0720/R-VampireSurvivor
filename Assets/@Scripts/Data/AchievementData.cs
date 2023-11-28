using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
  [Serializable]
  public class AchievementData
  {
    public int achievementID;
    public string descriptionTextID;
    public Define.EMissionTarget missionTarget;
    public int missionTargetValue;
    public int clearRewardItemId;
    public int rewardValue;
    public bool isCompleted;
    public bool isRewarded;
    public int progressValue;
  }
  
  [Serializable]
  public class AchievementDataLoader : ILoader<int, AchievementData>
  {
    public List<AchievementData> achievements = new List<AchievementData>();
    
    public Dictionary<int, AchievementData> MakeDict()
    {
      Dictionary<int, AchievementData> dict = new Dictionary<int, AchievementData>();
      foreach (AchievementData ach in achievements)
        dict.Add(ach.achievementID, ach);
      return dict;
    }
  }
}
