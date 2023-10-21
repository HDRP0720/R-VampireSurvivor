using System.Collections.Generic;
using UnityEngine;

public class UI_SkillSelectPopup : UI_Base
{
  [SerializeField] private Transform skillCardSelectListParent;
  
  private List<UI_SkillCardItem> _items = new List<UI_SkillCardItem>();

  private void start()
  {
    PopulateGrid();
  }

  private void PopulateGrid()
  {
    foreach (Transform t in skillCardSelectListParent.transform)
      Managers.Resource.Destroy(t.gameObject);

    for (int i = 0; i < 3; i++)
    {
      var go = Managers.Resource.Instantiate("UI_SkillCardItem.prefab", pooling: false);
      UI_SkillCardItem item = go.GetOrAddComponent<UI_SkillCardItem>();
    }
  }
}
