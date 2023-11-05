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
    AchievementData achievement = Achievements.Find(a => a.achievementID == dataId);
    if (achievement != null && achievement.isCompleted == false)
    {
      achievement.isCompleted = true;
      OnAchievementCompleted?.Invoke(achievement);
      Managers.Game.SaveGame();
    }
  }
  
  public void RewardedAchievement(int dataId)
  {
    AchievementData achievement = Achievements.Find(a => a.achievementID == dataId);
    if (achievement != null && achievement.isRewarded == false)
    {
      achievement.isRewarded = true;
      achievement.isCompleted = true;
      Managers.Game.SaveGame();
    }
  }
  
  public void Attendance()
  {
    List<AchievementData> list = Achievements.Where(data => data.missionTarget == Define.EMissionTarget.Login).ToList();

    foreach (AchievementData achievement in list)
    {
      if (!achievement.isCompleted && achievement.missionTargetValue == Managers.Time.AttendanceDay)
        CompleteAchievement(achievement.achievementID);
    }
  }
  
  public List<AchievementData> GetProceedingAchievment()
  {
    List<AchievementData> resultList = new List<AchievementData>();
    
    foreach(Define.EMissionTarget missionTarget in Enum.GetValues(typeof(Define.EMissionTarget)))
    {
      List<AchievementData> list = Achievements.Where(data => data.missionTarget == missionTarget).ToList();
      for(int i = 0; i < list.Count; i++)
      {
        if (list[i].isCompleted == false)
        {
          resultList.Add(list[i]);
          break;
        }
        else 
        {
          if (list[i].isRewarded == false)
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
    AchievementData achievement = Achievements.Find(a => a.achievementID == dataId + 1);
    if (achievement != null && achievement.isRewarded == false)
      return achievement;
    
    return null;
  }
  
  public void StageClear()
  {
    int maxStageClearIndex = Managers.Game.GetMaxStageClearIndex();

    List<AchievementData> list = Achievements.Where(data => data.missionTarget == Define.EMissionTarget.StageClear).ToList();
    foreach (AchievementData achievement in list)
    {
      if (!achievement.isCompleted && achievement.missionTargetValue == maxStageClearIndex)
        CompleteAchievement(achievement.achievementID);
    }
  }
  
  public void CommonOpen()
  {
    List<AchievementData> list = Achievements.Where(data => data.missionTarget == Define.EMissionTarget.StageClear).ToList();

    foreach (AchievementData achievement in Achievements)
    {
      if (!achievement.isCompleted && achievement.missionTargetValue == Managers.Game.CommonGachaOpenCount)
        CompleteAchievement(achievement.achievementID);
    }
  }
  
  public void AdvancedOpen()
  {
    List<AchievementData> list = Achievements.Where(data => data.missionTarget == Define.EMissionTarget.AdvancedGachaOpen).ToList();

    foreach (AchievementData achievement in list)
    {
      if (!achievement.isCompleted && achievement.missionTargetValue == Managers.Game.AdvancedGachaOpenCount)
        CompleteAchievement(achievement.achievementID);
    }
  }
  
  public void OfflineReward()
  {
    List<AchievementData> list = Achievements.Where(data => data.missionTarget == Define.EMissionTarget.OfflineRewardGet).ToList();

    foreach (AchievementData achievement in list)
    {
      if (!achievement.isCompleted && achievement.missionTargetValue == Managers.Game.OfflineRewardCount)
        CompleteAchievement(achievement.achievementID);
    }
  }
  
  public void FastReward()
  {
    List<AchievementData> list = Achievements.Where(data => data.missionTarget == Define.EMissionTarget.FastOfflineRewardGet).ToList();
       
    foreach (AchievementData achievement in list)
    {
      if (!achievement.isCompleted && achievement.missionTargetValue == Managers.Game.FastRewardCount)
        CompleteAchievement(achievement.achievementID);
    }
  }
  
  public void MonsterKill()
  {
    List<AchievementData> list = Achievements.Where(data => data.missionTarget == Define.EMissionTarget.MonsterKill).ToList();

    foreach (AchievementData achievement in list)
    {
      if (!achievement.isCompleted && achievement.missionTargetValue == Managers.Game.TotalMonsterKillCount)
        CompleteAchievement(achievement.achievementID);
    }
  }
  
  public void EliteKill()
  {
    List<AchievementData> list = Achievements.Where(data => data.missionTarget == Define.EMissionTarget.EliteMonsterKill).ToList();

    foreach (AchievementData achievement in list)
    {
      if (!achievement.isCompleted && achievement.missionTargetValue == Managers.Game.TotalEliteKillCount)
        CompleteAchievement(achievement.achievementID);
    }
  }
  
  public void BossKill()
  {
    List<AchievementData> list = Achievements.Where(data => data.missionTarget == Define.EMissionTarget.BossKill).ToList();

    foreach (AchievementData achievement in list)
    {
      if (!achievement.isCompleted && achievement.missionTargetValue == Managers.Game.TotalBossKillCount)
        CompleteAchievement(achievement.achievementID);
    }
  }
  #endregion
}
