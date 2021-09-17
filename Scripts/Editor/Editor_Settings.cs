using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(vtkRenderer))]
public class Editor_Settings : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        vtkRenderer myScript = (vtkRenderer)target;
        if (GUILayout.Button("Play"))
        {
            myScript.Render();
        }
    }

}
