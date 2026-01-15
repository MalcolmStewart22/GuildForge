using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SO_DungeonTag))]
public class TagDefinitionSOEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        DrawPropertiesExcluding(serializedObject, "DefaultEnemy");
        SO_DungeonTag tag = (SO_DungeonTag)target;

        if (tag.Category == TagType.Biome)
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Biome Settings", EditorStyles.boldLabel);

            EditorGUILayout.PropertyField
            (
                serializedObject.FindProperty("DefaultEnemy"),
                new GUIContent("Default Enemy")
            );

        };
        serializedObject.ApplyModifiedProperties();
    }
}