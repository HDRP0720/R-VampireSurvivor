using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;

using static Define;

public class ProjectileController : SkillBase
{
  public SkillBase skill;
  public int bounceCount = 1;
  
  private CreatureController _owner;
  private Vector2 _spawnPos;
  private Vector3 _dir = Vector3.zero;
  private Vector3 _target = Vector3.zero;
  private ESkillType _skillType;
  private Rigidbody2D _rigid;
  private int _numPenetrations;
  private GameObject _meteorShadow;
  private List<CreatureController> _enteredColliderList = new List<CreatureController>();
  private Coroutine _coDotDamage;
  private List<Transform> _chainLightningList = new List<Transform> ();
  
  private float _timer = 0;
  private float _rotateAmount = 1000;
  
  public override bool Init()
  {
    if (base.Init() == false)
      return false;

    ObjectType = EObjectType.Projectile;
    return true;
  }

  public void SetInfo(CreatureController owner, Vector2 position, Vector2 dir, Vector2 target, SkillBase skill)
  {
    _owner = owner;
    _spawnPos = position;
    _dir = dir;
    this.skill = skill;
    _rigid = GetComponent<Rigidbody2D>();
    _target = target;
    transform.localScale = Vector3.one * skill.SkillData.scaleMultiplier;
    _numPenetrations = skill.SkillData.numPenetrations;
    bounceCount = skill.SkillData.numBounce;
    switch (skill.SkillType)
    {
      case ESkillType.ChainLightning:
        StartCoroutine(CoChainLightning(_spawnPos, _target, true));
        break;
      case ESkillType.PhotonStrike:
        StartCoroutine(CoPhotonStrike());
        break;
      case ESkillType.Shuriken:
        bounceCount = skill.SkillData.numBounce;
        _rigid.velocity = _dir * Skill.SkillData.ProjSpeed;
        break;
      case ESkillType.ComboShot:
        LaunchComboShot();
        break;
      case ESkillType.WindCutter:
        if (gameObject.activeInHierarchy)
          StartCoroutine(CoWindCutter());
        break;
      case ESkillType.Meteor:
        _dir = (_target - transform.position).normalized;
        transform.rotation = Quaternion.FromToRotation(Vector3.up, _dir);
        _rigid.velocity = _dir * skill.SkillData.projSpeed;
        _meteorShadow = Managers.Resource.Instantiate("MeteorShadow", pooling: true);
        _meteorShadow.transform.position = target;
        if (gameObject.activeInHierarchy)
          StartCoroutine(CoMeteor());
        break;
      case ESkillType.PoisonField:
        if (gameObject.activeInHierarchy)
          StartCoroutine(CoPosionField(skill));
        break;
      case ESkillType.EgoSword:
      case ESkillType.StormBlade:
        StartCoroutine(CoDestroy());
        transform.rotation = Quaternion.FromToRotation(Vector3.up, _dir);
        _rigid.velocity = _dir * skill.SkillData.projSpeed;
        break;
      default:
        transform.rotation = Quaternion.FromToRotation(Vector3.up, _dir);
        _numPenetrations = skill.SkillData.numPenetrations;
        _rigid.velocity = _dir * skill.SkillData.projSpeed;
        break;
    }
    if (gameObject.activeInHierarchy)
      StartCoroutine(CoCheckDestory());
  }

  private IEnumerator CoChainLightning(Vector3 startPos, Vector3 endPos, bool isFollow = false)
  {
    SetParticleSize(startPos, endPos);
    yield return new WaitForSeconds(0.25f);
    DestroyProjectile();
  }
  private void SetParticleSize(Vector3 startPos, Vector3 endPos)
  {
    ParticleSystem particle = GetComponent<ParticleSystem>();
    ParticleSystem childParticle = Utils.FindChild<ParticleSystem>(gameObject);
    var main = particle.main;
    var main2 = childParticle.main;

    // Scale
    transform.position = startPos;
    float dist = Vector3.Distance(startPos, endPos);
    main.startSizeX = main2.startSizeX = dist;
    main.startSizeY = main2.startSizeY = 8;
    
    // rotatate
    Vector3 dir = (endPos - startPos).normalized;
    float angle = Mathf.Atan2(dir.y, dir.x);
    main.startRotation = main2.startRotation = angle * -1f;

    // Cast box
    List<Transform> listMonster = new List<Transform>();
    LayerMask targetLayer = LayerMask.GetMask("Monster", "Boss");
    float boxWidth = 1f;
    Vector3 midPos = (startPos + endPos) / 2f; // 시작점과 끝점 사이의 중간 지점
    Vector2 boxSize = new Vector2(boxWidth, boxWidth);
 
    float angleRad = angle * Mathf.Deg2Rad;

    RaycastHit2D[] colliders = Physics2D.BoxCastAll(midPos, boxSize, 0, dir, dist * 1.3f, targetLayer); 
    foreach (RaycastHit2D hit in colliders)
    {
      MonsterController monster = hit.transform.GetComponent<MonsterController>();
      if (monster != null)
      {
        monster.OnDamaged(_owner, skill);
      }
    }
  }
  private void DestroyProjectile()
  {
    Managers.Object.Despawn(this);
  }
  
  private IEnumerator CoWindCutter()
  {
    Vector3 targePoint = Managers.Game.Player.PlayerCenterPos + _dir * Skill.SkillData.ProjSpeed;
    transform.localScale = Vector3.zero;
    transform.localScale = Vector3.one * Skill.SkillData.ScaleMultiplier;

    Sequence seq = DOTween.Sequence();
    // 1. 목표지점까지 빠르게 도착
    // 2. 도착수 약간 더 전진
    // 3. 되돌아옴

    float projectileTravelTime = 1f; // 발사체가 목표지점까지 가는데 걸리는시간
    float secondSeqStartTime = 0.7f; // 두번쨰 시퀀스 시작시간
    float secondSeqDuringTime = 1.8f; //두번째 시퀀스 유지시간

    seq.Append(transform.DOMove(targePoint, projectileTravelTime).SetEase(Ease.OutExpo))
      .Insert(secondSeqStartTime, transform.DOMove(targePoint + _dir, secondSeqDuringTime).SetEase(Ease.Linear));

    yield return new WaitForSeconds(Skill.SkillData.Duration);

    while (true)
    {
      transform.position = Vector2.MoveTowards(this.transform.position, Managers.Game.Player.PlayerCenterPos, Time.deltaTime * Skill.SkillData.ProjSpeed * 4f);
      if (Managers.Game.Player.PlayerCenterPos == transform.position)
      {
        DestroyProjectile();
        break;
      }
      yield return new WaitForFixedUpdate();
    }
  }

  public override void HandleUpdate()
  {
    base.HandleUpdate();
    transform.position += _moveDir * (_speed * Time.deltaTime);
  }

  private void OnTriggerEnter2D(Collider2D other)
  {
    MonsterController mc = other.gameObject.GetComponent<MonsterController>();
    if (mc.IsValid() == false) return;

    if (this.IsValid() == false) return;
    
    mc.OnDamaged(_owner, SkillData.damage);
    
    StopDestroy();
    
    Managers.Object.Despawn(this);
  }
}