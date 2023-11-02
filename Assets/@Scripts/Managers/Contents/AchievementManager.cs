using System;
using System.Linq;
using System.Collections.Generic;

using Data;

public class AchievementManager
{
  // Property
  private List<AchievementData> Achievements
  {
    get => Managers.Game.Achievements;
    set => Managers.Game.Achievements = value;
  }
  
  // delegate
  public event Action<AchievementData> OnAchievementCompleted;

  #region Functions
  public void Init()
  {
    Achievements = Managers.Data.AchievementDataDic.Values.ToList();
  }

  public void CompleteAchievement(int dataId)
  {
    AchievementData achievement = Achievements.Find(a => a.AchievementID == dataId);
    if (achievement != null && achievement.IsCompleted == false)
    {
      achievement.IsCompleted = true;
      OnAchievementCompleted?.Invoke(achievement);
      Managers.Game.SaveGame();
    }
  }
  
  public void RewardedAchievement(int dataId)
  {
    AchievementData achievement = Achievements.Find(a => a.AchievementID == dataId);
    if (achievement != null && achievement.IsRewarded == false)
    {
      achievement.IsRewarded = true;
      achievement.IsCompleted = true;
      Managers.Game.SaveGame();
    }
  }
  
  public void Attendance()
  {
    List<AchievementData> list = Achievements.Where(data => data.MissionTarget == Define.EMissionTarget.Login).ToList();

    foreach (AchievementData achievement in list)
    {
      if (!achievement.IsCompleted && achievement.MissionTargetValue == Managers.Time.AttendanceDay)
        CompleteAchievement(achievement.AchievementID);
    }
  }
  
  public List<AchievementData> GetProceedingAchievment()
  {
    List<AchievementData> resultList = new List<AchievementData>();
    
    foreach(Define.EMissionTarget missionTarget in Enum.GetValues(typeof(Define.EMissionTarget)))
    {
      List<AchievementData> list = Achievements.Where(data => data.MissionTarget == missionTarget).ToList();
      for(int i = 0; i < list.Count; i++)
      {
        if (list[i].IsCompleted == false)
        {
          resultList.Add(list[i]);
          break;
        }
        else 
        {
          if (list[i].IsRewarded == false)
          {
            resultList.Add(list[i]);
            break;
          }

          if(i == list.Count - 1)
            resultList.Add(list[i]);
        }
      }
    }

    return resultList;
  }

  public int GetProgressValue(Define.EMissionTarget missionTarget)
  {
    switch (missionTarget)
    {
      case Define.EMissionTarget.DailyComplete:
      case Define.EMissionTarget.WeeklyComplete:
        return 0;
      
      case Define.EMissionTarget.StageEnter:
        return Managers.Game.DicMission[missionTarget].progress;
      
      case Define.EMissionTarget.StageClear:
        return Managers.Game.GetMaxStageClearIndex();
      
      case Define.EMissionTarget.EquipmentLevelUp:
        return Managers.Game.DicMission[missionTarget].progress;
      
      case Define.EMissionTarget.CommonGachaOpen:
        return Managers.Game.CommonGachaOpenCount;
            
      case Define.EMissionTarget.AdvancedGachaOpen:
        return Managers.Game.AdvancedGachaOpenCount;
            
      case Define.EMissionTarget.OfflineRewardGet:
        return Managers.Game.OfflineRewardCount;
            
      case Define.EMissionTarget.FastOfflineRewardGet:
        return Managers.Game.FastRewardCount;
            
      case Define.EMissionTarget.ShopProductBuy:
        return 0;
            
      case Define.EMissionTarget.Login:
        return Managers.Time.AttendanceDay;
            
      case Define.EMissionTarget.EquipmentMerge:
        return Managers.Game.DicMission[missionTarget].progress;
      
      case Define.EMissionTarget.MonsterAttack:
        return 0;
            
      case Define.EMissionTarget.MonsterKill:
        return Managers.Game.TotalMonsterKillCount;

      case Define.EMissionTarget.EliteMonsterAttack:
        return 0;

      case Define.EMissionTarget.EliteMonsterKill:
        return Managers.Game.TotalEliteKillCount;

      case Define.EMissionTarget.BossKill:
        return Managers.Game.TotalBossKillCount;
         
      case Define.EMissionTarget.DailyShopBuy:
        return 0;
      
      case Define.EMissionTarget.GachaOpen:
        return Managers.Game.DicMission[missionTarget].progress;
         
      case Define.EMissionTarget.ADWatchIng:
        return Managers.Game.DicMission[missionTarget].progress;
    }
    return 0;
  }
  
  public AchievementData GetNextAchievment(int dataId)
  {
    AchievementData achievement = Achievements.Find(a => a.AchievementID == dataId + 1);
    if (achievement != null && achievement.IsRewarded == false)
      return achievement;
    
    return null;
  }
  
  public void StageClear()
  {
    int maxStageClearIndex = Managers.Game.GetMaxStageClearIndex();

    List<AchievementData> list = Achievements.Where(data => data.MissionTarget == Define.EMissionTarget.StageClear).ToList();
    foreach (AchievementData achievement in list)
    {
      if (!achievement.IsCompleted && achievement.MissionTargetValue == maxStageClearIndex)
        CompleteAchievement(achievement.AchievementID);
    }
  }
  
  public void CommonOpen()
  {
    List<AchievementData> list = Achievements.Where(data => data.MissionTarget == Define.EMissionTarget.StageClear).ToList();

    foreach (AchievementData achievement in Achievements)
    {
      if (!achievement.IsCompleted && achievement.MissionTargetValue == Managers.Game.CommonGachaOpenCount)
        CompleteAchievement(achievement.AchievementID);
    }
  }
  
  public void AdvancedOpen()
  {
    List<AchievementData> list = Achievements.Where(data => data.MissionTarget == Define.EMissionTarget.AdvancedGachaOpen).ToList();

    foreach (AchievementData achievement in list)
    {
      if (!achievement.IsCompleted && achievement.MissionTargetValue == Managers.Game.AdvancedGachaOpenCount)
        CompleteAchievement(achievement.AchievementID);
    }
  }
  
  public void OfflineReward()
  {
    List<AchievementData> list = Achievements.Where(data => data.MissionTarget == Define.EMissionTarget.OfflineRewardGet).ToList();

    foreach (AchievementData achievement in list)
    {
      if (!achievement.IsCompleted && achievement.MissionTargetValue == Managers.Game.OfflineRewardCount)
        CompleteAchievement(achievement.AchievementID);
    }
  }
  
  public void FastReward()
  {
    List<AchievementData> list = Achievements.Where(data => data.MissionTarget == Define.EMissionTarget.FastOfflineRewardGet).ToList();
       
    foreach (AchievementData achievement in list)
    {
      if (!achievement.IsCompleted && achievement.MissionTargetValue == Managers.Game.FastRewardCount)
        CompleteAchievement(achievement.AchievementID);
    }
  }
  
  public void MonsterKill()
  {
    List<AchievementData> list = Achievements.Where(data => data.MissionTarget == Define.EMissionTarget.MonsterKill).ToList();

    foreach (AchievementData achievement in list)
    {
      if (!achievement.IsCompleted && achievement.MissionTargetValue == Managers.Game.TotalMonsterKillCount)
        CompleteAchievement(achievement.AchievementID);
    }
  }
  
  public void EliteKill()
  {
    List<AchievementData> list = Achievements.Where(data => data.MissionTarget == Define.EMissionTarget.EliteMonsterKill).ToList();

    foreach (AchievementData achievement in list)
    {
      if (!achievement.IsCompleted && achievement.MissionTargetValue == Managers.Game.TotalEliteKillCount)
        CompleteAchievement(achievement.AchievementID);
    }
  }
  
  public void BossKill()
  {
    List<AchievementData> list = Achievements.Where(data => data.MissionTarget == Define.EMissionTarget.BossKill).ToList();

    foreach (AchievementData achievement in list)
    {
      if (!achievement.IsCompleted && achievement.MissionTargetValue == Managers.Game.TotalBossKillCount)
        CompleteAchievement(achievement.AchievementID);
    }
  }
  #endregion
}
