using UnityEngine;

public class CameraController : MonoBehaviour
{
  public GameObject target;

  private void LateUpdate()
  {
    if(target == null) return;

    transform.position = new Vector3(target.transform.position.x, target.transform.position.y, -10);
  }
}