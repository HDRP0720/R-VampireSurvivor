using DG.Tweening;
using UnityEngine;

public class Map : MonoBehaviour
{
  public SpriteRenderer spriteBackground;
  public SpriteRenderer spritePattern;
  public BoxCollider2D[] borderCollider;  // left right top bottom
  public GridController grid;
  public GameObject demarcation;
  
  public Color backgroundColor;
  public Color patternColor;
  
  public Vector2 MapSize
  {
    get { return spriteBackground.size; }
    set { spriteBackground.size = value;}
  }

  public void Init()
  {
    Managers.Game.CurrentMap = this;
  }

  public void ChangeMapSize(float targetRate, float time = 120)
  {
    Vector3 currentSize = Vector3.one * 20f;
    if(Managers.Game.CurrentWaveIndex > 7) return;
    
    demarcation.transform.DOScale(currentSize * (10 - Managers.Game.CurrentWaveIndex) * 0.1f, 3);
  }
}
