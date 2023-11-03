using System.Collections;
using UnityEngine;

using DG.Tweening;

using static Define;

public class GemController : DropItemController
{
  private GemInfo _gemInfo;
  private Coroutine _coMoveToPlayer;
  
  public override bool Init()
  {
    itemType = EObjectType.Gem;
    base.Init();
    
    return true;
  }
  
  public override void OnDisable()
  {
    base.OnDisable();

    if (_coMoveToPlayer != null)
    {
      StopCoroutine(_coMoveToPlayer);
      _coMoveToPlayer = null;
    }
  }
  
  public void SetInfo(GemInfo gemInfo)
  {
    _gemInfo = gemInfo;
    Sprite spr = Managers.Resource.Load<Sprite>($"{_gemInfo.spriteName}");
    GetComponent<SpriteRenderer>().sprite = spr;
    transform.localScale = _gemInfo.gemScale;
  }
  
  public override void GetItem()
  {
    base.GetItem();
    if (_coMoveToPlayer == null && this.IsValid())
    {           
      Sequence seq = DOTween.Sequence();
      Vector3 dir = (transform.position - Managers.Game.Player.PlayerCenterPos).normalized;
      Vector3 target = gameObject.transform.position + dir * 1.5f;
      seq.Append(transform.DOMove(target, 0.3f).SetEase(Ease.Linear)).OnComplete(() =>
      {
        _coMoveToPlayer = StartCoroutine(CoMoveToPlayer());
      });
    }
  }

  private IEnumerator CoMoveToPlayer()
  {
    while (this.IsValid() == true)
    {
      float dist = Vector3.Distance(gameObject.transform.position, Managers.Game.Player.PlayerCenterPos);

      transform.position = Vector3.MoveTowards(transform.position, Managers.Game.Player.PlayerCenterPos, Time.deltaTime * 30.0f);

      if (dist < 0.4f)
      {
        string soundName = UnityEngine.Random.value > 0.5 ? "ExpGet_01" : "ExpGet_02";
        Managers.Sound.Play(ESound.Effect, soundName);
        Managers.Game.Player.Exp += _gemInfo.expAmount * Managers.Game.Player.ExpBonusRate;
        Managers.Object.Despawn(this);
        yield break;
      }

      yield return new WaitForFixedUpdate();
    }
  }
}

public class GemInfo
{
  public enum EGemType { Small, Green, Blue, Yellow }
  
  public EGemType type;
  public string spriteName;
  public Vector3 gemScale;
  public int expAmount;
  
  // Constructor
  public GemInfo(EGemType type, Vector3 gemScale)
  {
    this.type = type;
    spriteName = $"{type}Gem.sprite";
    this.gemScale = gemScale;
    switch (type)
    {
      case EGemType.Small:
        expAmount = SMALL_EXP_AMOUNT;
        break;
      case EGemType.Green:
        expAmount = GREEN_EXP_AMOUNT;
        break;
      case EGemType.Blue:
        expAmount = BLUE_EXP_AMOUNT;
        break;
      case EGemType.Yellow:
        expAmount = YELLOW_EXP_AMOUNT;
        break;
    }
  }
}
