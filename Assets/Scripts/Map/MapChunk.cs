using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random=System.Random;

public class MapChunk
{
    private List<GameObject> decorationList;//����װ����
    private Random chunkRandom;
    private MapConfig mapConfig;
    private Tilemap groundTileMap;
    private Transform decorationRoot;
    private float cellSize;//���ӳߴ�

    private float cellTopOffset;//��ͼ����
    private int chunkCoord;
    private bool active;
    private float destoryTimer;
    //���ɵ�ͼ��
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
        //TODO:���ɻ���
    }

    //����
    private void Destory()
    {
        //���ٵ���
        DestroyGround(chunkCoord*mapConfig.chunkSize,mapConfig.chunkSize);
        DestoryMapDecorations();
        //����װ����

        //���ٵ���
        DestroyEnemys();
    }

    //���ɵ�ͼ���еĶ�
    private void CreateChunkSegments(int chunkCoord, Random random)
    {
        int start = mapConfig.chunkSize * chunkCoord;
        //һ����ͼ�ڲ�Ҫ���ն�ȥ���ɣ�Ҫ����һ��������2��������������Ƭ�����
        for (int currentCoord = 0; currentCoord < mapConfig.chunkSize;)
        {
            //���һ������ߴ�
            int segmentSize = random.Next(mapConfig.chunkSegmentSizeRange.x, mapConfig.chunkSegmentSizeRange.y);

            //����ߴ粻�ܵ�����һ������ߴ�����2
            if (mapConfig.chunkSize - (currentCoord + segmentSize) < 2)
            {
                segmentSize = mapConfig.chunkSize - currentCoord;//����
            }
            //����ĳߴ粻�ܵ��³�����ǰ�ĵ�ͼ��
            if (currentCoord + segmentSize > mapConfig.chunkSize)
            {
                segmentSize = mapConfig.chunkSize - currentCoord;//����
            }
            bool secondFloor = random.Next(0, 2) == 0;
            int segmentStartCoord = start + currentCoord;
            CreateGround(segmentStartCoord, segmentSize, secondFloor);
            CreateMapDecorations(segmentStartCoord, segmentSize, secondFloor, random);
            CreateEnemys(segmentStartCoord, segmentSize, secondFloor);
            currentCoord += segmentSize;
        }
    }
    //���ɵ���
    private void CreateEnemys(int startCell, int size, bool secondFloor)
    {
        //��������û��Ҫ�����������
        MapSpawnEnemyConfig spawnEnemyConfig = mapConfig.mapSpawnEnemyConfig;

        //�Ƿ�Ҫ����
        if (UnityEngine.Random.Range(0, 1f) < spawnEnemyConfig.spawnProbability)
        {
            if (spawnEnemyConfig.prefabs.Count == 0) return;
            //��������
            int count = UnityEngine.Random.Range(spawnEnemyConfig.spawnCountRange.x, spawnEnemyConfig.spawnCountRange.y + 1);
            //������ɹ���
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

    //���ɵ���
    private void CreateGround(int startCoord, int size, bool secondFloor)
    {
        //���tileMap
        for (int i = 0; i < size; i++)
        {
            groundTileMap.SetTile(new Vector3Int(startCoord + i, 0, 0), mapConfig.groundTile);
            if (secondFloor)
            {
                groundTileMap.SetTile(new Vector3Int(startCoord + i, 1, 0), mapConfig.groundTile);
            }
        }

    }
    //����װ����
    private void CreateMapDecorations(int startCoord, int segmentSize, bool secondFloor, Random random)
    {
        int coordY = secondFloor ? 1 : 0;
        for (int i = 0; i < segmentSize; i++)
        {
            foreach (MapDecorationLayerConfig layerConfig in mapConfig.mapDecorationConfigs)
            {
                //ÿ���x���������һ������&&ʣ��ռ��㹻�����������
                if (i % layerConfig.size == 0 && segmentSize - i > layerConfig.size)
                {
                    //������ʣ������������������
                    if (random.NextDouble() < layerConfig.probaility)
                    {
                        GameObject prefab = layerConfig.prefab[random.Next(layerConfig.prefab.Count)];
                        GameObject newDecoration = GameObject.Instantiate(prefab, decorationRoot);
                        decorationList.Add(newDecoration);
                        Vector3Int cellCoord = new Vector3Int(startCoord + i, coordY);
                        Vector3 position = groundTileMap.GetCellCenterLocal(cellCoord);
                        //Y��ƫ�Ƶ����ӵĶ���
                        position.y += cellTopOffset;
                        //x������ƫ��,min+(max-min)
                        position.x += layerConfig.xOffsetRange.x + (layerConfig.xOffsetRange.y - layerConfig.xOffsetRange.x) *
                            (float)random.NextDouble();
                        //���䵽�м����������ռ��5����ô����λ��Ӧ���ڵ�����
                        position.x += layerConfig.size / 2;
                        newDecoration.transform.position = position;
                        //�޸ĵľ�����Ⱦ���Ĳ����ã����������Ƕ�׵�Ҳ�����ж��������Ⱦ����
                        foreach (SpriteRenderer spriteRenderer in newDecoration.GetComponentsInChildren<SpriteRenderer>())
                        {
                            spriteRenderer.sortingLayerName = layerConfig.layer;
                        }
                    }
                }
            }
        }
    }

    //���ٵ���
    private void DestroyGround(int startCoord,int size)
    {
        //���tileMap
        for (int i = 0; i < size; i++)
        {
            groundTileMap.SetTile(new Vector3Int(startCoord + i, 0, 0), null);

            groundTileMap.SetTile(new Vector3Int(startCoord + i, 1, 0), null);

        }
    }

    //���ٵ�ͼװ����
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

    //���ٵ���
    private void DestroyEnemys()
    {
        MapController.current.EnemyManager.RemoveMapchunkEnemys(chunkCoord);
    }

    public void SetActive(bool active)
    {
        if (this.active == active) return;
        this.active = active;
        if (!active)//��ʼ���ٵ���ʱ
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
