using UnityEngine;
using UnityEditor;
using System.IO;

namespace Folder
{
    [CustomPropertyDrawer(typeof(FolderAttribute))]
    public class FolderAttributeDrawer : PropertyDrawer
    {
        private const int margin = 15;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            float currentViewWidth = GUILayoutUtility.GetLastRect().width;

            Rect labelPosition = position;
            labelPosition.width = EditorGUIUtility.labelWidth;

            position = EditorGUI.PrefixLabel(labelPosition, GUIUtility.GetControlID(FocusType.Passive), label);
            Rect objectPosition = position;
            objectPosition.x = labelPosition.width - (EditorGUI.indentLevel - 1) * margin;

            objectPosition.width = currentViewWidth - objectPosition.x - 4;

            if (property.propertyType != SerializedPropertyType.String)
            {
                EditorGUI.HelpBox(objectPosition, "[Folder] only works with strings.", MessageType.Error);
            }
            else
            {
                Object folder = null;
                FolderInfo folderInfo = null;

                if (!string.IsNullOrEmpty(property.stringValue))
                {
                    folderInfo = property.stringValue.ToFolderInfo();
                    folder = AssetDatabase.LoadAssetAtPath(folderInfo.path, typeof(DefaultAsset));

                    if (folder == null) // The folder has been moved, deleted, or renamed
                    {
                        string newPath = AssetDatabase.GUIDToAssetPath(folderInfo.guid); // Try to retrieve from guid

                        if (newPath != folderInfo.path)
                        {
                            folderInfo.path = newPath;
                            folder = AssetDatabase.LoadAssetAtPath(folderInfo.path, typeof(DefaultAsset));
                        }
                        else // Retrieving from the guid led us to the same path: the file could not be found
                        {
                            Debug.LogWarning("The folder at path " + folderInfo.path + " could not be found. It has probably been deleted.");
                            folderInfo = null; // Reset folderInfo
                        }
                    }
                }
                else // The property is unassigned yet
                {
                    //Debug.Log("unassigned");
                }

                folder = EditorGUI.ObjectField(objectPosition, folder, typeof(DefaultAsset), false);

                string folderPathInAssets = AssetDatabase.GetAssetPath(folder);
                TryUpdateProperty(folderPathInAssets, folderInfo, property);
            }

            EditorGUI.EndProperty();
        }

        private void TryUpdateProperty(string folderPathInAssets, FolderInfo folderInfo, SerializedProperty property)
        {
            string folderPathOnPC = Application.dataPath.WithoutFileName() + folderPathInAssets;

            if (!Directory.Exists(folderPathOnPC))
            {
                Debug.LogError(folderPathInAssets + " is not a folder.");
            }
            else
            {
                if (folderInfo == null)
                {
                    if (!string.IsNullOrEmpty(folderPathInAssets)) // First time we assign a folder
                    {
                        folderInfo = new FolderInfo { guid = AssetDatabase.AssetPathToGUID(folderPathInAssets), path = folderPathInAssets };
                        property.stringValue = JsonUtility.ToJson(folderInfo);
                    }
                    else // Folder was deleted
                    {
                        property.stringValue = "";
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(folderPathInAssets)) // We just removed the reference
                    {
                        property.stringValue = "";
                    }
                    else if (folderPathInAssets != folderInfo.path) // We assigned a new folder
                    {
                        folderInfo = new FolderInfo { guid = AssetDatabase.AssetPathToGUID(folderPathInAssets), path = folderPathInAssets };
                        property.stringValue = JsonUtility.ToJson(folderInfo);
                    }
                    else // We renamed the folder
                    {
                        property.stringValue = JsonUtility.ToJson(folderInfo);
                    }
                }
            }
        }
    }
}