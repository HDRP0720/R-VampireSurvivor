using UnityEngine;

public class Managers : MonoBehaviour
{
  private static Managers _instance;
  private static bool _init = false;

  private static Managers Instance
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

  #region Contents
  private GameManager _game = new GameManager();
  private ObjectManager _object = new ObjectManager();
  private PoolManager _pool = new PoolManager();

  public static GameManager Game => Instance != null ? Instance._game : null;
  public static ObjectManager Object => Instance != null ? Instance._object : null;
  public static PoolManager Pool => Instance != null ? Instance._pool : null;
  #endregion

  #region Core
  private DataManager _data = new DataManager();
  private ResourceManager _resource = new ResourceManager();
  private SoundManager _sound = new SoundManager();
  private SceneManagerEx _scene = new SceneManagerEx();
  private UIManager _ui = new UIManager();
  
  public static DataManager Data => Instance != null ? Instance._data : null;
  public static ResourceManager Resource => Instance != null ? Instance._resource : null;
  public static SoundManager Sound => Instance != null ? Instance._sound : null;
  public static SceneManagerEx Scene => Instance != null ? Instance._scene : null;
  public static UIManager UI => Instance != null ? Instance._ui : null;
  #endregion
}
