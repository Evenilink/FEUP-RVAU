using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(MeshGenerator))]
public class ObjectBuilderEditor : Editor {
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        MeshGenerator generator = (MeshGenerator)target;
        if (GUILayout.Button("Generate")) {
            generator.GenerateMesh();
        }
    }
}