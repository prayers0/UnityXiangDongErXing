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
    [SerializeField] private MapConfig mapConfig;
    [SerializeField] private Tilemap groundTileMap;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private int mapSeed;//地图种子
    
    private float lastTargetPosx = float.MaxValue;

    private void Awake()
    {
        groundTileMap.ClearAllTiles();
        Init(mapSeed);
        CreateMapChunk(0);
    }

    public void Init(int mapSeed)
    {
        this.mapSeed = mapSeed;
    }

    private void LateUpdate()
    {
        if (cameraTransform.position.x != lastTargetPosx)
        {
            lastTargetPosx=cameraTransform.position.x;
            ParallaxScrollingController.UpdateLyayrs(lastTargetPosx);
        }
    }

    //生成地图块
    private void CreateMapChunk(int chunkCoord)
    {
        int chunkSeed = mapSeed + chunkCoord;
        Random chunkRandom=new Random(chunkSeed);
        CreateChunkSegments(chunkCoord, chunkRandom);
        //TODO:生成花草
    }

    //生成地图块中的段
    private void CreateChunkSegments(int chunkCoord,Random random)
    {
        int start = mapConfig.chunkSize * chunkCoord;
        //一个地图内部要按照端去生成，要避免一个段少于2格的情况（规则瓦片会崩）
        for(int currentCoord = 0; currentCoord < mapConfig.chunkSize;)
        {
            //随机一个段落尺寸
            int segmentSize = random.Next(mapConfig.chunkSegmentSizeRange.x, mapConfig.chunkSegmentSizeRange.y);

            //段落尺寸不能导致下一个段落尺寸少于2
            if (mapConfig.chunkSize - (currentCoord + segmentSize) < 2)
            {
                segmentSize = mapConfig.chunkSize - currentCoord;//填满
            }
            //段落的尺寸不能导致超出当前的地图块
            if (currentCoord+segmentSize>mapConfig.chunkSize)
            {
                segmentSize =   mapConfig.chunkSize - currentCoord;//填满
            }
            bool secondFloor = random.Next(0, 2) == 0;
            int segmentStartCoord = start + currentCoord;
            CreateGround(segmentStartCoord,segmentSize,secondFloor);
            CreateMapDecorations(segmentStartCoord, segmentSize, secondFloor);
            currentCoord += segmentSize;
        }
    }
    //生成地面
    private void CreateGround(int startCoord,int segmentSize,bool secondFloor)
    {
        //填充tileMap
        for (int i = 0; i < segmentSize; i++)
        {
            groundTileMap.SetTile(new Vector3Int(startCoord + i, 0, 0), mapConfig.groundTile);
            if (secondFloor)
            {
                groundTileMap.SetTile(new Vector3Int(startCoord + i, 1, 0), mapConfig.groundTile);
            }
        }
        
    }
    //创建装饰物
    private void CreateMapDecorations(int startCoord, int segmentSize, bool secondFloor)
    {

    }
}
