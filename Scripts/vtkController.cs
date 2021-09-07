using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(vtkRenderer))]
public class vtkController : MonoBehaviour
{

    public string[] paths;
    public string[] vectors;
    public int frameTimeMilliseconds;
    public bool saveMemory;
    public float vectorScale;
    public int currentVector;
    vtkRenderer renderer_vtk;
    
    // Start is called before the first frame update
    void Start()
    {
        renderer_vtk = GetComponent<vtkRenderer>();
        renderer_vtk.GetMesh();
        renderer_vtk.SetFrameTime(frameTimeMilliseconds);
        renderer_vtk.TogglePlay();
        renderer_vtk.Render(paths, vectors);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
