using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveManager
{
    private static string gameDataFilePath;
    private static string gameSettingsFilePath;

    static SaveManager()
    {
        gameDataFilePath = $"{Application.persistentDataPath}/{nameof(GameData)}";
        gameSettingsFilePath = $"{Application.persistentDataPath}/{nameof(GameSettings)}";
    }

    #region ��Ϸ����
    //�Ƿ�����Ϸ����
    public static bool ExistGameData()
    {
        return File.Exists(gameDataFilePath);
    }
    //��ȡ��Ϸ����
    public static GameData GetGameData()
    {
        if (!ExistGameData()) return null;
        BinaryFormatter bf = new BinaryFormatter();
        using (FileStream fileStream = File.Open(gameDataFilePath, FileMode.Open))
        {
            return (GameData)bf.Deserialize(fileStream);
        }
    }
    //������Ϸ����
    public static void SaveGameData(GameData gameData)
    {
        BinaryFormatter bf = new BinaryFormatter();
        using (FileStream fileStream = File.Create(gameDataFilePath))
        {
            bf.Serialize(fileStream, gameData);
        }
    }
    //ɾ����Ϸ����
    public static void DeleteGameData()
    {
        if (ExistGameData())
        {
            File.Delete(gameDataFilePath);
        }
    }
    #endregion

    #region ��Ϸ����
    //�Ƿ�����Ϸ����
    public static bool ExistGameSettings()
    {
        return File.Exists(gameSettingsFilePath);
    }
    //��ȡ��Ϸ����
    public static GameSettings GetGameSettings()
    {
        if (!ExistGameSettings()) return null;
        BinaryFormatter bf = new BinaryFormatter();
        using (FileStream fileStream = File.Open(gameSettingsFilePath, FileMode.Open))
        {
            return (GameSettings)bf.Deserialize(fileStream);
        }
    }
    //������Ϸ����
    public static void SaveGmaeSetting(GameSettings gameSetting)
    {
        BinaryFormatter bf = new BinaryFormatter();
        using (FileStream fileStream = File.Create(gameSettingsFilePath))
        {
            bf.Serialize(fileStream, gameSetting);
        }
    }
    //ɾ����Ϸ����

    public static void DeleteGameSettings()
    {
        if (ExistGameData())
        {
            File.Delete(gameSettingsFilePath);
        }
    }
    #endregion


}
