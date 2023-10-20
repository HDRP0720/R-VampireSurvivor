using UnityEngine;

public class EgoSwordChild : MonoBehaviour
{
  private BaseController _owner;
  private int _damage;

  public void SetInfo(BaseController owner, int damage)
  {
    _owner = owner;
    _damage = damage;
  }

  private void OnTriggerEnter2D(Collider2D other)
  {
    MonsterController mc = other.transform.GetComponent<MonsterController>();
    if (mc.IsValid() == false) return;
    
    mc.OnDamaged(_owner, _damage);
  }
}
