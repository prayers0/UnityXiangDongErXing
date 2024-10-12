
using System;

[Serializable]
public class GameData
{
    public SVector3 playerMainPos;
    public SVector3 playerDungeonMapPos;
    public int mapSeed;
    public int dungeonCoord;
    public bool onMainMap;
    public BagData bagData;
}

[Serializable]
public struct SVector3//�������л�������
{
    public float x, y, z;

    public SVector3(float x,float y,float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public SVector3(UnityEngine.Vector3 v3)
    {
        this.x=v3.x; 
        this.y=v3.y; 
        this.z=v3.z;
    }

    public UnityEngine. Vector3 ToVectr3()
    {
        return new UnityEngine.Vector3(x, y, z);
    }
}

[Serializable]
public class BagData
{
    //���ӵĹ̶�����
    public const int itemCount = 25;
    //�ո��ӵı��֣�items[index]==null
    public ItemDataBase[] items=new ItemDataBase[itemCount];

    public void Swap(int aIndex, int bIndex)
    {
        ItemDataBase temp = items[aIndex];
        items[aIndex] = items[bIndex]; 
        items[bIndex] = temp;
    }
}
