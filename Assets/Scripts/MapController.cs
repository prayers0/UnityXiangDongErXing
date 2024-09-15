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

    //key��ͼ��������value����ͼ�����
    private Dictionary<int,MapChunk> mapChunkDic = new Dictionary<int,MapChunk>();
    private float cellSize;//���ӳߴ�

    private float cellTopOffset;//��ͼ����


    private float lastTargetPosx = float.MaxValue;

    private void Awake()
    {
        Init(mapSeed);
        //CreateMapChunk(0);
        
    }

    public void Init(int mapSeed)
    {
        Grid grid = GetComponentInChildren<Grid>();
        groundTileMap.ClearAllTiles();

        cellSize = grid.cellSize.x;
        cellTopOffset = cellSize / 2;
        this.mapSeed = mapSeed;
    }

    private void LateUpdate()
    {
        if (cameraTransform.position.x != lastTargetPosx)
        {
            float oldTargetPosX = lastTargetPosx;
            float newTargetPosX = cameraTransform.position.x;
            lastTargetPosx = cameraTransform.position.x;
            //�����Ӳ�
            ParallaxScrollingController.UpdateLyayrs(newTargetPosX);
            //���µ�ͼ
            CheckMapChunkCreation(oldTargetPosX, newTargetPosX);
            lastTargetPosx = newTargetPosX;
        }
        CheckMapChunkDestroy();
    }

    //����ͼ�������
    private void CheckMapChunkCreation(float oldTargetPosX,float newTargetPosX)
    {
        int oldChunkIndex = (int)(oldTargetPosX / (mapConfig.chunkSize * cellSize));
        int newChunkIndex = (int)(newTargetPosX / (mapConfig.chunkSize * cellSize));
        if (oldChunkIndex != newChunkIndex)//��ҵ���һ���µĵ�ͼ��
        {
            //����п����Ǵ������ģ�Ҳ���ǿ�Խ�˺ܶ����ͼ��
            //�رվɵĵ�ͼ��
            DisableMapChunk(oldChunkIndex);
            DisableMapChunk(oldChunkIndex - 1);
            DisableMapChunk(oldChunkIndex + 1);
            //�����µĵ�ͼ��
            EnableMapChunk(newChunkIndex);
            EnableMapChunk(newChunkIndex - 1);
            EnableMapChunk(newChunkIndex + 1);
        }
    }

    //�رյ�ͼ��
    private void DisableMapChunk(int chunkCoord)
    {
        if (chunkCoord < 0) return;
        if(mapChunkDic.TryGetValue(chunkCoord,out MapChunk mapChunk))
        {
            mapChunk.SetActive(false);
        }
    }
    //������ͼ��
    private void EnableMapChunk(int chunkCoord)
    {
        if (chunkCoord < 0) return;
        if(mapChunkDic.TryGetValue(chunkCoord,out MapChunk mapChunk))//������֪ͨ�䲻Ҫ����
        {
            mapChunk.SetActive(true);
        }
        else//�������򴴽�
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

    