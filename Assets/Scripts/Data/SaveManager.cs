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

    #region 游戏数据
    //是否有游戏数据
    public static bool ExistGameData()
    {
        return File.Exists(gameDataFilePath);
    }
    //获取游戏数据
    public static GameData GetGameData()
    {
        if (!ExistGameData()) return null;
        BinaryFormatter bf = new BinaryFormatter();
        using (FileStream fileStream = File.Open(gameDataFilePath, FileMode.Open))
        {
            return (GameData)bf.Deserialize(fileStream);
        }
    }
    //保存游戏数据
    public static void SaveGameData(GameData gameData)
    {
        BinaryFormatter bf = new BinaryFormatter();
        using (FileStream fileStream = File.Create(gameDataFilePath))
        {
            bf.Serialize(fileStream, gameData);
        }
    }
    //删除游戏数据
    public static void DeleteGameData()
    {
        if (ExistGameData())
        {
            File.Delete(gameDataFilePath);
        }
    }
    #endregion

    #region 游戏设置
    //是否有游戏设置
    public static bool ExistGameSettings()
    {
        return File.Exists(gameSettingsFilePath);
    }
    //获取游戏设置
    public static GameSettings GetGameSettings()
    {
        if (!ExistGameSettings()) return null;
        BinaryFormatter bf = new BinaryFormatter();
        using (FileStream fileStream = File.Open(gameSettingsFilePath, FileMode.Open))
        {
            return (GameSettings)bf.Deserialize(fileStream);
        }
    }
    //保存游戏设置
    public static void SaveGmaeSetting(GameSettings gameSetting)
    {
        BinaryFormatter bf = new BinaryFormatter();
        using (FileStream fileStream = File.Create(gameSettingsFilePath))
        {
            bf.Serialize(fileStream, gameSetting);
        }
    }
    //删除游戏设置

    public static void DeleteGameSettings()
    {
        if (ExistGameData())
        {
            File.Delete(gameSettingsFilePath);
        }
    }
    #endregion


}
