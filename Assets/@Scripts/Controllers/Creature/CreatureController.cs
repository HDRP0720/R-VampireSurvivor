public class CreatureController : BaseController
{
  protected float _speed = 1.0f;
  
  // Properties
  public int HP { get; set; } = 100;
  public int MaxHP { get; set; } = 100;
  public SkillBook Skills { get; protected set; }

  public override bool Init()
  {
    base.Init();
    
    Skills = gameObject.GetOrAddComponent<SkillBook>();
    
    return true;
  }

  public virtual void OnDamaged(BaseController attacker, int damage)
  {
    if (HP <= 0) return;
    
    HP -= damage;
    if (HP <= 0)
    {
      HP = 0;
      OnDead();
    }
  }

  protected virtual void OnDead() { }
}
