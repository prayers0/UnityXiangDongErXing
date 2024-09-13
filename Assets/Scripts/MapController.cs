using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;
using Random = System.Random;

public class MapController : MonoBehaviour
{
    [SerializeField] private ParallaxScrollingController ParallaxScrollingController;
    [SerializeField] private Tilemap groundTileMap;
    [SerializeField] private TileBase groundTile;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private int mapSeed;//地图种子
    [SerializeField] private int chunkSize=50;
    [SerializeField] private Vector2Int chunkSegmentSizeRange = new Vector2Int(2, 11);//2~10
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
        CreateGrounds(chunkCoord, chunkRandom);
        //TODO:生成花草
    }

    //创建地面
    private void CreateGrounds(int chunkCoord,Random random)
    {
        int start = chunkSize * chunkCoord;
        //一个地图内部要按照端去生成，要避免一个段少于2格的情况（规则瓦片会崩）
        for(int currentCoord = 0; currentCoord < chunkSize;)
        {
            //随机一个段落尺寸
            int segmentSize = random.Next(chunkSegmentSizeRange.x, chunkSegmentSizeRange.y);

            //段落尺寸不能导致下一个段落尺寸少于2
            if (chunkSize - (currentCoord + segmentSize) < 2)
            {
                segmentSize = chunkSize - currentCoord;//填满
            }
            //段落的尺寸不能导致超出当前的地图块
            if (currentCoord+segmentSize>chunkSize)
            {
                segmentSize = chunkSize - currentCoord;//填满
            }
            //填充tileMap
            bool secondFloor = random.Next(0, 2) == 0;
            for(int i = 0; i < segmentSize; i++)
            {
                groundTileMap.SetTile(new Vector3Int(start + currentCoord + i, 0, 0), groundTile);
                if (secondFloor)
                {
                    groundTileMap.SetTile(new Vector3Int(start + currentCoord + i, 1, 0), groundTile);
                }
            }
            Debug.Log($"地图块段起点。{currentCoord},长度，{segmentSize}");
            currentCoord+= segmentSize;
        }
    }

}
