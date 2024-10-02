using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get;private set; }
    public GameData gameData { get; private set; }

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

    public void NewGame()
    {
        
        UIManager.Instance.CloseAllWindow();
        //¹¹½¨Ä¬ÈÏ´æµµ
        gameData = new GameData
        {
            playerPos = new SVector3(10, 5.5f, 0),
            mapSeed = Random.Range(int.MinValue, int.MaxValue),
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

}
