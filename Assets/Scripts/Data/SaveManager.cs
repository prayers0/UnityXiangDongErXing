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

    //�Ƿ�����Ϸ����
    public static bool ExistGameData()
    {
        return File.Exists(filePath);
    }
    //��ȡ��Ϸ����
    public static GameData GetGameData()
    {
        if(!ExistGameData()) return null;
        BinaryFormatter bf=new BinaryFormatter();
        using(FileStream fileStream = File.Open(filePath, FileMode.Open))
        {
            return (GameData)bf.Deserialize(fileStream);
        }
    }
    //������Ϸ����
    public static void SaveGameData(GameData gameData)
    {
        BinaryFormatter bf = new BinaryFormatter();
        using (FileStream fileStream = File.Create(filePath))
        {
            bf.Serialize(fileStream,gameData);
        }
    }
    //ɾ����Ϸ����
    public static void DeleteGameData()
    {
        if (ExistGameData())
        {
            File.Delete(filePath);
        }
    }
}
