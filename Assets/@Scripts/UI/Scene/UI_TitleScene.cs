using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

using static Define;

public class UI_TitleScene : UI_Scene
{
  #region Enum For Binding UI Automatically
  enum GameObjects
  {
    Slider,
  }
  enum Buttons
  {
    StartButton
  }
  enum Texts
  {
    StartText
  }
  #endregion

  private bool _isPreload = false;
  
  private void Awake()
  {
    Init();
  }
  private void Start()
  {
    Managers.Resource.LoadAllAsync<Object>("Preload", (key, count, totalCount) =>
    {
      GetObject((int)GameObjects.Slider).GetComponent<Slider>().value = (float)count/totalCount;
      if (count == totalCount)
      {
        _isPreload = true;
        GetButton((int)Buttons.StartButton).gameObject.SetActive(true);
        Managers.Data.Init();
        Managers.Game.Init();
        Managers.Time.Init();
        StartButtonAnimation();
      }
    });
  }

  protected override bool Init()
  {
    if (base.Init() == false) return false;

    BindObject(typeof(GameObjects));
    BindButton(typeof(Buttons));
    BindText(typeof(Texts));

    GetObject((int)GameObjects.Slider).GetComponent<Slider>().value = 0;
    GetButton((int)Buttons.StartButton).gameObject.BindEvent(() =>
    {
      if (_isPreload)
        Managers.Scene.LoadScene(EScene.LobbyScene, transform);
    });
    GetButton((int)Buttons.StartButton).gameObject.SetActive(false);
    return true;
  }
  
  private void StartButtonAnimation()
  {
    GetText((int)Texts.StartText).DOFade(0, 1f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutCubic).Play();
  }
}
