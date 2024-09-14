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
    [SerializeField] private int mapSeed;//��ͼ����
    [SerializeField] private float cellSize;//���ӳߴ�

    [SerializeField] private float cellTopOffset;//��ͼ����


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

    //���ɵ�ͼ��
    private void CreateMapChunk(int chunkCoord)
    {
        int chunkSeed = mapSeed + chunkCoord;
        Random chunkRandom=new Random(chunkSeed);
        CreateChunkSegments(chunkCoord, chunkRandom);
        //TODO:���ɻ���
    }

    //���ɵ�ͼ���еĶ�
    private void CreateChunkSegments(int chunkCoord,Random random)
    {
        int start = mapConfig.chunkSize * chunkCoord;
        //һ����ͼ�ڲ�Ҫ���ն�ȥ���ɣ�Ҫ����һ��������2��������������Ƭ�����
        for(int currentCoord = 0; currentCoord < mapConfig.chunkSize;)
        {
            //���һ������ߴ�
            int segmentSize = random.Next(mapConfig.chunkSegmentSizeRange.x, mapConfig.chunkSegmentSizeRange.y);

            //����ߴ粻�ܵ�����һ������ߴ�����2
            if (mapConfig.chunkSize - (currentCoord + segmentSize) < 2)
            {
                segmentSize = mapConfig.chunkSize - currentCoord;//����
            }
            //����ĳߴ粻�ܵ��³�����ǰ�ĵ�ͼ��
            if (currentCoord+segmentSize>mapConfig.chunkSize)
            {
                segmentSize =   mapConfig.chunkSize - currentCoord;//����
            }
            bool secondFloor = random.Next(0, 2) == 0;
            int segmentStartCoord = start + currentCoord;
            CreateGround(segmentStartCoord,segmentSize,secondFloor);
            CreateMapDecorations(segmentStartCoord, segmentSize, secondFloor,random);
            currentCoord += segmentSize;
        }
    }
    //���ɵ���
    private void CreateGround(int startCoord,int segmentSize,bool secondFloor)
    {
        //���tileMap
        for (int i = 0; i < segmentSize; i++)
        {
            groundTileMap.SetTile(new Vector3Int(startCoord + i, 0, 0), mapConfig.groundTile);
            if (secondFloor)
            {
                groundTileMap.SetTile(new Vector3Int(startCoord + i, 1, 0), mapConfig.groundTile);
            }
        }
        
    }
    //����װ����
    private void CreateMapDecorations(int startCoord, int segmentSize, bool secondFloor,Random random)
    {
        int coordY = secondFloor ? 1 : 0;
        for(int i = 0; i < segmentSize; i++)
        {
            foreach(MapDecorationLayerConfig layerConfig in mapConfig.mapDecorationConfigs)
            {
                //ÿ���x���������һ������&&ʣ��ռ��㹻�����������
                if (i % layerConfig.size == 0 && segmentSize - i > layerConfig.size)
                {
                    //������ʣ������������������
                    if (random.NextDouble() < layerConfig.probaility)
                    {
                        GameObject prefab = layerConfig.prefab[random.Next(layerConfig.prefab.Count)];
                        GameObject newDecoration=GameObject.Instantiate(prefab,decorationRoot);
                        Vector3Int cellCoord = new Vector3Int(startCoord + i, coordY);
                        Vector3 position=groundTileMap.GetCellCenterLocal(cellCoord);
                        //Y��ƫ�Ƶ����ӵĶ���
                        position.y += cellTopOffset;
                        //x������ƫ��,min+(max-min)
                        position.x+=layerConfig.xOffsetRange.x+(layerConfig.xOffsetRange.y-layerConfig.xOffsetRange.x)*
                            (float)random.NextDouble();
                        //���䵽�м����������ռ��5����ô����λ��Ӧ���ڵ�����
                        position.x += layerConfig.size / 2;
                        newDecoration.transform.position = position;
                        //�޸ĵľ�����Ⱦ���Ĳ����ã����������Ƕ�׵�Ҳ�����ж��������Ⱦ����
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
