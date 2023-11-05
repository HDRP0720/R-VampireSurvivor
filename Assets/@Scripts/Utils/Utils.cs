using System;
using UnityEngine;
using Random = UnityEngine.Random;
using Transform = UnityEngine.Transform;

using static Define;

public class Utils
{
  public static T GetOrAddComponent<T>(GameObject go) where T : Component
  {
    T component = go.GetComponent<T>();
    if (component == null)
      component = go.AddComponent<T>();

    return component;
  }

  public static GameObject FindChild(GameObject go, string name = null, bool recursive = false)
  {
    Transform transform = FindChild<Transform>(go, name, recursive);
    if (transform == null) return null;

    return transform.gameObject;
  }

  public static T FindChild<T>(GameObject go, string name = null, bool recursive = false) where T : UnityEngine.Object
  {
    if (go == null) return null;

    if (recursive == false)
    {
      for (int i = 0; i < go.transform.childCount; i++)
      {
        Transform transform = go.transform.GetChild(i);
        if (string.IsNullOrEmpty(name) || transform.name == name)
        {
          T component = transform.GetComponent<T>();
          if (component != null)
            return component;
        }
      }
    }
    else
    {
      foreach (T component in go.GetComponentsInChildren<T>())
      {
        if (string.IsNullOrEmpty(name) || component.name == name)
          return component;
      }
    }

    return null;
  }

  public static Vector2 RandomPointInAnnulus(Vector2 origin, float minRadius = 6, float maxRadius = 12)
  {
    float randomDist = Random.Range(minRadius, maxRadius);
    Vector2 randomDir = new Vector2(Random.Range(-100, 100), Random.Range(-100, 100)).normalized;

    var point = origin + randomDir * randomDist;
    return point;
  }
  
  public static Vector2 GenerateMonsterSpawnPosition(Vector2 characterPosition, float minDistance = 20.0f, float maxDistance = 25.0f)
  {
    float angle = Random.Range(0, 360) * Mathf.Deg2Rad;
    float distance = Random.Range(minDistance, maxDistance);

    float xDist = Mathf.Cos(angle) * distance;
    float yDist = Mathf.Sin(angle) * distance;

    Vector2 spawnPosition = characterPosition + new Vector2(xDist, yDist);
    
    // Change spawn position to ellipse shape if outside map boundaries
    float size = Managers.Game.CurrentMap.MapSize.x * 0.5f;
    if (Mathf.Abs(spawnPosition.x) > size || Mathf.Abs(spawnPosition.y) > size)
    {
      float ellipseFactorX = Mathf.Lerp(1f, 0.5f, Mathf.Abs(characterPosition.x) / size);
      float ellipseFactorY = Mathf.Lerp(1f, 0.5f, Mathf.Abs(characterPosition.y) / size);

      xDist *= ellipseFactorX;
      yDist *= ellipseFactorY;

      spawnPosition = Vector2.zero + new Vector2(xDist, yDist);

      // Change spawn position inside of map boundaries
      spawnPosition.x = Mathf.Clamp(spawnPosition.x, -size, size);
      spawnPosition.y = Mathf.Clamp(spawnPosition.y, -size, size);
    }
    
    return spawnPosition;
  }
  
  public static Color HexToColor(string color)
  {
    Color parsedColor;
    ColorUtility.TryParseHtmlString("#"+color, out parsedColor);

    return parsedColor;
  }
  
  public static ESkillType GetSkillTypeFromInt(int value)
  {
    foreach (ESkillType skillType in Enum.GetValues(typeof(ESkillType)))
    {
      int minValue = (int)skillType;
      int maxValue = minValue + 5; // 100501~ 100506 사이 값이면 100501값 리턴

      if (value >= minValue && value <= maxValue)
      {
        return skillType;
      }
    }

    Debug.LogError($" Faild add skill : {value}");
    return ESkillType.None;
  }
}
