using UnityEngine;

public class BaseController : MonoBehaviour
{
  private bool _init = false;
  
  // Property
  public Define.EObjectType ObjectType { get; protected set; }

  private void Awake()
  {
    Init();
  }
  
  public virtual bool Init()
  {
    if (_init) return false;

    _init = true;
    return true;
  }
}
