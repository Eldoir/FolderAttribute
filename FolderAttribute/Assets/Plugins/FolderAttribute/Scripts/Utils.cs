using UnityEngine;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Folder;

namespace FolderInternal
{
    public static class Utils
    {
        private const string dataFilename = "folder_changes.txt";
        private static string dataFilepath { get { return Path.Combine(Application.persistentDataPath, dataFilename); } }

        private static List<FolderInfo> _folders;
        private static List<FolderInfo> folders
        {
            get
            {
                if (_folders == null)
                    _folders = GetFolders();

                return _folders;
            }
        }

        public static void UpdateFolderPath(string guid, string filepath)
        {
            CreateFoldersFileIfNotExists();

            FolderInfo folder = GetFolderWithGUID(guid);

            if (folder != null)
            {
                folder.path = filepath;
                SaveFolders();
            }
            else
            {
                Debug.LogError("Folder with guid " + guid + " could not be found.");
            }
        }

        public static void RegisterNewFolder(string filepath, string guid)
        {
            CreateFoldersFileIfNotExists();

            FolderInfo folderInfo = GetFolderWithGUID(guid);

            if (folderInfo == null) // FolderInfo creation
            {
                folders.Add(new FolderInfo { path = filepath, guid = guid });
            }
            else // Simple path update
            {
                folderInfo.path = filepath;
            }

            SaveFolders();
        }

        public static void RemoveFolderWithPath(string filepath)
        {
            _folders = _folders.Where(f => f.path != filepath).ToList();
            SaveFolders();
        }

        public static bool FolderWithPathExists(string path)
        {
            return GetFolderWithPath(path) != null;
        }

        public static bool FolderWithGUIDExists(string guid)
        {
            return GetFolderWithGUID(guid) != null;
        }

        public static string GetGUIDFromPath(string path, bool showError = true)
        {
            FolderInfo folder = GetFolderWithPath(path);

            if (folder != null)
                return folder.guid;
            else
            {
                if (showError)
                    Debug.LogError("Folder with path " + path + " could not be found.");

                return "";
            }
        }

        public static FolderInfo GetFolderWithPath(string path)
        {
            CreateFoldersFileIfNotExists();
            return folders.FirstOrDefault(f => f.path == path);
        }

        public static FolderInfo GetFolderWithGUID(string guid)
        {
            CreateFoldersFileIfNotExists();
            return folders.FirstOrDefault(f => f.guid == guid);
        }

        private static void CreateFoldersFileIfNotExists()
        {
            if (!File.Exists(dataFilepath))
            {
                File.WriteAllText(dataFilepath, string.Empty);
            }
        }

        private static List<FolderInfo> GetFolders()
        {
            CreateFoldersFileIfNotExists();

            string[] lines = File.ReadAllLines(dataFilepath);

            List<FolderInfo> folders = new List<FolderInfo>();

            for (int i = 0; i < lines.Length; i++)
            {
                string[] folderInfos = lines[i].Split(new char[] { ' ' }, 2, StringSplitOptions.None);
                folders.Add(new FolderInfo { guid = folderInfos[0], path = folderInfos[1] });
            }

            return folders;
        }

        private static void SaveFolders()
        {
            string content = "";

            for (int i = 0; i < folders.Count; i++)
            {
                content += folders[i] + Environment.NewLine;
            }

            File.WriteAllText(dataFilepath, content);
        }
    }
}