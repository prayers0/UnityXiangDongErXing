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
    [SerializeField] private Transform decorationRoot;
    [SerializeField] private MapConfig mapConfig;

    [SerializeField] private Tilemap groundTileMap;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private int mapSeed;//地图种子
    [SerializeField] private float cellSize;//格子尺寸

    [SerializeField] private float cellTopOffset;//地图种子


    private float lastTargetPosx = float.MaxValue;

    private void Awake()
    {
        Grid grid = GetComponentInChildren<Grid>();
        cellSize = grid.cellSize.x;
        cellTopOffset = cellSize / 2;

        groundTileMap.ClearAllTiles();
        Init(mapSeed);
        CreateMapChunk(0);
        CreateMapChunk(1);
        CreateMapChunk(2);
        CreateMapChunk(3);
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
            CreateMapDecorations(segmentStartCoord, segmentSize, secondFloor,random);
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
    private void CreateMapDecorations(int startCoord, int segmentSize, bool secondFloor,Random random)
    {
        int coordY = secondFloor ? 1 : 0;
        for(int i = 0; i < segmentSize; i++)
        {
            foreach(MapDecorationLayerConfig layerConfig in mapConfig.mapDecorationConfigs)
            {
                //每间隔x格格子生成一个物体&&剩余空间足够放下这层物体
                if (i % layerConfig.size == 0 && segmentSize - i > layerConfig.size)
                {
                    //计算概率，如果命中则生成物体
                    if (random.NextDouble() < layerConfig.probaility)
                    {
                        GameObject prefab = layerConfig.prefab[random.Next(layerConfig.prefab.Count)];
                        GameObject newDecoration=GameObject.Instantiate(prefab,decorationRoot);
                        Vector3Int cellCoord = new Vector3Int(startCoord + i, coordY);
                        Vector3 position=groundTileMap.GetCellCenterLocal(cellCoord);
                        //Y轴偏移到盒子的顶部
                        position.y += cellTopOffset;
                        //x轴的随机偏移,min+(max-min)
                        position.x+=layerConfig.xOffsetRange.x+(layerConfig.xOffsetRange.y-layerConfig.xOffsetRange.x)*
                            (float)random.NextDouble();
                        //对其到中间如果建筑物占据5格，那么它的位置应该在第三格
                        position.x += layerConfig.size / 2;
                        newDecoration.transform.position = position;
                        //修改的精灵渲染器的层设置（物体可能是嵌套的也就是有多个精灵渲染器）
                        foreach(SpriteRenderer spriteRenderer in newDecoration.GetComponentsInChildren<SpriteRenderer>())
                        {
                            spriteRenderer.sortingLayerName = layerConfig.layer;
                        }
                    }
                }
            }
        }
    }
}
