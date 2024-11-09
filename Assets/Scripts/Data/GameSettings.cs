using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameSettings
{
    public float volume;
    public ResolutionType resolutionType;
    public bool fullScreen;

    public static List<string> ResolutionTypeString = new List<string>()
    {
        "1920 X 1080",
        "1280 X 720"
    };

    public Vector2Int GetScreenResolution()
    {
        int screenWidth = 0;
        int screenHeight = 0;
        switch (resolutionType)
        {
            case ResolutionType.R1920X1080:
                screenWidth = 1920;screenHeight = 1080;
                break;
            case ResolutionType.R1280X720:
                screenWidth = 1280;screenHeight = 720;
                break;
        }
        return new Vector2Int(screenWidth, screenHeight);
    }
}

public enum ResolutionType
{
    R1920X1080,
    R1280X720
}

