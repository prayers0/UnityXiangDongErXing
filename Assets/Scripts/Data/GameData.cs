
using System;
using System.Numerics;
using Unity.VisualScripting;
using Vector3 = System.Numerics.Vector3;

[Serializable]
public class GameData
{
    public SVector3 playerPos;
    public int mapSeed;
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

    public SVector3(Vector3 v3)
    {
        this.x=v3.X; 
        this.y=v3.Y; 
        this.z=v3.Z;
    }

    public UnityEngine. Vector3 ToVectr3()
    {
        return new UnityEngine.Vector3(x, y, z);
    }
}
