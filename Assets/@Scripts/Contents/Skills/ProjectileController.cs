using UnityEngine;

public class ProjectileController : SkillController
{
  private CreatureController _owner;
  private Vector3 _moveDir;
  private float _speed = 10.0f;
  private float _lifeTime = 10.0f;

  public override bool Init()
  {
    base.Init();
    
    StartDestroy(_lifeTime);

    return true;
  }

  public void SetInfo(int templateID, CreatureController owner, Vector3 moveDir)
  {
    if (Managers.Data.SkillDic.TryGetValue(templateID, out Data.SkillData data) == false)
    {
      Debug.LogError("ProjectileController SetInfo Failed!!");
      return;
    }
    
    SkillData = data;
    _owner = owner;
    _moveDir = moveDir;
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