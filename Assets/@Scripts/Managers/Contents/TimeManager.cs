using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

using Data;
using static Define;

public class TimeManager : MonoBehaviour
{
  private float _minute = 60;
  private DateTime _lastRewardTime;

  #region Properties
  public float RewardInterval { get; set; } = 10f;
  public int AttendanceDay
  {
    get
    {
      int savedTime= PlayerPrefs.GetInt("AttendanceDay", 1);
      return savedTime;
    }
    set
    {
      PlayerPrefs.SetInt("AttendanceDay", value);
      Managers.Achievement.Attendance();
      PlayerPrefs.Save();
    }
  }
  public float StaminaTime
  {
    get 
    {
      float time = PlayerPrefs.GetFloat("StaminaTime", STAMINA_RECHARGE_INTERVAL);
      return time;
    }
    set 
    {
      float time = value;
      PlayerPrefs.SetFloat("StaminaTime", time);
      PlayerPrefs.Save();
    }
  }
  public DateTime LastGeneratedStaminaTime
  {
    get
    {
      string savedTimeStr = PlayerPrefs.GetString("LastGeneratedStaminaTime", string.Empty);
      if (!string.IsNullOrEmpty(savedTimeStr))
        return DateTime.Parse(savedTimeStr);
      else
        return DateTime.Now;
    }
    set
    {
      string timeStr = value.ToString();
      PlayerPrefs.SetString("LastGeneratedStaminaTime", timeStr);
      PlayerPrefs.Save();
    }
  }
  public TimeSpan TimeSinceLastStamina => DateTime.Now - LastGeneratedStaminaTime;
  public DateTime LastLoginTime
  {
    get
    {
      string savedTimeStr = PlayerPrefs.GetString("LastLoginTime", string.Empty);
      if (!string.IsNullOrEmpty(savedTimeStr))
        return DateTime.Parse(savedTimeStr);
      else
        return DateTime.Now;
    }
    set
    {
      string timeStr = value.ToString();
      PlayerPrefs.SetString("LastLoginTime", timeStr);
      PlayerPrefs.Save();
    }
  }
  public DateTime LastRewardTime
  {
    get
    {
      if (_lastRewardTime == default(DateTime))
      {
        string savedTimeStr = PlayerPrefs.GetString("LastRewardTime", string.Empty);
        if (!string.IsNullOrEmpty(savedTimeStr))
          _lastRewardTime = DateTime.Parse(savedTimeStr);
        else
          _lastRewardTime = DateTime.Now;
      }

      return _lastRewardTime;
    }
    set
    {
      _lastRewardTime = value;
      string timeStr = value.ToString();
      PlayerPrefs.SetString("LastRewardTime", timeStr);
      PlayerPrefs.Save();
    }
  }
  public TimeSpan TimeSinceLastReward
  {
    get
    {
      TimeSpan timeSpan = DateTime.Now - LastRewardTime;
      if (timeSpan > TimeSpan.FromHours(24))
        return TimeSpan.FromHours(24);
      
      return timeSpan;
    }
  }
  #endregion

  public void Init()
  {
    StartTimer();
  }
  private void StartTimer()
  {
    StartCoroutine(CoStartTimer());
  }
  private IEnumerator CoStartTimer()
  {
    while (true)
    {
      yield return new WaitForSeconds(1f);
      
      StaminaTime--;
      _minute--;
      TimeSpan timeSpan = TimeSpan.FromSeconds(StaminaTime);
      string formattedTime = string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);

      if (StaminaTime == 0)
      {
        RechargeStamina();
        StaminaTime = Define.STAMINA_RECHARGE_INTERVAL;
      }

      if(_minute == 0)
      {
        CheckAttendance();
        _minute = 60;
      }
    }
  }
  private void RechargeStamina(int count = 1)
  {
    if (Managers.Game.Stamina < Define.MAX_STAMINA)
    { 
      Managers.Game.Stamina += count;
      LastGeneratedStaminaTime = DateTime.Now;
    }
  }
  private void CheckAttendance()
  {
    if (IsSameDay(LastLoginTime, DateTime.Now) == false)
    {
      AttendanceDay++;
      LastLoginTime = DateTime.Now;
            
      // 모든 하루 n번 가능한 변수들 클리어
      Managers.Game.CloverCountAds = 1;         // 상점에서 광고를 보고 클로버(부활권)를 얻을 수 있는 기회
      Managers.Game.GachaCountAdsAdvanced = 1;  // 상점에서 광고를 보고 고급 가챠를 돌릴 수 있는 기회
      Managers.Game.GachaCountAdsCommon = 1;    // 상점에서 광고를 보고 일반 가챠를 돌릴 수 있는 기회
      Managers.Game.GoldCountAds = 1;           // 상점에서 광고를 보고 골드를 얻을 수 있는 기회
      
      Managers.Game.SkillRefreshCountAds = 3;   // 전투에서 광고를 보고 추천 스킬을 변경할 수 있는 기회
      Managers.Game.RebirthCountAds = 3;        // 전투에서 광고를 보고 부활을 할 수 있는 기회
      
      Managers.Game.DiaCountAds = 3;            // 로비에서 광고를 보고 다이아 재화를 얻을 수 있는 기회
      Managers.Game.StaminaCountAds = 1;        // 로비에서 광고를 보고 스태미나를 충전 할 수 있는 기회
      Managers.Game.FastRewardCountAds = 1;     // 로비에서 광고를 보고 빠른 정찰 보수를 얻을 수 있는 기회
      
      Managers.Game.GainStaminaByDia = 3;
      
      Managers.Game.FastRewardCountStamina = 3; // 로비에서 스태미나를 사용하여 빠른 정찰 보수를 얻을 수 있는 기회

      Managers.Game.DicMission.Clear();
      Managers.Game.DicMission = new Dictionary<EMissionTarget, MissionInfo>()
      {
        {EMissionTarget.StageEnter, new MissionInfo() { progress = 0, isRewarded = false }},
        {EMissionTarget.StageClear, new MissionInfo() { progress = 0, isRewarded = false }},
        {EMissionTarget.EquipmentLevelUp, new MissionInfo() { progress = 0, isRewarded = false }},
        {EMissionTarget.OfflineRewardGet, new MissionInfo() { progress = 0, isRewarded = false }},
        {EMissionTarget.EquipmentMerge, new MissionInfo() { progress = 0, isRewarded = false }},
        {EMissionTarget.MonsterKill, new MissionInfo() { progress = 0, isRewarded = false }},
        {EMissionTarget.EliteMonsterKill, new MissionInfo() { progress = 0, isRewarded = false }},
        {EMissionTarget.GachaOpen, new MissionInfo() { progress = 0, isRewarded = false }},
        {EMissionTarget.ADWatchIng, new MissionInfo() { progress = 0, isRewarded = false }},
      };
      Managers.Game.SaveGame();
    }
  }
  private bool IsSameDay(DateTime savedTime, DateTime currentTime)
  {
    if (LastLoginTime.Day == DateTime.Now.Day)
      return true;
    else
      return false;
  }
  
  public void GiveOfflineReward(OfflineRewardData data)
  {
    string[] spriteName = new string[1];
    int[] count = new int[1];
    int gold = (int)CalculateGoldPerMinute(data.reward_Gold);

    spriteName[0] = Define.GOLD_SPRITE_NAME;
    count[0] = gold;

    Managers.Game.Gold += gold;
    LastRewardTime = DateTime.Now;
    if (Managers.Game.DicMission.TryGetValue(EMissionTarget.OfflineRewardGet, out MissionInfo info))
      info.progress++;
    Managers.Game.OfflineRewardCount++;

    UI_RewardPopup rewardPopup = (Managers.UI.SceneUI as UI_LobbyScene).RewardPopupUI;
    rewardPopup.gameObject.SetActive(true);
    rewardPopup.SetInfo(spriteName, count);
  }
  public void GiveFastOfflineReward(OfflineRewardData data)
  {

    string[] spriteName = new string[3];
    int[] count = new int[3];
    int gold = data.reward_Gold * 5;

    spriteName[0] = GOLD_SPRITE_NAME;
    count[0] = gold;

    spriteName[1] = Managers.Data.MaterialDic[ID_RANDOM_SCROLL].spriteName;
    count[1] = data.fastReward_Scroll;

    spriteName[2] = Managers.Data.MaterialDic[ID_SILVER_KEY].spriteName;
    count[2] = data.fastReward_Scroll;

    UI_RewardPopup rewardPopup = (Managers.UI.SceneUI as UI_LobbyScene).RewardPopupUI;
    rewardPopup.gameObject.SetActive(true);

    if (Managers.Game.DicMission.TryGetValue(EMissionTarget.OfflineRewardGet, out MissionInfo mission))
      mission.progress++;
    Managers.Game.FastRewardCount++;

    Managers.Game.ExchangeMaterial(Managers.Data.MaterialDic[Define.ID_RANDOM_SCROLL], data.fastReward_Scroll);
    Managers.Game.ExchangeMaterial(Managers.Data.MaterialDic[Define.ID_SILVER_KEY], data.fastReward_Scroll);
    Managers.Game.ExchangeMaterial(Managers.Data.MaterialDic[Define.ID_GOLD], gold);

    rewardPopup.SetInfo(spriteName, count);

  }
  
  public float CalculateGoldPerMinute(float goldPerHour)
  {
    float goldPerMinute = goldPerHour / 60f * (int)TimeSinceLastReward.TotalMinutes;
    return goldPerMinute;
  }
}
