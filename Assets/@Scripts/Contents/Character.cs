using Data;

public class Character
{
  #region Variables
  public bool isCurrentCharacter = false;
  
  private CreatureData _data;
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
    _data = Managers.Data.CreatureDic[key];
    MaxHp = (int)((_data.maxHp + Level * _data.maxHpBonus) * _data.hpRate);
    Atk = (int)(_data.atk + (Level * _data.atkBonus) * _data.atkRate);
    Def = (int)_data.def;
    MoveSpeed = _data.moveSpeed * _data.moveSpeedRate;
  }

  public void LevelUp()
  {
    Level++;
    _data = Managers.Data.CreatureDic[DataId];
    MaxHp = (int)((_data.maxHp + Level * _data.maxHpBonus) * _data.hpRate);
    Atk = (int)(_data.atk + (Level * _data.atkBonus) * _data.atkRate);
    Def = (int)_data.def;
    MoveSpeed = _data.moveSpeed * _data.moveSpeedRate;
  }
}
