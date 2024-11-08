
using System;
using UnityEditor.Experimental.GraphView;

[Serializable]
public class GameData
{
    public SVector3 playerMainPos;
    public SVector3 playerDungeonMapPos;
    public int mapSeed;
    public int dungeonCoord;
    public bool onMainMap;
    public BagData bagData;
    public int coinCount;
    public float playerHp;
}

[Serializable]
public struct SVector3//可以序列化的向量
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
    //格子的固定数量
    public const int itemCount = 25;
    //空格子的表现，items[index]==null
    public ItemDataBase[] items=new ItemDataBase[itemCount];
    //已经使用的武器索引
    public int usedWeaponIndex;

    public WeaponData useWeaponData =>(WeaponData) items[usedWeaponIndex];

    public void Swap(int aIndex, int bIndex)
    {
        ItemDataBase temp = items[aIndex];
        items[aIndex] = items[bIndex]; 
        items[bIndex] = temp;

        if(aIndex==usedWeaponIndex) usedWeaponIndex = bIndex;
        else if(bIndex==usedWeaponIndex) usedWeaponIndex = aIndex;
    }

    public bool TryGetItem(string id, out int existIndex)
    {
        for(int i = 0; i < items.Length; i++)
        {
            if (items[i] != null && items[i].id == id)
            {
                existIndex = i;
                return true;
            }
        }
        existIndex = -1;
        return false;
    }
}
