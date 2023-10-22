using UnityEngine;

public class UI_SkillCardItem : UI_Base
{
  private int _templateID;
  private Data.SkillData _skillData;

  public void SetInfo(int templateID)
  {
    _templateID = templateID;
    Managers.Data.SkillDic.TryGetValue(templateID, out _skillData);
  }

  public void OnClickItem()
  {
    Debug.Log("Skill Card OnClicked");
    Managers.UI.ClosePopup();
  }
}
