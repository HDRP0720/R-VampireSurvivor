using UnityEngine;
using static Define;

public class UI_ToolTipItem : UI_Base
{
  #region Enum For Binding UI Automatically
  enum Images
  {
    TargetImage,
    BackgroundImage,
  }
  enum Buttons
  {
    CloseButton
  }
  enum Texts
  {
    TargetNameText,
    TargetDescriptionText
  }
  #endregion

  private RectTransform _canvas;
  private Camera _uiCamera;
  
  private void OnEnable()
  {
    GetText((int)Texts.TargetNameText).gameObject.SetActive(false);
    GetText((int)Texts.TargetNameText).gameObject.SetActive(false);
  }
  private void Awake()
  {
    Init();
  }
  
  protected override bool Init()
  {
    if (base.Init() == false) return false;
 
    BindButton(typeof(Buttons));
    BindImage(typeof(Images));
    BindText(typeof(Texts));
    GetButton((int)Buttons.CloseButton).gameObject.BindEvent(OnClickCloseButton);

    Refresh();
    return true;
  }
  
  // Tooltip for support skills
  public void SetInfo(Data.SupportSkillData skillData, RectTransform targetPos, RectTransform parentsCanvas)
  {
    GetImage((int)Images.TargetImage).sprite = Managers.Resource.Load<Sprite>(skillData.iconLabel);
    GetText((int)Texts.TargetNameText).gameObject.SetActive(true);
    GetText((int)Texts.TargetNameText).text = skillData.name;
    GetText((int)Texts.TargetDescriptionText).gameObject.SetActive(true);
    GetText((int)Texts.TargetDescriptionText).text = skillData.description;
    
    // Change colors by skill grade
    switch (skillData.supportSkillGrade)
    {
      case ESupportSkillGrade.Common:
        GetImage((int)Images.BackgroundImage).color = EquipmentUIColors.Common;
        break;
      case ESupportSkillGrade.Uncommon:
        GetImage((int)Images.BackgroundImage).color = EquipmentUIColors.Uncommon;
        break;
      case ESupportSkillGrade.Rare:
        GetImage((int)Images.BackgroundImage).color = EquipmentUIColors.Rare;
        break;
      case ESupportSkillGrade.Epic:
        GetImage((int)Images.BackgroundImage).color = EquipmentUIColors.Epic;
        break;
      case ESupportSkillGrade.Legend:
        GetImage((int)Images.BackgroundImage).color = EquipmentUIColors.Legendary;
        break;
      default:
        break;
    }

    ToolTipPosSet(targetPos, parentsCanvas);
    Refresh();
  }
  
  // Tooltip for materials
  public void SetInfo(Data.MaterialData materialData, RectTransform targetPos, RectTransform parentsCanvas)
  {
    GetImage((int)Images.TargetImage).sprite = Managers.Resource.Load<Sprite>(materialData.SpriteName);
    GetText((int)Texts.TargetNameText).gameObject.SetActive(true); 
    GetText((int)Texts.TargetNameText).text = materialData.NameTextID;
    GetText((int)Texts.TargetDescriptionText).gameObject.SetActive(true); 
    GetText((int)Texts.TargetDescriptionText).text = materialData.DescriptionTextID;
    
    // Change background colors by material grade
    switch (materialData.MaterialGrade)
    {
      case EMaterialGrade.Common:
        GetImage((int)Images.BackgroundImage).color = EquipmentUIColors.Common;
        break;
      case EMaterialGrade.Uncommon:
        GetImage((int)Images.BackgroundImage).color = EquipmentUIColors.Uncommon;
        break;
      case EMaterialGrade.Rare:
        GetImage((int)Images.BackgroundImage).color = EquipmentUIColors.Rare;
        break;
      case EMaterialGrade.Epic:
        GetImage((int)Images.BackgroundImage).color = EquipmentUIColors.Epic;
        break;
      case EMaterialGrade.Legendary:
        GetImage((int)Images.BackgroundImage).color = EquipmentUIColors.Legendary;
        break;
      default:
        break;
    }

    ToolTipPosSet(targetPos, parentsCanvas); // 위치 설정
    Refresh();
  }
  
  // Tooltip for monsters
  public void SetInfo(Data.CreatureData creatureData, RectTransform targetPos, RectTransform parentsCanvas)
  {
    GetImage((int)Images.TargetImage).sprite = Managers.Resource.Load<Sprite>(creatureData.iconLabel);
    GetText((int)Texts.TargetDescriptionText).gameObject.SetActive(true);
    GetText((int)Texts.TargetDescriptionText).text = creatureData.descriptionTextID;
    GetImage((int)Images.BackgroundImage).color = EquipmentUIColors.Common;

    ToolTipPosSet(targetPos, parentsCanvas); // 위치 설정
    Refresh();
  }
  
  private void Refresh()
  {

  }
  private void OnClickCloseButton()
  {
    Managers.Sound.PlayButtonClick();
    Managers.Resource.Destroy(gameObject);
  }
  private void ToolTipPosSet(RectTransform targetPos, RectTransform parentsCanvas)
  {
    gameObject.transform.position = targetPos.transform.position;

    float sizeY = targetPos.sizeDelta.y / 2;
    transform.localPosition += new Vector3(0f, sizeY);
 
    if (targetPos.transform.localPosition.x > 0)
    {
      float canvasMaxX = parentsCanvas.sizeDelta.x / 2;
      float targetPosMaxX = transform.localPosition.x + transform.GetComponent<RectTransform>().sizeDelta.x / 2;
      if (canvasMaxX < targetPosMaxX)
      {
        float deltaX = targetPosMaxX - canvasMaxX;
        transform.localPosition = -new Vector3(deltaX+20, 0f) + transform.localPosition;
      }
    }
    else
    {
      float canvasMinX = -parentsCanvas.sizeDelta.x / 2;
      float targetPosMinX = transform.localPosition.x - transform.GetComponent<RectTransform>().sizeDelta.x / 2;
      if (canvasMinX > targetPosMinX)
      {
        float deltaX = canvasMinX - targetPosMinX;
        transform.localPosition = new Vector3(deltaX+20, 0f) + transform.localPosition;
      }
    }
  }
}
