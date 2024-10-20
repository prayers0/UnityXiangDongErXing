using System.Collections;
using System.Collections.Generic;
using System.Drawing;
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
    private Transform otherRoot;
    private float cellSize;//���ӳߴ�

    private float cellTopOffset;//��ͼ����
    private int chunkCoord;
    private bool active;
    private float destoryTimer;
    private bool haveDoor;
    private bool haveNPC;
    //���ɵ�ͼ��
    public void Init(MapConfig mapConfig,Tilemap groupTileMap,Transform otherRoot,Transform decorationRoot,int chunkCoord,
        int mapSeed,float cellSize,float cellTopOffset)
    {
        this.mapConfig = mapConfig;
        this.groundTileMap = groupTileMap;
        this.decorationRoot = decorationRoot;
        this.chunkCoord = chunkCoord;
        this.otherRoot = otherRoot;
        this.cellSize = cellSize;
        this.cellTopOffset = cellTopOffset;
        int chunkSeed = mapSeed + chunkCoord;
        Random chunkRandom = new Random(chunkSeed);
        decorationList = new List<GameObject>();
        //�Ƿ������
        MapDungeonDoorConfig doorConfig = mapConfig.mapDoorConfig;
        haveDoor = doorConfig.prefab.Count > 0 
            && chunkCoord % doorConfig.chunkStep == 0
            && chunkRandom.NextDouble()<doorConfig.probaility;

        MapNPCConfig npcConfig=mapConfig.mapNPCConfig;
        haveNPC=!haveDoor
            &&doorConfig.prefab.Count>0
            && chunkCoord%npcConfig.chunkStep==0
            &&chunkRandom.NextDouble()< npcConfig.probaility;

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
        DestroyDoor();
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
            CreateGround(segmentStartCoord, segmentSize, secondFloor);//��������
            if (chunkCoord != 0 || segmentStartCoord!= 0)//�����һ����ͼ��ĵ�һ����ͼ�ξ����ɵ���
            {
                CreateEnemys(segmentStartCoord, segmentSize, secondFloor);
            }
            bool lastSecond=segmentSize==mapConfig.chunkSize-currentCoord;
            if (haveDoor && lastSecond)//��Ҫ�Ų��ҵ�ǰ�����һ��
            {
                CreateDoor(segmentStartCoord, segmentSize, secondFloor, random);
            }
            else if(haveNPC && lastSecond)
            {
                CreateNpc(segmentStartCoord, segmentSize, secondFloor, random);
            }
            else
            {
                CreateMapDecorations(segmentStartCoord, segmentSize, secondFloor, random);//����װ����
            }

            
            currentCoord += segmentSize;
        }
    }
    private GameObject npc;
    private void CreateNpc(int startCoord, int size, bool secondFloor, Random random)
    {
        int coordY = secondFloor ? 1 : 0;
        MapNPCConfig npcConfig = mapConfig.mapNPCConfig;
        GameObject prefab = npcConfig.prefab[random.Next(npcConfig.prefab.Count)];
        npc = GameObject.Instantiate(prefab, otherRoot);
        Vector3Int cellCoord = new Vector3Int(startCoord + (size / 2), coordY);
        Vector3 position = groundTileMap.GetCellCenterLocal(cellCoord);
        //Y��ƫ�Ƶ����ӵĶ���
        position.y += cellTopOffset;
        npc.transform.position = position;
        //�޸ĵľ�����Ⱦ���Ĳ����ã����������Ƕ�׵�Ҳ�����ж��������Ⱦ����
        foreach (SpriteRenderer spriteRenderer in npc.GetComponentsInChildren<SpriteRenderer>())
        {
            spriteRenderer.sortingLayerName = npcConfig.layer;
        }
        //���˵ĳ�ʼ��
        npc.GetComponent<NPCBase>().Init(random);
    }

    private GameObject door;
    private void CreateDoor(int startCoord, int size, bool secondFloor,Random random)
    {
        int coordY = secondFloor ? 1 : 0;
        MapDungeonDoorConfig doorConfig = mapConfig.mapDoorConfig;
        GameObject prefab= doorConfig.prefab[random.Next(doorConfig.prefab.Count)];
        door = GameObject.Instantiate(prefab,otherRoot);
        Vector3Int cellCoord = new Vector3Int(startCoord + (size/2), coordY);
        Vector3 position = groundTileMap.GetCellCenterLocal(cellCoord);
        //Y��ƫ�Ƶ����ӵĶ���
        position.y += cellTopOffset;
        door.transform.position = position;
        //�޸ĵľ�����Ⱦ���Ĳ����ã����������Ƕ�׵�Ҳ�����ж��������Ⱦ����
        foreach (SpriteRenderer spriteRenderer in door.GetComponentsInChildren<SpriteRenderer>())
        {
            spriteRenderer.sortingLayerName = doorConfig.layer;
        }
        door.GetComponent<Door>().Init(doorConfig);
    }

    private void DestroyDoor()
    {
        if (door != null)
        {
            GameObject.Destroy(door);
            door = null;
        }
    }

    //���ɵ���
    private void CreateEnemys(int startCell, int size, bool secondFloor)
    {
        if (size < mapConfig.mapSpawnEnemyConfig.minSegmentSize) return;
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
            if (mapConfig.groundTile != null)
            {
                groundTileMap.SetTile(new Vector3Int(startCoord + i, mapConfig.groundTileCoordY, 0), mapConfig.groundTile);
                if (secondFloor)
                {
                    groundTileMap.SetTile(new Vector3Int(startCoord + i, mapConfig.groundTileCoordY+1, 0), mapConfig.groundTile);
                }
            }

            if (mapConfig.topTile != null)
            {
                groundTileMap.SetTile(new Vector3Int(startCoord + i, mapConfig.topTileCoordY, 0), mapConfig.topTile);
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
