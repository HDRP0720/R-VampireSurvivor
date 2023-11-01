using Unity.VisualScripting;
using UnityEngine;

public class Managers : MonoBehaviour
{
  private static Managers _instance;

  private static Managers Instance { get { Init(); return _instance; } } 
  

  #region Contents
  private GameManager _game = new GameManager();
  private ObjectManager _object = new ObjectManager();
  private TimeManager _time;
  private AchievementManager _achievment = new AchievementManager();
  
  // properties
  public static GameManager Game => Instance != null ? Instance._game : null;
  public static ObjectManager Object => Instance != null ? Instance._object : null;
  public static TimeManager Time => Instance != null ? Instance._time : null;
  public static AchievementManager Achievement => Instance != null ? Instance._achievment : null;
  #endregion

  #region Core
  private DataManager _data = new DataManager();
  private PoolManager _pool = new PoolManager();
  private ResourceManager _resource = new ResourceManager();
  private SceneManagerEx _scene = new SceneManagerEx();
  private SoundManager _sound = new SoundManager();
  private UIManager _ui = new UIManager();
  
  // properties
  public static DataManager Data => Instance != null ? Instance._data : null;
  public static PoolManager Pool => Instance != null ? Instance._pool : null;
  public static ResourceManager Resource => Instance != null ? Instance._resource : null;
  public static SceneManagerEx Scene => Instance != null ? Instance._scene : null;
  public static SoundManager Sound => Instance != null ? Instance._sound : null;
  public static UIManager UI => Instance != null ? Instance._ui : null;
  #endregion

  public static void Init()
  {
    if (_instance == null)
    {
      GameObject go = GameObject.Find("Managers");
      if (go == null)
      {
        go = new GameObject { name = "Managers" };
        go.AddComponent<Managers>();
      }
      
      DontDestroyOnLoad(go);
      _instance = go.GetComponent<Managers>();
      _instance._sound.Init();
      _instance._time = go.AddComponent<TimeManager>();
    }
  }

  public static void Clear()
  {
    Sound.Clear();
    Scene.Clear();
    UI.Clear();
    Pool.Clear();
    Object.Clear();
  }
}
