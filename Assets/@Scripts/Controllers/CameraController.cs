using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
  #region Variables
  public Transform _playerTransform;
  
  [SerializeField] private float _tickvalue = 5;
  [SerializeField] private float _adjust = 0.5f;
  
  private bool _isShake = false;
  #endregion

  #region Properties
  public float Height { get; set; } = 0;
  public float Width { get; set; } = 0;
  #endregion


  private void Start()
  {
    SetCameraSize();
  }
  private void LateUpdate()
  {
    if (_playerTransform != null && Managers.Game.CurrentMap != null && _isShake == false)
      LimitCameraArea();
  }

  private void SetCameraSize()
  {
    Camera.main.orthographicSize = 21f;
    Height = Camera.main.orthographicSize;
    Width = Height * Screen.width / Screen.height;
  }

  private void LimitCameraArea()
  {
    transform.position = new Vector3(_playerTransform.position.x, _playerTransform.position.y, -10f);
    
    float limitX = Managers.Game.CurrentMap.MapSize.x * 0.5f - Width;
    float clampX = Mathf.Clamp(transform.position.x, -limitX, limitX);

    float limitY = Managers.Game.CurrentMap.MapSize.y * 0.5f - Height;
    float clampY = Mathf.Clamp(transform.position.y, -limitY, limitY);

    transform.position = new Vector3(clampX, clampY, -10f);
  }

  public void ShakeCamera()
  {
    if(_isShake == false)
      StartCoroutine(CoShake(0.25f));
  }

  private IEnumerator CoShake(float duration)
  {
    float halfDuration = duration / 2;
    float elapsed = 0f;
    float tick = Random.Range(-10f, 10f);
    _isShake = true;
    while (elapsed < duration)
    {
      // TODO: Check if this is necessary
      // if (Managers.UI.GetPopupCount() > 0)
      //   break;
      
      elapsed += Time.deltaTime / halfDuration;

      tick += Time.deltaTime * _tickvalue;
      transform.position += new Vector3(
        Mathf.PerlinNoise(tick, 0) - .5f,
        Mathf.PerlinNoise(0, tick) - .5f,
        0f) * (_adjust * Mathf.PingPong(elapsed, halfDuration));
      yield return null;
    }

    _isShake = false;
  }
}