using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
  #region Variables
  public Data.CreatureData data;
  public bool isCurrentCharacter = false;
  #endregion

  #region Properties
  public int DataId { get; set; } = 1;
  public int Level { get; set; } = 1;
  public int MaxHp { get; set; } = 1;
  public int Atk { get; set; } = 1;
  public int Def { get; set; } = 1;
  public int TotalExp { get; set; } = 1;
  public float MoveSpeed { get; set; } = 1;
  #endregion

  public void SetInfo(int key)
  {
    DataId = key;
    data = Managers.Data.CreatureDic[key];
    MaxHp = (int)((Data.MaxHp + Level * Data.MaxHpBonus) * Data.HpRate);
    Atk = (int)(Data.Atk + (Level * Data.AtkBonus) * Data.AtkRate);
    Def = (int)Data.Def;
    MoveSpeed = Data.MoveSpeed * Data.MoveSpeedRate;
  }

  public void LevelUp()
  {
    Level++;
    data = Managers.Data.CreatureDic[DataId];
    MaxHp = (int)((Data.MaxHp + Level * Data.MaxHpBonus) * Data.HpRate);
    Atk = (int)(Data.Atk + (Level * Data.AtkBonus) * Data.AtkRate);
    Def = (int)Data.Def;
    MoveSpeed = Data.MoveSpeed * Data.MoveSpeedRate;
  }
}
