using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveManager
{
    private static string filePath;

    static SaveManager()
    {
        filePath = $"{Application.persistentDataPath}/{nameof(GameData)}";
    }

    //是否有游戏数据
    public static bool ExistGameData()
    {
        return File.Exists(filePath);
    }
    //获取游戏数据
    public static GameData GetGameData()
    {
        if(!ExistGameData()) return null;
        BinaryFormatter bf=new BinaryFormatter();
        using(FileStream fileStream = File.Open(filePath, FileMode.Open))
        {
            return (GameData)bf.Deserialize(fileStream);
        }
    }
    //保存游戏数据
    public static void SaveGameData(GameData gameData)
    {
        BinaryFormatter bf = new BinaryFormatter();
        using (FileStream fileStream = File.Create(filePath))
        {
            bf.Serialize(fileStream,gameData);
        }
    }
    //删除游戏数据
    public static void DeleteGameData()
    {
        if (ExistGameData())
        {
            File.Delete(filePath);
        }
    }
}
