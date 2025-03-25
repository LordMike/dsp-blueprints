using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;

namespace DysonSphereBlueprints.Gamelibs.Code.Patchwork;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public static class GameConfig
{
    private static string _gameName = "Dyson Sphere Program";
    private static string _gameAbbr = "DSP";
    private static Version _gameVersion = new(0, 10, 32);
    public static int build;
    public static string versionFilename = "Updates/Versions.txt";
    private static string systemDocument;
    public static string pathFilename = "Configs/path.txt";
    public static string overrideDocumentFolder;
    private static string gameDocument;
    private static string gameOption;
    private static string gameXMLOption;
    private static string gameSavePath;
    private static string blueprintPath;
    private static string mechaPath;
    private static string armorPath;
    private static string achievementPath;
    private static string propertyPath;
    private static string gameScreenshot;

    public static string gameName => _gameName;

    public static string gameAbbr => _gameAbbr;

    public static Version gameVersion
    {
        get => _gameVersion;
        set => _gameVersion = value;
    }

    public static List<VersionInfo> historyVersions { get; set; }

    public static string systemDocumentFolder
    {
        get
        {
            if (systemDocument == null)
                systemDocument =
                    Environment.GetFolderPath(Environment.SpecialFolder.Personal).Replace('\\', '/') + "/";
            return systemDocument;
        }
    }

    public static void InitOverrideDocumentFolder()
    {
        try
        {
            overrideDocumentFolder = File.ReadAllText(pathFilename);
        }
        catch
        {
            overrideDocumentFolder = systemDocumentFolder;
            return;
        }

        if (string.IsNullOrEmpty(overrideDocumentFolder))
        {
            overrideDocumentFolder = systemDocumentFolder;
        }
        else
        {
            overrideDocumentFolder = overrideDocumentFolder.Trim();
            if (string.IsNullOrEmpty(overrideDocumentFolder))
            {
                overrideDocumentFolder = systemDocumentFolder;
            }
            else
            {
                overrideDocumentFolder = overrideDocumentFolder.SlashDirectory();
                if (!string.IsNullOrEmpty(overrideDocumentFolder))
                    return;
                overrideDocumentFolder = systemDocumentFolder;
            }
        }
    }

    public static string gameDocumentFolder
    {
        get
        {
            if (gameDocument == null)
            {
                gameDocument = new StringBuilder(overrideDocumentFolder)
                    .Append(gameName).Append("/").ToString();
                if (!Directory.Exists(gameDocument))
                    Directory.CreateDirectory(gameDocument);
            }

            return gameDocument;
        }
    }

    public static string gameOptionPath
    {
        get
        {
            if (gameOption == null)
                gameOption = gameDocumentFolder + "options.dso";
            return gameOption;
        }
    }

    public static string gameXMLOptionPath
    {
        get
        {
            if (gameXMLOption == null)
                gameXMLOption = gameDocumentFolder + "options.xml";
            return gameXMLOption;
        }
    }

    public static string gameSaveFolder
    {
        get
        {
            if (gameSavePath == null)
            {
                gameSavePath = new StringBuilder(overrideDocumentFolder)
                    .Append(gameName).Append("/Save/").ToString();
                if (!Directory.Exists(gameSavePath))
                    Directory.CreateDirectory(gameSavePath);
            }

            return gameSavePath;
        }
    }

    public static string blueprintFolder
    {
        get
        {
            if (blueprintPath == null)
            {
                blueprintPath = new StringBuilder(overrideDocumentFolder)
                    .Append(gameName).Append("/Blueprint/").ToString();
                if (!Directory.Exists(blueprintPath))
                    Directory.CreateDirectory(blueprintPath);
            }

            return blueprintPath;
        }
    }

    public static string mechaFolder
    {
        get
        {
            if (mechaPath == null)
            {
                mechaPath = new StringBuilder(overrideDocumentFolder).Append(gameName)
                    .Append("/Customize/Mecha/").ToString();
                if (!Directory.Exists(mechaPath))
                    Directory.CreateDirectory(mechaPath);
            }

            return mechaPath;
        }
    }

    public static string armorFolder
    {
        get
        {
            if (armorPath == null)
            {
                armorPath = new StringBuilder(overrideDocumentFolder).Append(gameName)
                    .Append("/Customize/Armor/").ToString();
                if (!Directory.Exists(armorPath))
                    Directory.CreateDirectory(armorPath);
            }

            return armorPath;
        }
    }

    public static string achievementFolder
    {
        get
        {
            if (achievementPath == null)
            {
                achievementPath = new StringBuilder(overrideDocumentFolder)
                    .Append(gameName).Append("/Achievement/").ToString();
                if (!Directory.Exists(achievementPath))
                    Directory.CreateDirectory(achievementPath);
            }

            return achievementPath;
        }
    }

    public static string propertyFolder
    {
        get
        {
            if (propertyPath == null)
            {
                propertyPath = new StringBuilder(overrideDocumentFolder)
                    .Append(gameName).Append("/Property/").ToString();
                if (!Directory.Exists(propertyPath))
                    Directory.CreateDirectory(propertyPath);
            }

            return propertyPath;
        }
    }

    public static string gameScreenshotPath
    {
        get
        {
            if (gameScreenshot == null)
                gameScreenshot = new StringBuilder(overrideDocumentFolder)
                    .Append(gameName).Append("/Screenshot/").ToString();
            if (!Directory.Exists(gameScreenshot))
                Directory.CreateDirectory(gameScreenshot);
            return gameScreenshot;
        }
    }

    public static void Init() => InitOverrideDocumentFolder();
}