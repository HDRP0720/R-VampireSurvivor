using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using Unity.VisualScripting;

using DG.Tweening;
using Sequence = DG.Tweening.Sequence;

using Data;
using static Define;

public class UI_GameScene : UI_Scene
{
  #region UI Feature List
  // 정보 갱신
  // GoldValueText : 습득 골드 연결 (골드를 먹을때 마다 리프레쉬)
  // KillValueText : 죽인 적 수 연결 (킬 할 때 마다 리프레시) 
  // WaveValueText : 현재 진행중인 웨이브 수
  // TimeLimitValueText : 다음 웨이브 시작 까지 남은 시간 (초마다 갱신)
  // SupportSkillCardListObject : 영혼상점의 서포트 카드가 들어가는 부모개체
  // ResetCostValueText : 영혼 상점의 서포트 카드 리스트 리셋 비용 (-n으로 표기, 리셋마다 150씩 늘어남, 기본 150 )
  // TotalDamageContentObject: UI_SkillDamageItem의 부모개체

  //  BattleSkillCountValueText : 배틀 스킬의 개수 표시
  //  SupportSkillCountValueText : 서포트 스킬의 개수 표시
  // SupportSkillListScrollContentObject : 배운 서포트 스킬들이 들어갈 부모개체

  // 웨이브 알람
  // MonsterAlarmObject : 다음 웨이브가 시작 3초전 3초간 출력 (애니 추가 예정)
  // BossAlarmObject : 보스전 시작 3초전 3초간 출력 (애니 추가 예정)

  // 엘리트 정보
  // EliteInfoObject : 엘리트 등장 시 활성화
  // EliteHpSliderObject : 등장한 엘리트의 현재 체력% 연결
  // EliteNameValueText : 등장한 엘리트의 이름

  // 보스 정보
  // BossInfoObject : 보스 등장 시 활성화
  // BossHpSliderObject : 등장한 보스의 현재 체력% 연결
  // BossNameValueText : 등장한 보스의 이름


  // 로컬라이징
  // CardListResetText : 초기화
  // MonsterCommentText : 몬스터가 몰려옵니다
  // BossCommentText : 보스 등장
  // OwnBattleSkillInfoText : 전투 스킬
  // OwnSupportSkillInfoText : 서포트 스킬
  #endregion
  
  #region Enum for Binding UI Automatically
  enum GameObjects
  {
    ExpSliderObject, // 슬라이더로 변경 필요
    WaveObject,
    SoulImage,
    OnDamaged,
    SoulShopObject,
    SupportSkillCardListObject,
    OwnBattleSkillInfoObject,
    //TotalDamageContentObject,
    SupportSkillListScrollObject,
    SupportSkillListScrollContentObject,
    WhiteFlash,
    MonsterAlarmObject,
    BossAlarmObject,

    EliteInfoObject,
    EliteHpSliderObject,
    BossInfoObject,
    BossHpSliderObject,

    BattleSkillSlotGroupObject,
  }
  enum Buttons
  {
    PauseButton,
    DevelopButton,
    SoulShopButton,
    CardListResetButton,
    SoulShopCloseButton,
    TotalDamageButton,
    SupportSkillListButton,
    SoulShopLeadButton,
    SoulShopBackgroundButton,
  }
  enum Texts
  {
    WaveValueText,
    TimeLimitValueText,
    SoulValueText,
    KillValueText,
    CharacterLevelValueText,
    CardListResetText,
    ResetCostValueText,
    //BattleSkillCountValueText,
    SupportSkillCountValueText,

    EliteNameValueText,
    BossNameValueText,

    MonsterCommentText,
    BossCommentText,
  }
  enum Images
  {
    BattleSkilI_Icon_0,
    BattleSkilI_Icon_1,
    BattleSkilI_Icon_2,
    BattleSkilI_Icon_3,
    BattleSkilI_Icon_4,
    BattleSkilI_Icon_5,
    SupportSkilI_Icon_0,
    SupportSkilI_Icon_1,
    SupportSkilI_Icon_2,
    SupportSkilI_Icon_3,
    SupportSkilI_Icon_4,
    SupportSkilI_Icon_5,
  }
  enum AlramType
  {
    wave,
    boss
  }
  #endregion
 
  private GameManager _game;
  private Coroutine _coWaveAlarm;
  private bool _isSupportSkillListButton = false;
  
  private void Awake()
  {
    Init();
    Managers.Game.Player.Skills.OnUpdateSkillUI += HandleOnLevelUpSkill;
    Managers.Game.Player.OnPlayerMove += HandleOnPlayerMove;
    Refresh();
  }
  private void OnDestroy()
  {
    if (Managers.Game.Player != null)
    {
      Managers.Game.Player.Skills.OnUpdateSkillUI -= HandleOnLevelUpSkill;
      Managers.Game.Player.OnPlayerMove -= HandleOnPlayerMove;
    }
  }
  
  protected override bool Init()
  {
    if (base.Init() == false) return false;

    #region Object Bind
    BindObject(typeof(GameObjects));
    BindButton(typeof(Buttons));
    BindText(typeof(Texts));
    BindImage(typeof(Images));

    GetButton((int)Buttons.PauseButton).gameObject.BindEvent(OnClickPauseButton);
    GetButton((int)Buttons.PauseButton).GetOrAddComponent<UI_ButtonAnimation>();
    GetButton((int)Buttons.SoulShopButton).gameObject.BindEvent(OnClickSoulShopButton);
    GetButton((int)Buttons.CardListResetButton).gameObject.BindEvent(OnClickCardListResetButton);
    GetButton((int)Buttons.CardListResetButton).GetOrAddComponent<UI_ButtonAnimation>();
    GetButton((int)Buttons.SoulShopLeadButton).gameObject.BindEvent(OnClickSoulShopButton);
    GetButton((int)Buttons.SoulShopLeadButton).GetOrAddComponent<UI_ButtonAnimation>();
    GetButton((int)Buttons.SoulShopCloseButton).gameObject.BindEvent(OnClickSoulShopCloseButton);
    GetButton((int)Buttons.SoulShopCloseButton).GetOrAddComponent<UI_ButtonAnimation>();
    GetButton((int)Buttons.TotalDamageButton).gameObject.BindEvent(OnClickTotalDamageButton);
    GetButton((int)Buttons.TotalDamageButton).GetOrAddComponent<UI_ButtonAnimation>();
    GetButton((int)Buttons.SupportSkillListButton).gameObject.BindEvent(OnClickSupportSkillListButton);
    GetButton((int)Buttons.SupportSkillListButton).GetOrAddComponent<UI_ButtonAnimation>();
    GetButton((int)Buttons.SoulShopCloseButton).gameObject.SetActive(false); // 영혼 상점 버튼 초기 상태
    GetButton((int)Buttons.SoulShopLeadButton).gameObject.SetActive(true); // 영혼 상점 버튼 초기 상태
    GetButton((int)Buttons.SoulShopBackgroundButton).gameObject.BindEvent(OnClickSoulShopCloseButton);
    GetButton((int)Buttons.SoulShopBackgroundButton).gameObject.SetActive(false); // 영혼 상점 배경
    GetObject((int)GameObjects.OwnBattleSkillInfoObject).gameObject.SetActive(false); // 배틀 스킬 리스트 초기 상태
    GetObject((int)GameObjects.SupportSkillListScrollObject).gameObject.SetActive(false); // 서포트 스킬 리스트 초기 상태
    _isSupportSkillListButton = false;

    GetObject((int)GameObjects.MonsterAlarmObject).gameObject.SetActive(false); // 몬스터 웨이브 알람 초기 상태
    GetObject((int)GameObjects.BossAlarmObject).gameObject.SetActive(false); // 보스 알람 초기 상태

    GetObject((int)GameObjects.EliteInfoObject).gameObject.SetActive(false); // 엘리트 정보(체력바) 초기 상태
    GetObject((int)GameObjects.BossInfoObject).gameObject.SetActive(false); // 보스 정보(체력바) 초기 상태
    #endregion

    _game = Managers.Game;

    Managers.Game.Player.OnPlayerDataUpdated = HandleOnPlayerDataUpdated;
    Managers.Game.Player.OnPlayerLevelUp = HandleOnPlayerLevelUp;
    Managers.Game.Player.OnPlayerDamaged = HandleOnDamaged;

    GetComponent<Canvas>().worldCamera = Camera.main; 

    return true;
  }

  #region Functions For Action
  private void HandleOnPlayerDataUpdated()
  {
    GetObject((int)GameObjects.ExpSliderObject).GetComponent<Slider>().value = _game.Player.ExpRatio;
    GetText((int)Texts.KillValueText).text = $"{_game.Player.KillCount}";
    GetText((int)Texts.SoulValueText).text = _game.Player.SoulCount.ToString();
  }
  private void HandleOnPlayerLevelUp()
  {
    if (Managers.Game.isGameEnd) return;

    List<SkillBase> list = Managers.Game.Player.Skills.RecommendSkills();

    if (list.Count > 0)
    {
      Managers.UI.ShowPopupUI<UI_SkillSelectPopup>();
    }

    //GetObject((int)GameObjects.ExpSliderObject).GetComponent<Slider>().value = (float)_game.Player.Exp / _game.Player.TotalExp;
    GetObject((int)GameObjects.ExpSliderObject).GetComponent<Slider>().value = _game.Player.ExpRatio;
    GetText((int)Texts.CharacterLevelValueText).text = $"{_game.Player.Level}";
  }
  private void HandleOnDamaged()
  {
    StartCoroutine(CoBloodScreen());
  }
  private void HandleOnLevelUpSkill()
  {
    ClearSkillSlot();
   
    List<SkillBase> activeSkills = Managers.Game.Player.Skills.SkillList.Where(skill => skill.IsLearnedSkill).ToList();
    for (int i = 0; i < activeSkills.Count; i++)
      AddSkillSlot(i, activeSkills[i].SkillData.iconLabel);

    // Support skill starts at 6th Enum
    int index = 6;
    int count = Mathf.Min(6, Managers.Game.Player.Skills.supportSkills.Count);
    for (int i = 0; i < count; i++)
      AddSkillSlot(i + index, Managers.Game.Player.Skills.supportSkills[i].iconLabel);

    GetText((int)Texts.SupportSkillCountValueText).text = Managers.Game.Player.Skills.supportSkills.Count.ToString();
  }
  private void HandleOnPlayerMove()
  {
    Vector2 uiPos = GetObject((int)GameObjects.SoulImage).transform.position;
    Managers.Game.SoulDestination = uiPos;
  }
  #endregion
  
  private void Refresh() // 데이터 갱신
  {
    // 보유 스킬 정보 갱신
    SetBattleSkill();
    GetObject((int)GameObjects.ExpSliderObject).GetComponent<Slider>().value = _game.Player.ExpRatio;
    GetText((int)Texts.CharacterLevelValueText).text = $"{_game.Player.Level}";
    GetText((int)Texts.KillValueText).text = $"{_game.Player.KillCount}";
    GetText((int)Texts.SoulValueText).text = _game.Player.SoulCount.ToString();
    // 웨이브 리프레시 버그 대응
    LayoutRebuilder.ForceRebuildLayoutImmediate(GetObject((int)GameObjects.WaveObject).GetComponent<RectTransform>());
  }

  public void DoWhiteFlash()
  {
    StartCoroutine(CoWhiteScreen());
  }
  
  private void SetBattleSkill()
  {
    GameObject container = GetObject((int)GameObjects.BattleSkillSlotGroupObject);
  
    foreach (Transform child in container.transform)
      Managers.Resource.Destroy(child.gameObject);

    foreach (SkillBase skill in Managers.Game.Player.Skills.ActivatedSkills)
    {
      UI_SkillSlotItem item = Managers.UI.MakeSubItem<UI_SkillSlotItem>(container.transform);
      item.GetComponent<UI_SkillSlotItem>().SetUI(skill.SkillData.iconLabel, skill.Level);
    }
  }
  
  private void OnClickPauseButton()
  {
    Managers.Sound.PlayButtonClick();
    Managers.UI.ShowPopupUI<UI_PausePopup>();
  }
  private void OnClickSoulShopButton()
  {
    Managers.Sound.Play(ESound.Effect, "PopupOpen_SoulShop");
    //상점 활성화
    SetActiveSoulShop(true);

    if (Managers.Game.ContinueInfo.soulShopList.Count == 0)
      ResetSupportCard();
    else
      LoadSupportCard();

    Refresh();
  }
  private void OnClickCardListResetButton()
  {
    Managers.Sound.PlayButtonClick();
    // 서포트 카드 리스트 내용 리셋
    int cardListResetCost = (int)SOUL_SHOP_COST_PROB[6];
    if (Managers.Game.Player.SoulCount >= cardListResetCost)
    {
      Managers.Game.Player.SoulCount -= cardListResetCost;
      ResetSupportCard();
    }   
  }
  private void OnClickSupportSkillListButton() // 서포트 스킬 리스트 버튼
  {
    Managers.Sound.PlayButtonClick();
    if (_isSupportSkillListButton) // 이미 눌렀다면 닫기
      SoulShopInit(); // 영혼 상점 초기 상태
    else // 누르지 않았다면 서포트 스킬 리스트 열기
    {
      // 서포트 스킬 리스트 상태
      GetObject((int)GameObjects.SoulShopObject).GetComponent<RectTransform>().pivot = new Vector2(0.5f, 1); 
      GetObject((int)GameObjects.SoulShopObject).GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0); 
      GetObject((int)GameObjects.SoulShopObject).GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0); 
      GetObject((int)GameObjects.SoulShopObject).GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 1120); 

      PopupOpenAnimation(GetObject((int)GameObjects.SoulShopObject));
      GetObject((int)GameObjects.OwnBattleSkillInfoObject).gameObject.SetActive(false); // 배틀 스킬 리스트 비활성화
      GetObject((int)GameObjects.SupportSkillListScrollObject).gameObject.SetActive(true); // 서포트 스킬 리스트 활성화
      _isSupportSkillListButton = true;

      ResetSupportSkillList();
    }
  }
  private void OnClickSoulShopCloseButton()
  {
    Managers.Sound.PlayButtonClick();

    SetActiveSoulShop(false);
  }
  private void OnClickTotalDamageButton() // 토탈 데미지 버튼
  {
    Managers.Sound.PlayButtonClick();
    Managers.UI.ShowPopupUI<UI_TotalDamagePopup>().SetInfo();
  }
  
  private void SoulShopInit() // 영혼 상점 초기 상태
  {
    // 영혼 상점 플로팅
    GetObject((int)GameObjects.SoulShopObject).GetComponent<RectTransform>().pivot = new Vector2(0.5f, 1); 
    GetObject((int)GameObjects.SoulShopObject).GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0); 
    GetObject((int)GameObjects.SoulShopObject).GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0); 
    GetObject((int)GameObjects.SoulShopObject).GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 700); 

    PopupOpenAnimation(GetObject((int)GameObjects.SoulShopObject));
    GetButton((int)Buttons.SoulShopCloseButton).gameObject.SetActive(true); // 영혼 상점 버튼 활성화
    GetButton((int)Buttons.SoulShopLeadButton).gameObject.SetActive(false);
    GetButton((int)Buttons.SoulShopBackgroundButton).gameObject.SetActive(true); // 영혼 상점 배경
    GetObject((int)GameObjects.OwnBattleSkillInfoObject).gameObject.SetActive(true); // 배틀 스킬 리스트 활성화
    PopupOpenAnimation(GetObject((int)GameObjects.OwnBattleSkillInfoObject));
    GetObject((int)GameObjects.SupportSkillListScrollObject).gameObject.SetActive(false); // 서포트 스킬 리스트 비활성화
    _isSupportSkillListButton = false;
    Managers.UI.IsActiveSoulShop = true;
  }
  private void ResetSupportSkillList()
  {
    GameObject container = GetObject((int)GameObjects.SupportSkillListScrollContentObject);
    container.DestroyChildren();

    List<Data.SupportSkillData> temp = Managers.Game.Player.Skills.supportSkills.OrderByDescending(x => x.dataId).ToList();
    foreach (Data.SupportSkillData skill in temp)
    {
      UI_SupportSkillItem item = Managers.UI.MakeSubItem<UI_SupportSkillItem>(container.transform);
      ScrollRect scrollRect = GetObject((int)GameObjects.SupportSkillListScrollObject).GetComponent<ScrollRect>();
      item.SeteInfo(skill, this.transform, scrollRect);
      Managers.Game.SoulShopList.Add(skill);
    }
  }
  
  private void ResetSupportCard()
  {
    GameObject supportSkillContainer = GetObject((int)GameObjects.SupportSkillCardListObject);
    supportSkillContainer.DestroyChildren();
    Managers.Game.SoulShopList.Clear();

    foreach (SupportSkillData supportSkill in Managers.Game.Player.Skills.RecommendSupportkills())
    {
      supportSkill.isPurchased = false;
      UI_SupportCardItem skillData = Managers.UI.MakeSubItem<UI_SupportCardItem>(supportSkillContainer.transform);
      skillData.SetInfo(supportSkill);
    }
  }

  private void LoadSupportCard()
  {
    GameObject supportSkillContainer = GetObject((int)GameObjects.SupportSkillCardListObject);
    supportSkillContainer.DestroyChildren();

    foreach (SupportSkillData supportSkill in Managers.Game.SoulShopList)
    {
      UI_SupportCardItem skillData = Managers.UI.MakeSubItem<UI_SupportCardItem>(supportSkillContainer.transform);
      skillData.SetInfo(supportSkill);
    }
  }
  
  private void AddSkillSlot(int index, string iconLabel)
  {
    GetImage(index).sprite = Managers.Resource.Load<Sprite>(iconLabel);
    GetImage(index).enabled = true;
  }
  private void ClearSkillSlot()
  {
    int count = Enum.GetValues(typeof(Images)).Length;
    for (int i = 0; i < count; i++)
    {
      GetImage(i).enabled = false;
    }
  }
  private void SetActiveSoulShop(bool isActive)
  {
    if (isActive)
    { 
      SoulShopInit(); // 영혼 상점 초기 상태
      GetComponent<Canvas>().sortingOrder = UI_GAMESCENE_SORT_OPEN;
    }
    else
    {
      GetComponent<Canvas>().sortingOrder = UI_GAMESCENE_SORT_CLOSED;

      // 영혼 상점 초기 상태
      GetObject((int)GameObjects.SoulShopObject).GetComponent<RectTransform>().pivot = new Vector2(0.5f, 1); 
      GetObject((int)GameObjects.SoulShopObject).GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0); 
      GetObject((int)GameObjects.SoulShopObject).GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0); 
      GetObject((int)GameObjects.SoulShopObject).GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0); 

      PopupOpenAnimation(GetObject((int)GameObjects.SoulShopObject));
      GetButton((int)Buttons.SoulShopCloseButton).gameObject.SetActive(false); // 영혼 상점 버튼 비활성화
      GetButton((int)Buttons.SoulShopLeadButton).gameObject.SetActive(true);
      GetButton((int)Buttons.SoulShopBackgroundButton).gameObject.SetActive(false); // 영혼 상점 배경
      GetObject((int)GameObjects.OwnBattleSkillInfoObject).gameObject.SetActive(false); // 배틀 스킬 리스트 비활성화
      GetObject((int)GameObjects.SupportSkillListScrollObject).gameObject.SetActive(false); // 서포트 스킬 리스트 비활성화
      
      _isSupportSkillListButton = false;
      Managers.UI.IsActiveSoulShop = false;
    }
  }
  
  private IEnumerator CoBloodScreen()
  {
    Color targetColor = new Color(1, 0, 0, 0.3f);

    yield return null;
    Sequence seq = DOTween.Sequence();

    seq.Append(GetObject((int)GameObjects.OnDamaged).GetComponent<Image>().DOColor(targetColor, 0.3f))
      .Append(GetObject((int)GameObjects.OnDamaged).GetComponent<Image>().DOColor(Color.clear, 0.3f)).OnComplete(() => { });
  }
  private IEnumerator CoWhiteScreen()
  {
    Color targetColor = new Color(1, 1, 1, 1f);

    yield return null;
    Sequence seq = DOTween.Sequence();

    seq.Append(GetObject((int)GameObjects.WhiteFlash).GetComponent<Image>().DOFade(1, 0.1f))
      .Append(GetObject((int)GameObjects.WhiteFlash).GetComponent<Image>().DOFade(0, 0.2f)).OnComplete(() => { });
  }
  private IEnumerator SwitchAlarm(AlramType type)
  {
    switch (type)
    {
      case AlramType.wave:
        Managers.Sound.Play(ESound.Effect, "Warning_Wave");
        GetObject((int)GameObjects.MonsterAlarmObject).gameObject.SetActive(true);
        yield return new WaitForSeconds(3f);
        GetObject((int)GameObjects.MonsterAlarmObject).gameObject.SetActive(false);
        break;
      case AlramType.boss:
        Managers.Sound.Play(ESound.Effect, "Warning_Boss");
        GetObject((int)GameObjects.BossAlarmObject).gameObject.SetActive(true);
        yield return new WaitForSeconds(3f);
        GetObject((int)GameObjects.BossAlarmObject).gameObject.SetActive(false);
        break;
    }
  }
}
