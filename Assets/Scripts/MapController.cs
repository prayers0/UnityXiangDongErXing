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
    [SerializeField] private int mapSeed;//��ͼ����
    
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
            CreateMapDecorations(segmentStartCoord, segmentSize, secondFloor);
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
    private void CreateMapDecorations(int startCoord, int segmentSize, bool secondFloor)
    {

    }
}
