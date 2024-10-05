using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName ="Config/MapConfig")]
public class MapConfig : ScriptableObject
{
    public TileBase groundTile;
    public TileBase topTile;
    public int groundTileCoordY = 0;
    public int topTileCoordY = 0;
    public Vector3 playerDefaultPosition = new Vector3(1, 5.5f, 0);
    public float mapChunkDestroyTime = 5;
    public int chunkSize = 50;//��ͼ��ĳߴ�
    public Vector2Int chunkSegmentSizeRange = new Vector2Int(2, 11);//��ͼ��ĳߴ�2~10
    //����ʱÿһ��֮�以������
    public List<MapDecorationLayerConfig> mapDecorationConfigs = new List<MapDecorationLayerConfig>();
    public MapSpawnEnemyConfig mapSpawnEnemyConfig;
    public MapDungeonDoorConfig mapDoorConfig;
}

[Serializable]
public  class MapDecorationLayerConfig
{
    public string name;//���ơ�������
    public string layer;//��һ���������ɺ�ľ�����Ⱦ���Ĳ�����
    [Range(0,1f)]public float probaility;//���ɸ���0~1
    public int size;//ռ�ݼ��������⣬������5����ľ2����1
    public Vector2 xOffsetRange;//��������������ƫ�Ʒ�Χ
    public List<GameObject> prefab;//Ԥ�������ѡȡһ��
}

[Serializable]
public class MapDungeonDoorConfig
{
    public int chunkStep;//���ٸ���ͼ�����һ��
    public string layer; //���ɺ�ľ�����Ⱦ���Ĳ�����
    [Range(0, 1f)] public float probaility;//���ɸ���0~1
    public bool isEntrance;//�Ƿ�����ڣ���������ʲô��ͼ
    public List<GameObject> prefab;//Ԥ�������ѡȡһ��
}


[Serializable]
public class MapSpawnEnemyConfig
{
    [Range(0, 1f)] public float spawnProbability;//�Ƿ����ɵĸ���
    public Vector2Int spawnCountRange;//���������ķ�Χ
    public int minSegmentSize = 5;//��С�Ĺ������ɶ���ߴ�
    public List<GameObject> prefabs;
}
