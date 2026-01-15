using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SO_TraitLibrary))]
public class Editor_SO_TraitLibrary : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        EditorGUILayout.Space();

        if (GUILayout.Button("Rebuild Trait Library"))
        {
            Rebuild((SO_TraitLibrary)target);
        }
    }

    private static void Rebuild(SO_TraitLibrary library)
    {
        string[] guids = AssetDatabase.FindAssets("t:SO_Trait");

        var all = new List<SO_Trait>(guids.Length);

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            var item = AssetDatabase.LoadAssetAtPath<SO_Trait>(path);
            if (item != null) all.Add(item);
        }

        library.AllTraits.Clear();


        foreach (var item in all)
        {
            library.AllTraits.Add(item);
        }

        EditorUtility.SetDirty(library);
        AssetDatabase.SaveAssets();

        Debug.Log($"Rebuilt TraitLibrary: {library.AllTraits.Count} total events ");
    }
}