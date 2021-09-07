using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(MeshFilter))]
public class vtkRenderer : MonoBehaviour
{
    Mesh mesh;
    public Gradient grad;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<MeshFilter>().mesh = mesh;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
