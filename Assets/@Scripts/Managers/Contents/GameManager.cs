using System;
using UnityEngine;

public class GameManager
{
  private int _gem;
  private int _killCount;
  private Vector2 _moveDir;
  
  // Delegate
  public event Action<Vector2> OnMoveDirChanged;
  public event Action<int> OnGemCountChanged;
  public event Action<int> OnKillCountChanged;
  
  // Property
  public int Gem
  {
    get => _gem;
    set
    {
      _gem = value;
      OnGemCountChanged?.Invoke(value);
    }
  }
  public int KillCount
  {
    get => _killCount;
    set
    {
      _killCount = value;
      OnKillCountChanged?.Invoke(value);
    }
  }
  public Vector2 MoveDir
  {
    get => _moveDir;
    set
    {
      _moveDir = value;
      OnMoveDirChanged?.Invoke(_moveDir);
    }
  }
  public PlayerController Player => Managers.Object?.Player;
  public int Gold { get; set; }
  
}
