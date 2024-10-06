using System;
using Unity.VisualScripting;
using UnityEngine;

public class GameSceneManager : MonoBehaviour
{
    public static GameSceneManager Instance;
    public GameObject mainMapPrefab;
    public GameObject dungeonMapPrefab;
    private GameData gameData => GameManager.Instance.gameData;
    private void Awake()
    {
        Instance = this;
    }
    private void  Start()
    {
        PlayerController.Instance.Init();

        if (gameData.onMainMap) LoadMainMap();
        else LoadDungeonMap();
        //ºÚÄ»ÕÚµ²
        //UIManager.Instance.ShowWindow<UI_BlackCanvasDropWindow>();
        
        //UIManager.Instance.CloseWindow<UI_BlackCanvasDropWindow>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UIManager.Instance.ShowWindow<UI_GameScenePopUpWindow>();
        }
    }

    public void LoadMainMap()
    {
        gameData.onMainMap = true;
        if (MapController.HaveMap)
        {
            MapController.current.Destroy();
        }
        
        MapController mapController = Instantiate(mainMapPrefab).GetComponent<MapController>();
        PlayerController.Instance.transform.position = GetMainMapPlayerPosition();
        mapController.Init(gameData.mapSeed, PlayerController.Instance);
        mapController.UpdateMapChunk();
    }


    private void LoadDungeonMap()
    {
        gameData.onMainMap = false;
        if (MapController.HaveMap)
        {
            MapController.current.Destroy();
        }
        
        MapController mapController = Instantiate(dungeonMapPrefab).GetComponent<MapController>();
        PlayerController.Instance.transform.position = GetDungeonMapPlayerPosition();
        int seed = (gameData.mapSeed + 100) / 2 + gameData.dungeonCoord;
        mapController.Init(seed, PlayerController.Instance);
        mapController.UpdateMapChunk();
    }

    public void EnterDungeonMap(int doorCoord)
    {
        gameData.onMainMap = false;
        if (MapController.HaveMap)
        {
            MapController.current.Destroy();
        }
        MapController mapController=Instantiate(dungeonMapPrefab).GetComponent<MapController>();
        gameData.playerDungeonMapPos = default;
        PlayerController.Instance.transform.position=GetDungeonMapPlayerPosition();
        int seed = (gameData.mapSeed + 100) / 2 + doorCoord;
        gameData.dungeonCoord = seed;
        mapController.Init(seed,PlayerController.Instance);
        mapController.UpdateMapChunk();
    }

    private Vector3 GetMainMapPlayerPosition()
    {
        Vector3 pos = gameData.playerMainPos.ToVectr3();
        if (pos == default)
        {
            pos = MapController.current.GetPlayerDefaultPosition();
            gameData.playerMainPos = new SVector3(pos);
        }
        return pos;
    }

    private Vector3 GetDungeonMapPlayerPosition()
    {
        //Vector3 pos = default;
        Vector3 pos = gameData.playerDungeonMapPos.ToVectr3();
        if (pos == default)
        {
            pos = MapController.current.GetPlayerDefaultPosition();
            gameData.playerDungeonMapPos = new SVector3(pos);
        }
        return pos;
    }

    public void UpdatePlayerPositionData(Vector3 position)
    {
        if (gameData.onMainMap)
        {
            gameData.playerMainPos=new SVector3(position);
        }
        else
        {
            gameData.playerDungeonMapPos = new SVector3(position);
        }
    }
}
