using UnityEngine;
using UnityEngine.UI;
using Unity.VisualScripting;

using Data;
using static Define;

public class UI_EquipmentPopup : UI_Popup
{
  #region UI Feature List
  // 정보 갱신
  // CharacterImage : 캐릭터 이미지 (애니메이션이라면 변경 필요)
  // AttackValueText : 캐릭터의 최종 공격력 표시 (숫자 변화시 연출 필요)
  // HealthValueText : 캐릭터의 최종 체력 표시 (숫자 변화시 연출 필요)
  // MergeButtonRedDotObject : 합성이 가능하다면 레드닷 출력
  // EquipInventoryObject : 보유하고 있는 장비가 들어갈 부모개체
  // (최대 150개, 인벤토리 여유 공간 없을 시 보상 수령 못하고 인벤토리 경고 팝업 호출)
  // ItemInventoryObject : 보유하고 있는 아이템이 들어갈 부모개체

  // 로컬라이징 텍스트
  // EquipInventoryTlileText : 장비
  // ItemInventoryTlileText : 아이템
  #endregion
  
  #region Enum For Bindigng UI Automatically
  enum GameObjects
  {
    ContentObject,
    WeaponEquipObject, //무기 장착 시 들어갈 부모개체
    GlovesEquipObject, // 장갑 장착 시 들어갈 부모개체
    RingEquipObject, // 반지 장착 시 들어갈 부모개체
    BeltEquipObject, // 헬멧 장착 시 들어갈 부모개체
    ArmorEquipObject, // 갑옷 장착 시 들어갈 부모개체
    BootsEquipObject, // 부츠 장착 시 들어갈 부모개체
    CharacterRedDotObject,
    MergeButtonRedDotObject,
    EquipInventoryObject,
    ItemInventoryObject,
    EquipInventoryGroupObject,
    ItemInventoryGroupObject,
  }
  enum Buttons
  {
    CharacterButton,
    SortButton,
    MergeButton,
  }
  enum Images
  {
    CharacterImage,
  }
  enum Texts
  {
    AttackValueText,
    HealthValueText,
    SortButtonText,
    MergeButtonText,
    EquipInventoryTitleText,
    ItemInventoryTitleText,
  }
  #endregion
  
  [SerializeField] private ScrollRect _scrollRect;
  
  private EEquipmentSortType _equipmentSortType;
  private string _sortText_Level = "정렬 : 레벨";
  private string _sortText_Grade = "정렬 : 등급";
  
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

    GetObject((int)GameObjects.CharacterRedDotObject).gameObject.SetActive(false);
    GetObject((int)GameObjects.MergeButtonRedDotObject).gameObject.SetActive(false);
    GetButton((int)Buttons.CharacterButton).gameObject.BindEvent(OnClickCharacterButton);
    GetButton((int)Buttons.CharacterButton).GetOrAddComponent<UI_ButtonAnimation>();
    GetButton((int)Buttons.CharacterButton).gameObject.SetActive(false); // 출시때 제외

    GetButton((int)Buttons.SortButton).gameObject.BindEvent(OnClickSortButton);
    GetButton((int)Buttons.SortButton).GetOrAddComponent<UI_ButtonAnimation>();
    GetButton((int)Buttons.MergeButton).gameObject.BindEvent(OnClickMergeButton);
    GetButton((int)Buttons.MergeButton).GetOrAddComponent<UI_ButtonAnimation>();

    _equipmentSortType = EEquipmentSortType.Level;
    GetText((int)Texts.SortButtonText).text = _sortText_Level;

    Refresh();
    
    return true;
  }
  
  public void SetInfo()
  {
    Refresh();
  }
  
  private void SortEquipments()
  {
    Managers.Game.SortEquipment(_equipmentSortType);

    GetObject((int)GameObjects.EquipInventoryObject).DestroyChildren();

    foreach (Equipment item in Managers.Game.OwnedEquipments)
    {
      if (item.IsEquipped) continue;

      UI_EquipItem popup = Managers.Resource.Instantiate("UI_EquipItem", GetObject((int)GameObjects.EquipInventoryObject).transform, true).GetOrAddComponent<UI_EquipItem>();

      popup.transform.SetParent(GetObject((int)GameObjects.EquipInventoryObject).transform);
      popup.SetInfo(item, Define.UI_ItemParentType.EquipInventoryGroup, _scrollRect);
    }
  }
  
  public void SetItem()
  {
    GameObject container = GetObject((int)GameObjects.ItemInventoryObject);
    container.DestroyChildren();

    foreach (int id in Managers.Game.ItemDictionary.Keys)
    {
      if (Managers.Data.MaterialDic.TryGetValue(id, out MaterialData material) == true)
      {
        UI_MaterialItem item = Managers.UI.MakeSubItem<UI_MaterialItem>(container.transform);
        int count = Managers.Game.ItemDictionary[id];
                
        item.SetInfo(material, transform, count, _scrollRect);
      }
    }
  }
}
