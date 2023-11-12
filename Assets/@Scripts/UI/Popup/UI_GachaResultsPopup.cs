using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Define;

public class UI_GachaResultsPopup : UI_Popup
{
  #region UI Feature List
  // 정보 갱신
  // 팝업 진입 시 ResultsContentScrollObject가 활성화되어있으며 가챠 연출을 하고
  // 스킬 버튼 또는 일정시간이 넘어가면 OpenContentObject가 활성화가되는 구조

  // ResultsContentScrollObject : 가챠로 얻은 장비가 들어갈 부모개체

  // 로컬라이징
  // SkipButtonText : 스킵
  // RewardPopupTitleText : 오픈 결과
  #endregion
  
  #region Enum For Binding UI Automatically
  enum GameObjects
  {
    OpenContentObject,
    ResultsContentObject,
    ResultsContentScrollObject,
    GatchaBoxAni,
  }
  enum Texts
  {
    SkipButtonText,
  }
  enum Buttons
  {
    SkipButton,
    ConfirmButton,
  }
  #endregion

  [SerializeField] private GameObject _particle;
  
  private List<Equipment>_items = new List<Equipment> ();
  private Animator _anim;
  
  private void Awake()
  {
    Init();
  }
  private void OnEnable()
  {
    PopupOpenAnimation(GetObject((int)GameObjects.ResultsContentObject));
  }

  protected override bool Init()
  {
    if (base.Init() == false) return false;
    
    BindObject(typeof(GameObjects));
    BindText(typeof(Texts));
    BindButton(typeof(Buttons));

    GetObject((int)GameObjects.OpenContentObject).gameObject.SetActive(true);
    GetObject((int)GameObjects.ResultsContentObject).gameObject.SetActive(false);

    GetButton((int)Buttons.SkipButton).gameObject.BindEvent(OnClickSkipButton);
    GetButton((int)Buttons.SkipButton).GetOrAddComponent<UI_ButtonAnimation>();
    GetButton((int)Buttons.ConfirmButton).gameObject.BindEvent(OnClickConfirmButton);
    GetButton((int)Buttons.ConfirmButton).GetOrAddComponent<UI_ButtonAnimation>();
    
    AnimationEventDetector ad = GetObject((int)GameObjects.GatchaBoxAni).GetComponent<AnimationEventDetector>();
    ad.OnEvent -= PlayParticle;
    ad.OnEvent += PlayParticle;
    
    Refresh();
    
    var main = _particle.GetComponent<ParticleSystem>().main;
    main.stopAction = ParticleSystemStopAction.Callback;
    Managers.Sound.Play(ESound.Effect, "PopupOpen_GameResult");
    
    return true;
  }

  public void SetInfo(List<Equipment> items)
  {
    _items = items;
    Refresh();
  }

  private void Refresh()
  {

  }

  private void PlayParticle()
  {
    _particle.SetActive(true);
    StartCoroutine(CoSkip());
  }
  private IEnumerator CoSkip()
  {
    yield return new WaitForSeconds(2.5f);
    OnClickSkipButton();
  }
  
  private void OnClickSkipButton()
  {
    Managers.Sound.PlayButtonClick();

    GetObject((int)GameObjects.OpenContentObject).gameObject.SetActive(false);
    GetObject((int)GameObjects.ResultsContentObject).gameObject.SetActive(true);

    GameObject container = GetObject((int)GameObjects.ResultsContentScrollObject);
    container.DestroyChildren();

    foreach (Equipment item in _items)
    {
      UI_EquipItem equipItem = Managers.Resource.Instantiate("UI_EquipItem", pooling: true).GetOrAddComponent<UI_EquipItem>();
      equipItem.transform.SetParent(container.transform);
      equipItem.SetInfo(item, UI_ItemParentType.GachaResultPopup);
    }
  }
  private void OnClickConfirmButton()
  {
    Managers.Sound.PlayPopupClose();
    gameObject.SetActive(false);
  }
}
