using UnityEditor;

class CustomAssetPostprocessor : AssetPostprocessor
{
    static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        if (deletedAssets.Length > 0)
        {
            foreach (string sourcePath in deletedAssets)
            {
                if (sourcePath.LastIndexOf("/") > sourcePath.LastIndexOf(".")) // It's a folder
                {
                    FolderInternal.Utils.RemoveFolderWithPath(sourcePath);
                }
            }
        }
    }
}