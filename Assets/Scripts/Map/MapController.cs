using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;
using Random = System.Random;
using Unity.Mathematics;
using Unity.VisualScripting;

public class MapController : MonoBehaviour
{
    [SerializeField] private ParallaxScrollingController ParallaxScrollingController;
    [SerializeField] private CameraController cameraController;
    [SerializeField] private Transform decorationRoot;
    [SerializeField] private MapConfig mapConfig;

    [SerializeField] private Tilemap groundTileMap;
    [SerializeField] private int mapSeed;//地图种子
    [SerializeField] private PlayerController playerController; //玩家应该有更上级的管理器初始化得来

    //key地图块索引，value：地图块对象
    private Dictionary<int,MapChunk> mapChunkDic = new Dictionary<int,MapChunk>();
    private float cellSize;//格子尺寸

    private float cellTopOffset;//地图种子


    private float lastTargetPosx = float.MaxValue;

    private void Awake()
    {
        Init(mapSeed,playerController);
        //CreateMapChunk(0);
        
    }

    public void Init(int mapSeed,PlayerController playerController)
    {
        Grid grid = GetComponentInChildren<Grid>();
        groundTileMap.ClearAllTiles();
        cameraController.Init(playerController.transform);
        cellSize = grid.cellSize.x;
        cellTopOffset = cellSize / 2;
        this.mapSeed = mapSeed;
        
    }

    private void LateUpdate()
    {
        if (cameraController.transform.position.x != lastTargetPosx)
        {
            float oldTargetPosX = lastTargetPosx;
            float newTargetPosX = cameraController.transform.position.x;
            lastTargetPosx = cameraController.transform.position.x;
            //更新视差
            ParallaxScrollingController.UpdateLyayrs(newTargetPosX);
            //更新地图
            CheckMapChunkCreation(oldTargetPosX, newTargetPosX);
            lastTargetPosx = newTargetPosX;
        }
        CheckMapChunkDestroy();
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
            mapChunk.Init(mapConfig, groundTileMap, decorationRoot, chunkCoord, mapSeed, cellSize, cellTopOffset);
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

    
}

    