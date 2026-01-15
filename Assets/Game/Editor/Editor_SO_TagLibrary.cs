using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SO_TagLibrary))]
public class Editor_SO_TagLibrary : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        EditorGUILayout.Space();

        if (GUILayout.Button("Rebuild Tag Library"))
        {
            Rebuild((SO_TagLibrary)target);
        }
    }

    private static void Rebuild(SO_TagLibrary library)
    {
        string[] guids = AssetDatabase.FindAssets("t:SO_DungeonTag");

        var all = new List<SO_DungeonTag>(guids.Length);

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            var tag = AssetDatabase.LoadAssetAtPath<SO_DungeonTag>(path);
            if (tag != null) all.Add(tag);
        }

        library.AllTags.Clear();
        library.BiomeTags.Clear();
        library.LocationTags.Clear();
        library.EnemyTags.Clear();

        foreach (var tag in all)
        {
            library.AllTags.Add(tag);

            switch (tag.Category)
            {
                case TagType.Biome:
                    library.BiomeTags.Add(tag);
                    break;
                case TagType.Location:
                    library.LocationTags.Add(tag);
                    break;
                case TagType.Enemy:
                    library.EnemyTags.Add(tag);
                    break;
            }
        }

        EditorUtility.SetDirty(library);
        AssetDatabase.SaveAssets();

        Debug.Log($"Rebuilt TagLibrary: {library.AllTags.Count} total tags " +
                  $"(Biome {library.BiomeTags.Count}, Location {library.LocationTags.Count}, Enemy {library.EnemyTags.Count})",
                  library);
    }
}