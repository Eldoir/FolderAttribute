using UnityEngine;
using UnityEditor;
using System.IO;
using FolderInternal;

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

                if (!string.IsNullOrEmpty(property.stringValue))
                {
                    folder = AssetDatabase.LoadAssetAtPath(property.stringValue, typeof(DefaultAsset));

                    if (folder != null) // The folder exists
                    {
                        if (!Utils.FolderWithPathExists(property.stringValue)) // The folder isn't registered yet
                        {
                            string guid = AssetDatabase.AssetPathToGUID(property.stringValue);
                            Utils.RegisterNewFolder(property.stringValue, guid);
                        }
                    }
                    else // The folder has been moved, deleted, or renamed
                    {
                        string guid = Utils.GetGUIDFromPath(property.stringValue, showError: false);

                        if (!string.IsNullOrEmpty(guid))
                        {
                            string newPath = AssetDatabase.GUIDToAssetPath(guid); // The folder has been previously registered, we can recover it from its guid!
                            Utils.UpdateFolderPath(guid, newPath);
                            folder = AssetDatabase.LoadAssetAtPath(newPath, typeof(DefaultAsset));
                        }
                        else
                        {
                            Debug.LogError("The folder at path " + property.stringValue + " could not be found. It has probably been deleted.");
                        }
                    }
                }

                folder = EditorGUI.ObjectField(objectPosition, folder, typeof(DefaultAsset), false);

                string folderPathInAssets = AssetDatabase.GetAssetPath(folder);
                TryUpdateProperty(folderPathInAssets, property);
            }

            EditorGUI.EndProperty();
        }

        private void TryUpdateProperty(string folderPathInAssets, SerializedProperty property)
        {
            string folderPathOnPC = Application.dataPath.WithoutFileName() + folderPathInAssets;

            if (!Directory.Exists(folderPathOnPC))
            {
                Debug.LogError(folderPathInAssets + " is not a folder.");
            }
            else
            {
                property.stringValue = folderPathInAssets;
            }
        }
    }
}