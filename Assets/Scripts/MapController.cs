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
    [SerializeField] private int mapSeed;//��ͼ����
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

    //���ɵ�ͼ��
    private void CreateMapChunk(int chunkCoord)
    {
        int chunkSeed = mapSeed + chunkCoord;
        Random chunkRandom=new Random(chunkSeed);
        CreateGrounds(chunkCoord, chunkRandom);
        //TODO:���ɻ���
    }

    //��������
    private void CreateGrounds(int chunkCoord,Random random)
    {
        int start = chunkSize * chunkCoord;
        //һ����ͼ�ڲ�Ҫ���ն�ȥ���ɣ�Ҫ����һ��������2��������������Ƭ�����
        for(int currentCoord = 0; currentCoord < chunkSize;)
        {
            //���һ������ߴ�
            int segmentSize = random.Next(chunkSegmentSizeRange.x, chunkSegmentSizeRange.y);

            //����ߴ粻�ܵ�����һ������ߴ�����2
            if (chunkSize - (currentCoord + segmentSize) < 2)
            {
                segmentSize = chunkSize - currentCoord;//����
            }
            //����ĳߴ粻�ܵ��³�����ǰ�ĵ�ͼ��
            if (currentCoord+segmentSize>chunkSize)
            {
                segmentSize = chunkSize - currentCoord;//����
            }
            //���tileMap
            bool secondFloor = random.Next(0, 2) == 0;
            for(int i = 0; i < segmentSize; i++)
            {
                groundTileMap.SetTile(new Vector3Int(start + currentCoord + i, 0, 0), groundTile);
                if (secondFloor)
                {
                    groundTileMap.SetTile(new Vector3Int(start + currentCoord + i, 1, 0), groundTile);
                }
            }
            Debug.Log($"��ͼ�����㡣{currentCoord},���ȣ�{segmentSize}");
            currentCoord+= segmentSize;
        }
    }

}
