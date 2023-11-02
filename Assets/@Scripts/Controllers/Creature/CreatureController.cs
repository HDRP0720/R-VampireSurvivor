using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

using Data;
using static Define;

public class CreatureController : BaseController
{
  public CreatureData creatureData;
  public Material defaultMat;
  public Material hitEffectmat;
    
  [SerializeField] protected SpriteRenderer creatureSprite;
  [SerializeField] protected bool isPlayDamagedAnim = false;
    
  protected string spriteName;
    
  private Collider2D _offset;
  private ECreatureState _creatureState = ECreatureState.Moving;

  #region Properties
  public Rigidbody2D Rb { get; set; }
  public Animator Anim { get; set; }
  public Vector3 CenterPosition => _offset.bounds.center;
  public float ColliderRadius { get; set; }
  public virtual int DataId { get; set; }
  public virtual float Hp { get; set; }
  public virtual float MaxHp { get; set; }
  public virtual float MaxHpBonusRate { get; set; } = 1;
  public virtual float HealBonusRate { get; set; } = 1;
  public virtual float HpRegen { get; set; }
  public virtual float Atk { get; set; }
  public virtual float AttackRate { get; set; } = 1;
  public virtual float Def { get; set; }
  public virtual float DefRate { get; set; }
  public virtual float CriRate { get; set; }
  public virtual float CriDamage { get; set; } = 1.5f;
  public virtual float DamageReduction { get; set; }
  public virtual float MoveSpeedRate { get; set; } = 1;
  public virtual float MoveSpeed { get; set; }
  public virtual SkillBook Skills { get; set; }
  public virtual ECreatureState CreatureState
  {
    get => _creatureState;
    set
    {
      _creatureState = value;
      UpdateAnimation();
    }
  }
  #endregion
  
  private void Awake()
  {
    Init();
  }
  
  public override bool Init()
  {
    base.Init();
    Skills = gameObject.GetOrAddComponent<SkillBook>();
    Rb = GetComponent<Rigidbody2D>();
    _offset = GetComponent<Collider2D>();
    ColliderRadius = GetComponent<CircleCollider2D>().radius;

    creatureSprite = GetComponent<SpriteRenderer>();
    if (creatureSprite == null)
      creatureSprite = Utils.FindChild<SpriteRenderer>(gameObject);

    Anim = GetComponent<Animator>();
    if (Anim == null)
      Anim = Utils.FindChild<Animator>(gameObject);

    return true;
  }

  protected virtual void UpdateAnimation() { }
  protected virtual void UpdateIdle() { }
  protected virtual void UpdateSkill() { }
  protected virtual void UpdateMoving() { }
  protected virtual void UpdateDead() { }
  
  

  public virtual void OnDamaged(BaseController attacker, int damage)
  {
    bool isCritical = false;
    PlayerController player = attacker as PlayerController;
    if (player != null)
    {
      //크리티컬 적용
      if (Random.value <= player.CriRate)
      {
        damage = damage * player.CriDamage;
        isCritical = true;
      }
    }

    if (skill)
      skill.TotalDamage += damage;
    Hp -= damage;
    Managers.Object.ShowDamageFont(CenterPosition, damage, 0, transform, isCritical);

    if (gameObject.IsValid() || this.IsValid())
      StartCoroutine(PlayDamageAnimation());
  }

  protected virtual void OnDead() { }
}
