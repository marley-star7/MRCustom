/*
using System.IO;

namespace MRCustom;

public static class AssetLoader
{
    public static FAtlas GetAtlas(string atlasesDirPath, string atlasName)
    {
        string atlasDirPath = Path.Combine(atlasesDirPath, atlasName);
        if (Futile.atlasManager.DoesContainAtlas(atlasDirPath))
        {
            return Futile.atlasManager.LoadAtlas(atlasDirPath);
        }
        Plugin.LogError("Atlas not found! (" + atlasName + ")");
        return null;
    }

    private static void LoadAtlases(string targetDirPath)
    {
        foreach (string filePath in AssetManager.ListDirectory(targetDirPath, false, false, false))
        {
            if (!(Path.GetExtension(filePath).ToLower() != ".txt"))
            {
                string atlasFileName = Path.GetFileNameWithoutExtension(filePath);
                string atlasPath = Path.Combine(targetDirPath, atlasFileName);
                Futile.atlasManager.LoadAtlas(atlasPath);
            }
        }

        foreach (string dirPath in AssetManager.ListDirectory(targetDirPath, true, false, false))
        {
            AssetLoader.LoadAtlases(dirPath);
        }
    }

    private static void LoadSprites(string targetDirPath)
    {
        foreach (string filePath in AssetManager.ListDirectory(targetDirPath, false, false, false))
        {
            if (!(Path.GetExtension(filePath).ToLower() != ".png"))
            {
                string spriteName = Path.GetFileNameWithoutExtension(filePath);
                string spriteFilePath = filePath.TrimEnd(Path.GetExtension(filePath));
                Futile.atlasManager.ActuallyLoadAtlasOrImage(spriteName, spriteFilePath + Futile.resourceSuffix, "");
            }
        }

        foreach (string dirPath in AssetManager.ListDirectory(targetDirPath, true, false, false))
        {
            AssetLoader.LoadSprites(dirPath);
        }
    }
}
*/