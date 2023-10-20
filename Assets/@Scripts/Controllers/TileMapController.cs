using UnityEngine;

public class TileMapController : MonoBehaviour
{
  private void OnTriggerExit2D(Collider2D other)
  {
    Camera mainCamera = other.gameObject.GetComponent<Camera>();
    if (mainCamera == null) return;

    Vector3 dir = mainCamera.transform.position - transform.position;

    float dirX = dir.x < 0 ? -1 : 1;
    float dirY = dir.y < 0 ? -1 : 1;
    
    if(Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
      transform.Translate(Vector3.right * dirX * 200);
    else
      transform.Translate(Vector3.up * dirY * 200);
  }
}
