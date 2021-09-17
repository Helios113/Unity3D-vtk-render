using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(VtkConvert))]
public class Editor_convert : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        VtkConvert myScript = (VtkConvert)target;
        if (GUILayout.Button("Convert"))
        {
            myScript.make();
        }
    }

}
