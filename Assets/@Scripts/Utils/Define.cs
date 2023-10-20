using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
  public enum EScene { Unknown, DevScene, GameScene }
  
  public enum ESound { BGM, Effect }
  
  public enum EObjectType { Player, Monster, Projectile, Env }
  
  public enum ESkillType{ None, Melee, Projectile, Etc }

  public const string EXP_GEM_PREFAB = "EXPGem.prefab";
  public const string FIRE_PROJECTILE = "FireProjectile.prefab";
}

