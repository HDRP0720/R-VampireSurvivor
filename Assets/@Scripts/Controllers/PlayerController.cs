using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
  private Vector2 _moveDir = Vector2.zero;
  private float _speed = 5.0f;
  
  // Property
  public Vector2 MoveDir
  {
    get => _moveDir;
    set => _moveDir = value.normalized;
  }

  private void Start()
  {
    Managers.Game.OnMoveDirChanged += HandleOnMoveDirChanged;
  }
  private void Update()
  {
    MovePlayer();
  }
  private void OnDestroy()
  {
    if(Managers.Game != null)
      Managers.Game.OnMoveDirChanged -= HandleOnMoveDirChanged;
  }

  private void MovePlayer()
  {
    Vector2 dir = _moveDir * (_speed * Time.deltaTime);
    transform.position += new Vector3(dir.x, dir.y, 0);
  }

  private void HandleOnMoveDirChanged(Vector2 dir)
  {
    _moveDir = dir;
  }
}
