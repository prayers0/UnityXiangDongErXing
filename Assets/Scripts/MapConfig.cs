using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName ="Config/MapConfig")]
public class MapConfig : ScriptableObject
{
    public TileBase groundTile;
    public float mapChunkDestroyTime = 5;
    public int chunkSize = 50;//��ͼ��ĳߴ�
    public Vector2Int chunkSegmentSizeRange = new Vector2Int(2, 11);//��ͼ��ĳߴ�2~10
    //����ʱÿһ��֮�以������
    public List<MapDecorationLayerConfig> mapDecorationConfigs = new List<MapDecorationLayerConfig>();
}

[Serializable]
public  class MapDecorationLayerConfig
{
    public string name;//���ơ�������
    public string layer;//��һ���������ɺ�ľ�����Ⱦ���Ĳ�����
    public float probaility;//���ɸ���0~1
    public int size;//ռ�ݼ��������⣬������5����ľ2����1
    public Vector2 xOffsetRange;//��������������ƫ�Ʒ�Χ
    public List<GameObject> prefab;//Ԥ�������ѡȡһ��

}
