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
    public int chunkSize = 50;//地图块的尺寸
    public Vector2Int chunkSegmentSizeRange = new Vector2Int(2, 11);//地图块的尺寸2~10
    //生成时每一层之间互不干扰
    public List<MapDecorationLayerConfig> mapDecorationConfigs = new List<MapDecorationLayerConfig>();
}

[Serializable]
public  class MapDecorationLayerConfig
{
    public string name;//名称。无意义
    public string layer;//这一层物体生成后的精灵渲染器的层设置
    public float probaility;//生成概率0~1
    public int size;//占据几个格子兮，建筑物5，树木2花草1
    public Vector2 xOffsetRange;//格子内物体的随机偏移范围
    public List<GameObject> prefab;//预制体随机选取一个

}
