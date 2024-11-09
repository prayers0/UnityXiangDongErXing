using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get;private set; }
    public GameData gameData { get; private set; }
    public GameSettings gameSettings { get; private set; }
   
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        IGlobalManager[] managers = GetComponentsInChildren<IGlobalManager>();
        foreach(IGlobalManager manager in managers)
        {
            manager.Init();
        }
        OnGameStart();
    }

    private void OnGameStart()
    {
        if (!SaveManager.ExistGameSettings())
        {
            gameSettings = new GameSettings
            {
                volume = 1,
                resolutionType = ResolutionType.R1920X1080,
                fullScreen = true,
            };
            SaveGameSettings();
        }
        else
        {
            gameSettings=SaveManager.GetGameSettings();
        }
        ApplyGameSettings();
    }

    public void ApplyGameSettings()
    {
        AudioManager.Instance.SetVolume(gameSettings.volume);
        Vector2Int screenSize = gameSettings.GetScreenResolution();
        Screen.SetResolution(screenSize.x, screenSize.y, gameSettings.fullScreen);
    }

    public void SaveGameSettings()
    {
        SaveManager.SaveGmaeSetting(gameSettings);
    }

    public void NewGame()
    {
        UIManager.Instance.CloseAllWindow();
        //¹¹½¨Ä¬ÈÏ´æµµ
        BagData bagData=new BagData();
        bagData.items[0] = new WeaponData { id = "Weapon1" };
        bagData.items[2] = new WeaponData { id = "Weapon2" };
        bagData.items[3] = new ConsumableData { id = "Consunable1", count=1 };
        bagData.items[4] = new ConsumableData { id = "Consunable2", count=10 };
        bagData.items[5] = new ConsumableData { id = "Consunable3", count=99 };
        bagData.usedWeaponIndex = 0;
        gameData = new GameData
        {
            //playerMainPos = new SVector3(10, 5.5f, 0),
            mapSeed = Random.Range(int.MinValue, int.MaxValue),
            onMainMap = true,
            bagData = bagData,
            coinCount = ResManager.Instance.PlayerDefaultCoinCount,
            playerHp = ResManager.Instance.PlayerMaxHp
        };
        SaveGameData();
        SceneManager.LoadScene("Game");
    }

    public void SaveGameData()
    {
        SaveManager.SaveGameData(gameData);
    }

    public void ContinueGame()
    {
        UIManager.Instance.CloseAllWindow();
        //¼ÓÔØ¾É´æµµ
        gameData = SaveManager.GetGameData();
        SceneManager.LoadScene("Game");

    }

    public void LoadMenuScene()
    {
        UIManager.Instance.CloseAllWindow();
        SceneManager.LoadScene("Menu");
    }

    public void LoadGameOverScene()
    {
        UIManager.Instance.CloseAllWindow();
        SaveManager.DeleteGameData();
        SceneManager.LoadScene("GameOver");
    }
}
