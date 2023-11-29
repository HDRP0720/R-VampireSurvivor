using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Data;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class UI_StageSelectPopup : UI_Popup
{
  #region UI Feature List
  // 정보 갱신
  // StageScrollContentObject : UI_ChapterInfoItem이 들어갈 부모 개체
  // AppearingMonsterContentObject : UI_MonsterInfo가 들어갈 부모 개체
  // StageImage : 스테이지의 이미지 (테이블에 추가 필요)
  // StageNameValueText : 스테이지의 이름 (테이블에 추가 필요)
  // StageRewardProgressSliderObject : 스테이지 클리어 시 슬라이더 상승(챕터의 최대 스테이지 수, 1씩 상승)

  // 챕터 클리어 보상 데이터 연결 (기본상태 -> 보상 수령 가능 상태 -> 보상 수령 완료 상태)
  // FirstClearRewardText : 첫번째 보상 조건 
  //      첫번재 보상
  //      - FirstClearRewardItemBackgroundImage : 보상 아이템의 테두리 (색상 변경)
  //      - 일반(Common) : #AC9B83
  //      - 고급(Uncommon)  : #73EC4E
  //      - 희귀(Rare) : #0F84FF
  //      - 유일(Epic) : #B740EA
  //      - 전설(Legendary) : #F19B02
  //      - 신화(Myth) : #FC2302
  //      - FirstClearRewardItemImage : 보상 아이템의 아이콘
  //      - FirstClearRewardItemCountValueText : 보상 아이템의 벨류 (개수)
  //      - FirstClearRewardUnlockObject : 보상 수령 가능할 시 비활성화 (기본 활성화) 
  //      - FirstClearRedDotObject : 보상 수령 가능할 시 활성화 (기본 비활성화) 

  //      두번재 보상
  // SecondClearRewardText : 두번째 보상 조건 
  //      - SecondClearRewardItemBackgroundImage : : 보상 아이템의 테두리 (색상 변경)
  //      - SecondClearRewardItemImage : 보상 아이템의 아이콘
  //      - SecondClearRewardItemCountValueText : 보상 아이템의 벨류 (개수)
  //      - SecondClearRewardUnlockObject : 보상 수령 가능할 시 비활성화 (기본 활성화) 
  //      - SecondClearRedDotObject : 보상 수령 가능할 시 활성화 (기본 비활성화)

  //      세번재 보상
  // ThirdClearRewardText : 세번째 보상 조건 
  //      - ThirdClearRewardItemBackgroundImage : : 보상 아이템의 테두리 (색상 변경)
  //      - ThirdClearRewardItemImage : 보상 아이템의 아이콘
  //      - ThirdClearRewardItemCountValueText : 보상 아이템의 벨류 (개수)
  //      - ThirdClearRewardUnlockObject : 보상 수령 가능할 시 비활성화 (기본 활성화) 
  //      - ThirdClearRedDotObject : 보상 수령 가능할 시 활성화 (기본 비활성화)


  // 로컬라이징
  // StageSelectTitleText : 스테이지
  // AppearingMonsterText : 등장 몬스터
  // StageSelectButtonText : 선택

  #endregion
  
  #region Enum For Binding UI Automatically
  enum GameObjects
  {
    ContentObject,
    StageScrollContentObject,
    AppearingMonsterContentObject,
    StageSelectScrollView,
  }
  enum Buttons
  {
    StageSelectButton,
    BackButton,
  }
  enum Texts
  {
    StageSelectTitleText,
    AppearingMonsterText,
    StageSelectButtonText,
  }
  enum Images
  {
    LArrowImage,
    RArrowImage
  }
  #endregion
  
  private StageData _stageData;
  private HorizontalScrollSnap _scrollSnap;
  
  public Action OnPopupClosed;
  
  private void Awake()
  {
    Init();
  }
  private void OnEnable()
  {
    PopupOpenAnimation(GetObject((int)GameObjects.ContentObject));
  }

  protected override bool Init()
  {
    if (base.Init() == false) return false;
 
    BindObject(typeof(GameObjects));
    BindButton(typeof(Buttons));
    BindText(typeof(Texts));
    BindImage(typeof(Images));

    GetButton((int)Buttons.StageSelectButton).gameObject.SetActive(false);
    GetButton((int)Buttons.StageSelectButton).gameObject.BindEvent(OnClickStageSelectButton);
    GetButton((int)Buttons.StageSelectButton).GetOrAddComponent<UI_ButtonAnimation>();
    GetButton((int)Buttons.BackButton).gameObject.BindEvent(OnClickBackButton);
    GetButton((int)Buttons.BackButton).GetOrAddComponent<UI_ButtonAnimation>();

    _scrollSnap = Utils.FindChild<HorizontalScrollSnap>(gameObject, recursive : true);
    _scrollSnap.OnSelectionPageChangedEvent.AddListener(OnChangeStage);
    _scrollSnap.StartingScreen = Managers.Game.CurrentStageData.stageIndex -1;

    Refresh();
    return true;
  }
  public void SetInfo(StageData stageData)
  {
    _stageData = stageData;
    Refresh();
  }
  private void Refresh()
  {
    if (_init == false) return;

    if (_stageData == null) return;

    GameObject stageContainer = GetObject((int)GameObjects.StageScrollContentObject);
    stageContainer.DestroyChildren();

    _scrollSnap.ChildObjects = new GameObject[Managers.Data.StageDic.Count];
    foreach (StageData stageData in Managers.Data.StageDic.Values)
    {
      UI_StageInfoItem item = Managers.UI.MakeSubItem<UI_StageInfoItem>(stageContainer.transform);
      item.SetInfo(stageData);
      _scrollSnap.ChildObjects[stageData.stageIndex - 1] = item.gameObject;
    }
    
    StageInfoRefresh();
  }
  private void UIRefresh()
  {
    GetImage((int)Images.LArrowImage).gameObject.SetActive(true);
    GetImage((int)Images.RArrowImage).gameObject.SetActive(true);
    GetButton((int)Buttons.StageSelectButton).gameObject.SetActive(false);

    #region 스테이지 화살표
    if (_stageData.stageIndex == 1)
    {
      GetImage((int)Images.LArrowImage).gameObject.SetActive(false);
      GetImage((int)Images.RArrowImage).gameObject.SetActive(true);
    }
    else if (_stageData.stageIndex >= 2 && _stageData.stageIndex < 50)
    {
      GetImage((int)Images.LArrowImage).gameObject.SetActive(true);
      GetImage((int)Images.RArrowImage).gameObject.SetActive(true);
    }
    else if (_stageData.stageIndex == 50)
    {
      GetImage((int)Images.LArrowImage).gameObject.SetActive(true);
      GetImage((int)Images.RArrowImage).gameObject.SetActive(false);
    }
    #endregion

    #region 스테이지 선택 버튼
    if (Managers.Game.DicStageClearInfo.TryGetValue(_stageData.stageIndex, out StageClearInfo info) == false)
      return;
    
    //게임 처음 시작하고 스테이지창을 오픈 한 경우
    if (info.stageIndex == 1 && info.maxWaveIndex == 0)
    {
      GetButton((int)Buttons.StageSelectButton).gameObject.SetActive(true);
    }
    
    // 스테이지 진행중
    if (info.stageIndex <=_stageData.stageIndex)
      GetButton((int)Buttons.StageSelectButton).gameObject.SetActive(true);
    
    // 새로운 스테이지
    if (Managers.Game.DicStageClearInfo.TryGetValue(_stageData.stageIndex - 1, out StageClearInfo PrevInfo) == false)
      return;
    
    if (PrevInfo.isClear == true)
      GetButton((int)Buttons.StageSelectButton).gameObject.SetActive(true);
    else
      GetButton((int)Buttons.StageSelectButton).gameObject.SetActive(false);
    #endregion
  }
  private void StageInfoRefresh()
  {
    UIRefresh();
    List<int> monsterList = _stageData.appearingMonsters.ToList();

    GameObject container = GetObject((int)GameObjects.AppearingMonsterContentObject);
    container.DestroyChildren();
    for (int i = 0; i < monsterList.Count; i++)
    {
      UI_MonsterInfoItem monsterInfoItemUI = Managers.UI.MakeSubItem<UI_MonsterInfoItem>(container.transform);
     
      monsterInfoItemUI.SetInfo(monsterList[i], _stageData.stageLevel, this.transform);
    }
  }
  
  private void OnClickStageSelectButton()
  {
    Managers.Game.CurrentStageData = _stageData;
    OnPopupClosed?.Invoke();
    Managers.UI.ClosePopupUI(this);
  }
  private void OnClickBackButton()
  {
    OnPopupClosed?.Invoke();
    
    Managers.UI.ClosePopupUI(this);
  }
  private void OnChangeStage(int index)
  {
    //현재 스테이지 설정
    _stageData = Managers.Data.StageDic[index + 1];

    int[] monsterData = _stageData.appearingMonsters.ToArray();

    GameObject container = GetObject((int)GameObjects.AppearingMonsterContentObject);
    container.DestroyChildren();

    for (int i = 0; i < monsterData.Length; i++)
    {
      UI_MonsterInfoItem item = Managers.UI.MakeSubItem<UI_MonsterInfoItem>(GetObject((int)GameObjects.AppearingMonsterContentObject).transform);
      item.SetInfo(monsterData[i], _stageData.stageLevel, this.transform);
    }

    UIRefresh();
    LayoutRebuilder.ForceRebuildLayoutImmediate(GetObject((int)GameObjects.AppearingMonsterContentObject).GetComponent<RectTransform>());
  }
}
