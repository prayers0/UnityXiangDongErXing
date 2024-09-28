using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get;private set; }

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
    }
    public void ContinueGame()
    {
        SceneManager.LoadScene("Game");
    }
}
