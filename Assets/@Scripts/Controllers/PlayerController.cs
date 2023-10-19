using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : CreatureController
{
  private Vector2 _moveDir = Vector2.zero;
  private float _speed = 5.0f;
  private float EnvCollectDist { get; set; } = 1.0f;
  
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
    CollectGems();
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

  private void CollectGems()
  {
    float sqrCollectDist = EnvCollectDist * EnvCollectDist;
    
    List<GemController> gems = Managers.Object.Gems.ToList();
    foreach (GemController gem in gems)
    {
      Vector3 dir = gem.transform.position - transform.position;
      if (dir.sqrMagnitude <= sqrCollectDist)
      {
        Managers.Game.Gem += 1;
        Managers.Object.Despawn(gem);
      }
    }

    var findGems = GameObject.Find("Grid").GetComponent<GridController>()
      .GatherObjects(transform.position, EnvCollectDist + 0.5f);

    Debug.Log($"SearchGems({findGems.Count}) || TotalGems({gems.Count})");
  }
  
  private void OnCollisionEnter2D(Collision2D other)
  {
    MonsterController target = other.gameObject.GetComponent<MonsterController>();
    if (target == null) return;
    
    
  }

  public override void OnDamaged(BaseController attacker, int damage)
  {
    base.OnDamaged(attacker, damage);
    Debug.Log($"OnDamaged! {HP}");
    
    // TODO: This is temp code
    CreatureController cc = attacker as CreatureController;
    cc?.OnDamaged(this, 10000);
  }

  private void HandleOnMoveDirChanged(Vector2 dir)
  {
    _moveDir = dir;
  }
}
