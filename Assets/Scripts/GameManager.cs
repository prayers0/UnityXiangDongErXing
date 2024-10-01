using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get;private set; }
    public static GameData gameData { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        UIManager.Instance.ShowWindow<UI_MenuSceneMainWindow>();
    }

    public void NewGame()
    {
        SceneManager.LoadScene("Game");
        UIManager.Instance.CloseAllWindow();
        //构建默认存档
        gameData = new GameData
        {
            playerPos = new SVector3(10, 5.5f, 0),
            mapSeed = Random.Range(int.MinValue, int.MaxValue),
        };
        SaveGameData();
        StartCoroutine(InitGame());
    }

    public void SaveGameData()
    {
        SaveManager.SaveGameData(gameData);
    }

    public void ContinueGame()
    {
        SceneManager.LoadScene("Game");
        UIManager.Instance.CloseAllWindow();
        //加载旧存档
        gameData = SaveManager.GetGameData();
        StartCoroutine(InitGame());
    }

    private IEnumerator InitGame()
    {
        //黑幕遮挡
        UIManager.Instance.ShowWindow<UI_BlackCanvasDropWindow>();
        yield return null;//延迟一帧等待管理器级别的自身初始化
        PlayerController.Instance.transform.position = gameData.playerPos.ToVectr3();
        PlayerController.Instance.Init();
        MapController.current.Init(gameData.mapSeed, PlayerController.Instance);
        MapController.current.UpdateMapChunk();
        UIManager.Instance.CloseWindow<UI_BlackCanvasDropWindow>();
    }
}
