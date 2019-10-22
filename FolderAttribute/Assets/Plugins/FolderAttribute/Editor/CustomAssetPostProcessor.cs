using UnityEditor;

class CustomAssetPostprocessor : AssetPostprocessor
{
    static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        if (movedAssets.Length > 0)
        {
            for (int i = 0; i < movedAssets.Length; i++)
            {
                string destinationPath = movedAssets[i];

                if (IsFolder(destinationPath))
                {
                    string guid = AssetDatabase.AssetPathToGUID(destinationPath);
                    FolderInternal.Utils.RegisterNewFolder(destinationPath, guid);
                }
            }
        }
    }

    static bool IsFolder(string sourcePath)
    {
        return sourcePath.LastIndexOf("/") > sourcePath.LastIndexOf(".");
    }
}