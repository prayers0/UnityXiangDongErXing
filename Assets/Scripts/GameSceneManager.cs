using System;
using UnityEngine;

public class GameSceneManager : MonoBehaviour
{
    public static GameSceneManager Instance;
    private GameData gameData => GameManager.Instance.gameData;
    private void Awake()
    {
        Instance = this;
    }
    private void  Start()
    {
        //ºÚÄ»ÕÚµ²
        //UIManager.Instance.ShowWindow<UI_BlackCanvasDropWindow>();
        PlayerController.Instance.transform.position = GetMainMapPlayerPosition();
        PlayerController.Instance.Init();
        MapController.current.Init(GameManager.Instance.gameData.mapSeed, PlayerController.Instance);
        MapController.current.UpdateMapChunk();
        //UIManager.Instance.CloseWindow<UI_BlackCanvasDropWindow>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UIManager.Instance.ShowWindow<UI_GameScenePopUpWindow>();
        }
    }

    private Vector3 GetMainMapPlayerPosition()
    {
        Vector3 pos = gameData.playerPos.ToVectr3();
        if (pos == default)
        {
            pos=MapController.current.GetPlayerDefaultPosition();
            gameData.playerPos = new SVector3(pos);
        }
        return pos;
    }

    public void LoadMainMap()
    {
        throw new NotImplementedException();
    }

    public void EnterDungeonMap(int doorCoord)
    {
        throw new NotImplementedException();
    }
}
