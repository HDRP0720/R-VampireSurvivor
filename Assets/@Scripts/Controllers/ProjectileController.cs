using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

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
  private GameObject _meteorMagicCircle;
  private List<CreatureController> _enteredColliderList = new List<CreatureController>();
  private Coroutine _coDotDamage;
  
  private float _timer = 0;
  private float _rotateAmount = 1000;
  
  private void OnDisable()
  {
    StopAllCoroutines();
  }
  private void OnCollisionEnter2D(Collision2D other)
  {
    FrozenHeart frozenHeart = other.transform.GetComponent<FrozenHeart>();
    if (frozenHeart != null)
      DestroyProjectile();
  }
  private void OnTriggerEnter2D(Collider2D other)
  {
    MonsterSkill_01 monsterProj = GetComponent<MonsterSkill_01>();
    if (other.transform.parent != null && monsterProj != null)
    { 
      FrozenHeart frozenHeart = other.transform.parent.transform.GetComponent<FrozenHeart>();
      if(frozenHeart != null)
        DestroyProjectile();
    }

    CreatureController creature = other.transform.GetComponent<CreatureController>();
    if (creature.IsValid() == false) return;
 
    if (this.IsValid() == false) return;

    switch (skill.SkillType)
    {
      case ESkillType.IcicleArrow:
      case ESkillType.MonsterSkill_01:
      case ESkillType.SpinShot:
      case ESkillType.CircleShot:
      case ESkillType.PhotonStrike:
        _numPenetrations--;
        if (_numPenetrations < 0)
        {
          _rigid.velocity = Vector3.zero;
          DestroyProjectile();
        }
        break;
      case ESkillType.Shuriken:
      case ESkillType.EnergyBolt:
        bounceCount--;
        BounceProjectile(creature);
        if (bounceCount < 0)
        {
          _rigid.velocity = Vector3.zero;
          DestroyProjectile();
        }
        break;
      case ESkillType.WindCutter:
        _enteredColliderList.Add(creature);
        if (_coDotDamage == null)
          _coDotDamage = StartCoroutine(CoStartDotDamage());
        break;
      default:
        break;
    }
    creature.OnDamaged(_owner, skill);
  }
  private void OnTriggerExit2D(Collider2D other)
  {
    CreatureController target = other.transform.GetComponent<CreatureController>();
    if (target.IsValid() == false) return;

    if (this.IsValid() == false) return;

    _enteredColliderList.Remove(target);

    if (_enteredColliderList.Count == 0 && _coDotDamage != null)
    {
      StopCoroutine(_coDotDamage);
      _coDotDamage = null;
    }
  }

  public override bool Init()
  {
    if (base.Init() == false) return false;

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
        _rigid.velocity = _dir * skill.SkillData.projSpeed;
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
        _meteorMagicCircle = Managers.Resource.Instantiate("MeteorMagicCircle", pooling: true);
        _meteorMagicCircle.transform.position = target;
        if (gameObject.activeInHierarchy)
          StartCoroutine(CoMeteor());
        break;
      case ESkillType.PoisonField:
        if (gameObject.activeInHierarchy)
          StartCoroutine(CoPoisonField(skill));
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
      StartCoroutine(CoCheckDestroy());
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
  
  private IEnumerator CoPhotonStrike()
  {
    List<MonsterController> target = Managers.Object.GetMonsterWithinCamera(1);
    while (true)
    {
      _timer += Time.deltaTime;
      if (_timer > 3 || target == null)
      {
        DestroyProjectile();
        _timer = 0;
        break;
      }

      if (target[0].IsValid() == false) break;

      Vector2 direction = (Vector2)target[0].CenterPosition - _rigid.position;
      float rotateSpeed = Vector3.Cross(direction.normalized, transform.up).z;
      _rigid.angularVelocity = -_rotateAmount * rotateSpeed;
      _rigid.velocity = transform.up * skill.SkillData.projSpeed;
 
      yield return new WaitForFixedUpdate();
    }
  }
  
  private void LaunchComboShot()
  {
    Vector3 targePoint = _owner.CenterPosition + _dir * skill.SkillData.projRange;
    float angle = Mathf.Atan2(_dir.y, _dir.x) * Mathf.Rad2Deg;
    transform.rotation = Quaternion.Euler(0, 0, angle);

    Sequence seq = DOTween.Sequence();
    float duration = skill.SkillData.duration;

    seq.Append(transform.DOMove(targePoint, 0.5f).SetEase(Ease.Linear)).AppendInterval(duration - 0.5f).OnComplete(() =>
    {
      Vector3 targetDir = Managers.Game.Player.CenterPosition - transform.position;
      angle = Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg;
      transform.rotation = Quaternion.Euler(0, 0, angle);
      _rigid.velocity = targetDir.normalized * skill.SkillData.projSpeed;
    });

  }
  
  private IEnumerator CoWindCutter()
  {
    Vector3 targePoint = Managers.Game.Player.PlayerCenterPos + _dir * skill.SkillData.projSpeed;
    transform.localScale = Vector3.zero;
    transform.localScale = Vector3.one * skill.SkillData.scaleMultiplier;

    Sequence seq = DOTween.Sequence();

    float projectileTravelTime = 1f;
    float secondSeqStartTime = 0.7f;
    float secondSeqDuringTime = 1.8f;

    seq.Append(transform.DOMove(targePoint, projectileTravelTime).SetEase(Ease.OutExpo))
      .Insert(secondSeqStartTime, transform.DOMove(targePoint + _dir, secondSeqDuringTime).SetEase(Ease.Linear));

    yield return new WaitForSeconds(skill.SkillData.duration);

    while (true)
    {
      transform.position = Vector2.MoveTowards(this.transform.position, Managers.Game.Player.PlayerCenterPos, Time.deltaTime * skill.SkillData.projSpeed * 4f);
      if (Managers.Game.Player.PlayerCenterPos == transform.position)
      {
        DestroyProjectile();
        break;
      }
      yield return new WaitForFixedUpdate();
    }
  }
  
  private IEnumerator CoMeteor()
  {
    while (true)
    {
      if (_meteorShadow != null)
      {
        Vector2 shadowPosition = _meteorShadow.transform.position;

        float distance = Vector2.Distance(shadowPosition, transform.position);
        float scale = Mathf.Lerp(0f, 2.5f, 1 - distance / 10f);
        _meteorShadow.transform.position = shadowPosition;
        _meteorShadow.transform.localScale = new Vector3(scale, scale, 1f);
      }
      if (Vector2.Distance(_rigid.position, _target) < 0.3f)
        ExplosionMeteor();
      yield return new WaitForFixedUpdate();
    }
  }
  private void ExplosionMeteor()
  {
    Managers.Resource.Destroy(_meteorShadow);
    float scanRange = 1.5f;
    string prefabName = Level == 6 ? "MeteorHitEffect_Final" : "MeteorHitEffect";
    GameObject obj = Managers.Resource.Instantiate(prefabName, pooling : true);
    obj.transform.position = transform.position;

    RaycastHit2D[] targets = Physics2D.CircleCastAll(transform.position, scanRange, Vector2.zero, 0);
    foreach (RaycastHit2D target in targets)
    {
      CreatureController creature = target.transform.GetComponent<CreatureController>();
      if (creature != null && creature.IsMonster())
        creature.OnDamaged(_owner, skill);
    }
    DestroyProjectile();
  }
  
  private IEnumerator CoPoisonField(SkillBase skill)
  {
    while (true)
    {
      transform.position = Vector2.MoveTowards(this.transform.position, _target, Time.deltaTime * skill.SkillData.projSpeed);

      if (transform.position == _target)
      {
        string effectName = skill.Level == 6 ? "PoisonFieldEffect_Final" : "PoisonFieldEffect";
                
        GameObject fireEffect = Managers.Resource.Instantiate(effectName, pooling: true);
        fireEffect.GetComponent<PoisonFieldEffect>().SetInfo(Managers.Game.Player, skill);
        fireEffect.transform.position = _target;
        DestroyProjectile();
      }
      yield return new WaitForFixedUpdate();
    }
  }
  
  private IEnumerator CoDestroy()
  {
    yield return new WaitForSeconds(skill.SkillData.duration);
    DestroyProjectile();
  }
  
  private IEnumerator CoCheckDestroy()
  {
    while (true)
    {
      yield return new WaitForSeconds(5f);
      DestroyProjectile();
    }
  }
  
  private IEnumerator CoStartDotDamage()
  {
    while (true)
    {
      yield return new WaitForSeconds(1f);
      foreach (CreatureController target in _enteredColliderList)
      {
        target.OnDamaged(_owner, skill);
      }
    }
  }
  
  private void BounceProjectile(CreatureController creature)
  {
    List<Transform> list = new List<Transform>();
    list = Managers.Object.GetFindMonstersInFanShape(creature.CenterPosition, _dir, 5.5f, 240);

    List<Transform> sortedList = (from t in list
      orderby Vector3.Distance(t.position, transform.position)                  
      descending select t).ToList(); 

    if (sortedList.Count == 0)
    {
      DestroyProjectile();
    }
    else
    {
      int index = Random.Range(sortedList.Count / 2, sortedList.Count);
      _dir = (sortedList[index].position - transform.position).normalized;
      _rigid.velocity = _dir * skill.SkillData.bounceSpeed;
    }
  }
}