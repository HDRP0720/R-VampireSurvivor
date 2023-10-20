using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : CreatureController
{
  [SerializeField] private Transform indicator;
  [SerializeField] private Transform projectilePointer;
  private Vector2 _moveDir = Vector2.zero;
  
  private float EnvCollectDist { get; set; } = 1.0f;
  
  // Property
  public Vector2 MoveDir
  {
    get => _moveDir;
    set => _moveDir = value.normalized;
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

  public override bool Init()
  {
    if (base.Init() == false) return false;

    _speed = 5.0f;
    Managers.Game.OnMoveDirChanged += HandleOnMoveDirChanged;
    
    StartProjectile();

    return true;
  }

  private void MovePlayer()
  {
    Vector2 dir = _moveDir * (_speed * Time.deltaTime);
    transform.position += new Vector3(dir.x, dir.y, 0);

    if (_moveDir != Vector2.zero)
      indicator.eulerAngles = new Vector3(0, 0, Mathf.Atan2(-dir.x, dir.y) * 180 / Mathf.PI);

    GetComponent<Rigidbody2D>().velocity = Vector2.zero;
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
  
  // TODO: this is temporal code for projectile
  #region Fire Projectile Test
  private Coroutine _coFireProjectile;
  private void StartProjectile()
  {
    if(_coFireProjectile != null)
      StopCoroutine(_coFireProjectile);

    _coFireProjectile = StartCoroutine(CoStartProjectile());
  }

  private IEnumerator CoStartProjectile()
  {
    WaitForSeconds wait = new WaitForSeconds(0.5f);
    while (true)
    {
      ProjectileController pc = Managers.Object.Spawn<ProjectileController>(projectilePointer.position, 1);
      pc.SetInfo(1, this, (projectilePointer.position - indicator.position).normalized);
      
      yield return wait;
    }
  }
  #endregion
}
