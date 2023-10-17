using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
  private static Managers _instance;
  private static bool _init = false;

  public static Managers Instance
  {
    get
    {
      if (_init == false)
      {
        _init = true;
        GameObject go = GameObject.Find("Managers");
        if (go == null)
        {
          go = new GameObject() { name = "Managers" };
          go.AddComponent<Managers>();
        }
        DontDestroyOnLoad(go);
        _instance = go.GetComponent<Managers>();
      }

      return _instance;
    }
  }
  
  
}
