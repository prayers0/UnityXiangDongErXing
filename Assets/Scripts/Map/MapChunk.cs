using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random=System.Random;

public class MapChunk
{
    private List<GameObject> decorationList;//所有装饰物
    private Random chunkRandom;
    private MapConfig mapConfig;
    private Tilemap groundTileMap;
    private Transform decorationRoot;
    private float cellSize;//格子尺寸

    private float cellTopOffset;//地图种子
    private int chunkCoord;
    private bool active;
    private float destoryTimer;
    //生成地图块
    public void Init(MapConfig mapConfig,Tilemap groupTileMap,Transform decorationRoot,int chunkCoord,
        int mapSeed,float cellSize,float cellTopOffset)
    {
        this.mapConfig = mapConfig;
        this.groundTileMap = groupTileMap;
        this.decorationRoot = decorationRoot;
        this.chunkCoord = chunkCoord;
        this.cellSize = cellSize;
        this.cellTopOffset = cellTopOffset;
        int chunkSeed = mapSeed + chunkCoord;
        Random chunkRandom = new Random(chunkSeed);
        decorationList = new List<GameObject>();
        CreateChunkSegments(chunkCoord, chunkRandom);
        SetActive(true);
        //TODO:生成花草
    }

    //销毁
    private void Destory()
    {
        //销毁地面
        DestroyGround(chunkCoord*mapConfig.chunkSize,mapConfig.chunkSize);
        DestoryMapDecorations();
        //销毁装饰物

        //销毁敌人
        DestroyEnemys();
    }

    //生成地图块中的段
    private void CreateChunkSegments(int chunkCoord, Random random)
    {
        int start = mapConfig.chunkSize * chunkCoord;
        //一个地图内部要按照端去生成，要避免一个段少于2格的情况（规则瓦片会崩）
        for (int currentCoord = 0; currentCoord < mapConfig.chunkSize;)
        {
            //随机一个段落尺寸
            int segmentSize = random.Next(mapConfig.chunkSegmentSizeRange.x, mapConfig.chunkSegmentSizeRange.y);

            //段落尺寸不能导致下一个段落尺寸少于2
            if (mapConfig.chunkSize - (currentCoord + segmentSize) < 2)
            {
                segmentSize = mapConfig.chunkSize - currentCoord;//填满
            }
            //段落的尺寸不能导致超出当前的地图块
            if (currentCoord + segmentSize > mapConfig.chunkSize)
            {
                segmentSize = mapConfig.chunkSize - currentCoord;//填满
            }
            bool secondFloor = random.Next(0, 2) == 0;
            int segmentStartCoord = start + currentCoord;
            CreateGround(segmentStartCoord, segmentSize, secondFloor);
            CreateMapDecorations(segmentStartCoord, segmentSize, secondFloor, random);
            CreateEnemys(segmentStartCoord, segmentSize, secondFloor);
            currentCoord += segmentSize;
        }
    }
    //生成敌人
    private void CreateEnemys(int startCell, int size, bool secondFloor)
    {
        //怪物生成没必要基于种子随机
        MapSpawnEnemyConfig spawnEnemyConfig = mapConfig.mapSpawnEnemyConfig;

        //是否要生成
        if (UnityEngine.Random.Range(0, 1f) < spawnEnemyConfig.spawnProbability)
        {
            if (spawnEnemyConfig.prefabs.Count == 0) return;
            //生成数量
            int count = UnityEngine.Random.Range(spawnEnemyConfig.spawnCountRange.x, spawnEnemyConfig.spawnCountRange.y + 1);
            //随机生成怪物
            for(int i = 0; i < count; i++)
            {
                GameObject prefab = spawnEnemyConfig.prefabs[UnityEngine.Random.Range(0, spawnEnemyConfig.prefabs.Count)];
                int coord = UnityEngine.Random.Range(0, size) + startCell;
                Vector3 pos = new Vector3(MapController.current.GetCellWorldPostion(coord),
                    MapController.current.GetLayerWorldPosition(secondFloor), 0);
                MapController.current.EnemyManager.AddEnemy(prefab, chunkCoord,pos);
            }
        }
    }

    //生成地面
    private void CreateGround(int startCoord, int size, bool secondFloor)
    {
        //填充tileMap
        for (int i = 0; i < size; i++)
        {
            groundTileMap.SetTile(new Vector3Int(startCoord + i, 0, 0), mapConfig.groundTile);
            if (secondFloor)
            {
                groundTileMap.SetTile(new Vector3Int(startCoord + i, 1, 0), mapConfig.groundTile);
            }
        }

    }
    //创建装饰物
    private void CreateMapDecorations(int startCoord, int segmentSize, bool secondFloor, Random random)
    {
        int coordY = secondFloor ? 1 : 0;
        for (int i = 0; i < segmentSize; i++)
        {
            foreach (MapDecorationLayerConfig layerConfig in mapConfig.mapDecorationConfigs)
            {
                //每间隔x格格子生成一个物体&&剩余空间足够放下这层物体
                if (i % layerConfig.size == 0 && segmentSize - i > layerConfig.size)
                {
                    //计算概率，如果命中则生成物体
                    if (random.NextDouble() < layerConfig.probaility)
                    {
                        GameObject prefab = layerConfig.prefab[random.Next(layerConfig.prefab.Count)];
                        GameObject newDecoration = GameObject.Instantiate(prefab, decorationRoot);
                        decorationList.Add(newDecoration);
                        Vector3Int cellCoord = new Vector3Int(startCoord + i, coordY);
                        Vector3 position = groundTileMap.GetCellCenterLocal(cellCoord);
                        //Y轴偏移到盒子的顶部
                        position.y += cellTopOffset;
                        //x轴的随机偏移,min+(max-min)
                        position.x += layerConfig.xOffsetRange.x + (layerConfig.xOffsetRange.y - layerConfig.xOffsetRange.x) *
                            (float)random.NextDouble();
                        //对其到中间如果建筑物占据5格，那么它的位置应该在第三格
                        position.x += layerConfig.size / 2;
                        newDecoration.transform.position = position;
                        //修改的精灵渲染器的层设置（物体可能是嵌套的也就是有多个精灵渲染器）
                        foreach (SpriteRenderer spriteRenderer in newDecoration.GetComponentsInChildren<SpriteRenderer>())
                        {
                            spriteRenderer.sortingLayerName = layerConfig.layer;
                        }
                    }
                }
            }
        }
    }

    //销毁地面
    private void DestroyGround(int startCoord,int size)
    {
        //填充tileMap
        for (int i = 0; i < size; i++)
        {
            groundTileMap.SetTile(new Vector3Int(startCoord + i, 0, 0), null);

            groundTileMap.SetTile(new Vector3Int(startCoord + i, 1, 0), null);

        }
    }

    //销毁地图装饰物
    private void DestoryMapDecorations()
    {
        for(int i = 0; i < decorationList.Count; i++)
        {
            GameObject.Destroy(decorationList[i]);
        }
        decorationList.Clear();
        decorationList = null;
        mapConfig = null;
        groundTileMap = null;
        decorationRoot = null;
        chunkRandom = null;
    }

    //销毁敌人
    private void DestroyEnemys()
    {
        MapController.current.EnemyManager.RemoveMapchunkEnemys(chunkCoord);
    }

    public void SetActive(bool active)
    {
        if (this.active == active) return;
        this.active = active;
        if (!active)//开始销毁倒计时
        {
            destoryTimer = mapConfig.mapChunkDestroyTime;
        }
    }

    public bool CheckDestroy()
    {
        if (!active)
        {
            destoryTimer-=Time.deltaTime;
            if (destoryTimer <= 0)
            {
                Destory();
                return true;
            }
        }
        return false;
    }
}
