using System;
using UnityEngine;

public class GameManager
{
  public int Gold { get; set; }
  public int Gem { get; set; }
    
  private Vector2 _moveDir;
  
  // Delegate
  public event Action<Vector2> OnMoveDirChanged;
  
  // Property
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
}
