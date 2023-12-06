using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static Define;

public class AdsManager
{
  public enum EAdsStateType { None, Failed, Success }

  // private RewardedAd _rewardedAd;
  private Coroutine _coroutine;
  
  // Action
  private Action _rewardedCallback;
}
