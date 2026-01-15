using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SO_EventLibrary))]
public class Editor_SO_EventLibrary : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        EditorGUILayout.Space();

        if (GUILayout.Button("Rebuild Event Library"))
        {
            Rebuild((SO_EventLibrary)target);
        }
    }

    private static void Rebuild(SO_EventLibrary library)
    {
        string[] guids = AssetDatabase.FindAssets("t:SO_Event");

        var all = new List<SO_Event>(guids.Length);

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            var item = AssetDatabase.LoadAssetAtPath<SO_Event>(path);
            if (item != null) all.Add(item);
        }

        library.AllEvents.Clear();


        foreach (var item in all)
        {
            library.AllEvents.Add(item);
        }

        EditorUtility.SetDirty(library);
        AssetDatabase.SaveAssets();

        Debug.Log($"Rebuilt EventLibrary: {library.AllEvents.Count} total events ");
    }
}