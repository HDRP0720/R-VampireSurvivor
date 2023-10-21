using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
  public enum EScene { Unknown, DevScene, GameScene }
  
  public enum ESound { BGM, Effect }
  
  public enum EObjectType { Player, Monster, Projectile, Env }
  
  public enum ESkillType { None, Sequence, Repeat, Etc }
  
  public enum EStageType { Normal, Boss }
  
  public enum ECreatureState { Idle, Moving, Skill, Dead }

  public const int GOBLIN_ID = 1;
  public const int SNAKE_ID = 2;
  public const int BOSS_ID = 3;
 

  public const int EGO_SWORD_ID = 10;
  public const int FIRE_BALL_ID = 11;

  public const string EXP_GEM_PREFAB = "EXPGem.prefab";
  public const string FIRE_PROJECTILE = "FireProjectile.prefab";
}

