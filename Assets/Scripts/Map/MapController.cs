using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapController : MonoBehaviour
{
    public static MapController current;
    public static bool HaveMap=>current!=null&&current.gameObject!=null;
    [SerializeField] private ParallaxScrollingController ParallaxScrollingController;
    [SerializeField] private CameraController cameraController;
    [SerializeField] private EnemyManager enemyManager;
    [SerializeField] private Transform decorationRoot;
    [SerializeField] private MapConfig mapConfig;

    [SerializeField] private Tilemap groundTileMap;
    [SerializeField] private BoxCollider2D rightWall;
    
    [SerializeField] private int mapSeed;//地图种子
    [SerializeField] private PlayerController playerController; //玩家应该有更上级的管理器初始化得来
    public bool hasPlayer => playerController != null;

    //key地图块索引，value：地图块对象
    private Dictionary<int,MapChunk> mapChunkDic = new Dictionary<int,MapChunk>();
    private float cellSize;//格子尺寸
    public EnemyManager EnemyManager { get=>enemyManager; }

    private float halfCellSize;//地图种子


    public float lastCameraPosx { get; private set; } = float.MaxValue;
    public float playerControllerPoxX => playerController.transform.position.x;

    private void Awake()
    {
        //Init(mapSeed,playerController);
        //CreateMapChunk(0);
        current = this;
    }

    public void Init(int mapSeed,PlayerController playerController)
    {
        current = this;
        this.mapSeed = mapSeed;
        this.playerController = playerController;
        groundTileMap.ClearAllTiles();

        Grid grid = GetComponentInChildren<Grid>();
        cellSize = grid.cellSize.x;
        halfCellSize = cellSize / 2;
       
        float maxPosX = -1;
        if (mapConfig.maxChunk > 0)
        {
            maxPosX = mapConfig.chunkSize * mapConfig.maxChunk * cellSize;
            Vector3 pos = new Vector3(maxPosX + rightWall.size.x, 0, 0);
            rightWall.transform.position = pos;
        }
        cameraController.Init(playerController.transform,maxPosX);
        ParallaxScrollingController.Init(cameraController.screenWidth);

    }

    public Vector3 GetPlayerDefaultPosition()
    {
        return mapConfig.playerDefaultPosition;
    }

    private void LateUpdate()
    {
        if (!hasPlayer) return;
        UpdateMapChunk();
        CheckMapChunkDestroy();
    }

    public void UpdateMapChunk()
    {
        if (cameraController.transform.position.x != lastCameraPosx)
        {
            float oldTargetPosX = lastCameraPosx;
            float newTargetPosX = cameraController.transform.position.x;
            lastCameraPosx = cameraController.transform.position.x;
            //更新视差
            ParallaxScrollingController.UpdateLyayrs(newTargetPosX);
            //更新地图
            CheckMapChunkCreation(oldTargetPosX, newTargetPosX);
            lastCameraPosx = newTargetPosX;
        }
    }

    //检查地图快的生成
    private void CheckMapChunkCreation(float oldTargetPosX,float newTargetPosX)
    {
        int oldChunkIndex = (int)(oldTargetPosX / (mapConfig.chunkSize * cellSize));
        int newChunkIndex = (int)(newTargetPosX / (mapConfig.chunkSize * cellSize));
        if (oldChunkIndex != newChunkIndex)//玩家到了一个新的地图块
        {
            //玩家有可能是传送来的，也就是跨越了很多个地图块
            //关闭旧的地图块
            DisableMapChunk(oldChunkIndex);
            DisableMapChunk(oldChunkIndex - 1);
            DisableMapChunk(oldChunkIndex + 1);
            //开启新的地图块
            EnableMapChunk(newChunkIndex);
            EnableMapChunk(newChunkIndex - 1);
            EnableMapChunk(newChunkIndex + 1);
        }
    }

    //关闭地图块
    private void DisableMapChunk(int chunkCoord)
    {
        if (chunkCoord < 0) return;
        if(mapChunkDic.TryGetValue(chunkCoord,out MapChunk mapChunk))
        {
            mapChunk.SetActive(false);
        }
    }
    //开启地图块
    private void EnableMapChunk(int chunkCoord)
    {
        if (chunkCoord < 0) return;
        if(mapChunkDic.TryGetValue(chunkCoord,out MapChunk mapChunk))//存在则通知其不要销毁
        {
            mapChunk.SetActive(true);
        }
        else//不存在则创建
        {
            mapChunk = new MapChunk();
            mapChunkDic.Add(chunkCoord, mapChunk);
            mapChunk.Init(mapConfig, groundTileMap,transform, decorationRoot, chunkCoord, mapSeed, cellSize, halfCellSize);
        }
       
    }
    private List<int> destoryMapChunkList = new List<int>();
    private void CheckMapChunkDestroy()
    {
        foreach(KeyValuePair<int,MapChunk> item in mapChunkDic)
        {
            if (item.Value.CheckDestroy())
            {
                destoryMapChunkList.Add(item.Key);
            }
        }

        foreach(var chunkCoord in destoryMapChunkList)
        {
            mapChunkDic.Remove(chunkCoord);
        }
        destoryMapChunkList.Clear();
    }

    public int GetCellCoord(float pos)
    {
        return (int)(pos / cellSize);
    }

    public bool CheckCoordIsSedondLayer(int coord)
    {
        return groundTileMap.GetTile(new Vector3Int(coord, 1, 0)) != null;
    }

    public bool IsEmptyCell(int coord)
    {
        return groundTileMap.GetTile(new Vector3Int(coord, 0, 0)) == null;
    }

    public float GetCellWorldPostion(int coord)
    {
        return cellSize * coord - halfCellSize; 
    }

    public float GetLayerWorldPosition(bool secondLayer)
    {
        return cellSize * (secondLayer ? 2 : 1) ;
    }

    public int GetChunkCoord(float pos)
    {
        return (int)(pos / cellSize/mapConfig.chunkSize);
    }

    public void Destroy()
    {
        GameObject.Destroy(gameObject);
        current = null;
    }

    public void AddDropObject(GameObject obj, int mapChunkCoord)
    {
        if(mapChunkDic.TryGetValue(mapChunkCoord,out MapChunk mapChunk))
        {
            mapChunk.AddDropObject(obj);
        }
    }

    public void RemoveDropObject(GameObject obj, int mapChunkCoord)
    {
        if(mapChunkDic.TryGetValue(mapChunkCoord,out MapChunk mapChunk))
        {
            mapChunk.RemoveDropObject(obj);
        }
    }
}

    