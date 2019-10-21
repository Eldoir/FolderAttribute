using UnityEngine;
using UnityEditor;
using System.IO;
using Folder;

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
            EditorGUI.HelpBox(objectPosition, "[Folder] works only with variables of type string.", MessageType.Error);
        }
        else
        {

            Object folder = null;

            if (!string.IsNullOrEmpty(property.stringValue))
            {
                folder = AssetDatabase.LoadAssetAtPath(property.stringValue, typeof(DefaultAsset));
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