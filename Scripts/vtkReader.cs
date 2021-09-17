using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vtkReader : MonoBehaviour
{
    static public vtkObj[] Read(string[] path, string[] vectors = null, string[] scalars = null)
    {

        int len = path.Length;
        vtkObj[] objects = new vtkObj[len];
        for (int i =0;i<len; i++)
        {
            objects[i] = Read(path[i], vectors, scalars);
        }
        return objects;

    }

    static public vtkObj Read(string path, string[] vectors=null, string[] scalars = null)
    {
        TextAsset ta = Resources.Load(path) as TextAsset;
        vtkObj obj = vtkParser.Parse(ta.text, vectors, scalars);
        return obj;
    }




}
