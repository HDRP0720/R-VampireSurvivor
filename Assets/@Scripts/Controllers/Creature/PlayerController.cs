using UnityEngine;

public class PlayerController : CreatureController
{
  [SerializeField] private Transform indicator;
  [SerializeField] private Transform projectilePoint;
  private Vector2 _moveDir = Vector2.zero;

  // Property
  public Transform Indicator { get => indicator; }
  public Vector3 ProjectilePoint { get => projectilePoint.position; }
  public Vector2 MoveDir
  {
    get => _moveDir;
    set => _moveDir = value.normalized;
  }
  public Vector3 ShootDir { get => (projectilePoint.position - indicator.position).normalized; }
  private float EnvCollectDist { get; set; } = 1.0f;

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
    
    // TODO:
    Skills.AddSkill<FireballSkill>(transform.position);
    Skills.AddSkill<EgoSword>(indicator.position);
    
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
    
    var findGems = GameObject.Find("Grid").GetComponent<GridController>()
      .GatherObjects(transform.position, EnvCollectDist + 0.5f);

    foreach (GameObject go in findGems)
    {
      GemController gem = go.GetComponent<GemController>();
      Vector3 dir = gem.transform.position - transform.position;
      if (dir.sqrMagnitude <= sqrCollectDist)
      {
        Managers.Game.Gem += 1;
        Managers.Object.Despawn(gem);
      }
    }
  }
  
  private void OnCollisionEnter2D(Collision2D other)
  {
    MonsterController target = other.gameObject.GetComponent<MonsterController>();
    if (target == null) return;
    
    
  }

  public override void OnDamaged(BaseController attacker, int damage)
  {
    base.OnDamaged(attacker, damage);
    
    // TODO: This is temp code
    CreatureController cc = attacker as CreatureController;
    cc?.OnDamaged(this, 10000);
  }

  private void HandleOnMoveDirChanged(Vector2 dir)
  {
    _moveDir = dir;
  }
}
