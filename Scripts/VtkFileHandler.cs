using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Linq;

public class VtkFileHandler : MonoBehaviour
{
    public TextAsset[] vtkObjs;
    public Shader colorShader;
    public GameObject[] renderObjects;
    public Text text;
    public CameraFollow cam;
    void Start()
    {
        string[] frames;
        renderObjects = new GameObject[vtkObjs.Length];

        for (int i=0; i<vtkObjs.Length; i++)
        {
            frames = vtkObjs[i].text.Split(',');
            CreateRenderObject(i);
            renderObjects[i].GetComponent<vtkRenderer>().SetFrames(frames);
            cam.target = renderObjects[i].transform;
            
        }

    }

    void CreateRenderObject(int i)
    {
        renderObjects[i] = new GameObject();
        renderObjects[i].AddComponent<MeshFilter>();
        renderObjects[i].AddComponent<MeshRenderer>();
        renderObjects[i].GetComponent<MeshRenderer>().material = new Material(colorShader);
        renderObjects[i].AddComponent<vtkRenderer>();
    }
}
