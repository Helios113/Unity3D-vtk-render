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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
