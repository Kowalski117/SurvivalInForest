using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TerrainManager))]
public class TerrainManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        TerrainManager tc = (TerrainManager)target;
        if (GUILayout.Button("Change trees")) tc.Correct();
        if (GUILayout.Button("Delete all trees")) tc.Delete();
    }
}
