using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

[RequireComponent(typeof(vtkRenderer))]
public class vtkController : MonoBehaviour
{


    public string[] paths;
    public string[] vectors;
    public string[] scalars;
    public int frameTimeMilliseconds;
    public bool saveMemory;
    public float vectorScale;
    public int currentVector;
    vtkRenderer renderer_vtk;
    
    // Start is called before the first frame update
    void Start()
    {
        TextAsset textFile = Resources.Load<TextAsset>("/out_result_0");
        print(textFile.name);
        print(textFile.text);
        /*
        Application.targetFrameRate = 72;
        renderer_vtk = GetComponent<vtkRenderer>();
        renderer_vtk.GetMesh();
        renderer_vtk.SetFrameTime(frameTimeMilliseconds);
        renderer_vtk.TogglePlay();
        renderer_vtk.Render(fLines, vectors, scalars);
        */
    }

    void Update()
    {
    }
    // Update is called once per frame

}
