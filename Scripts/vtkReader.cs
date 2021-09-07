using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vtkReader : MonoBehaviour
{
    // Start is called before the first frame update
    static public vtkObj[] Read(string[] path, string[] vectors = null)
    {
        int len = path.Length;
        vtkObj[] objects = new vtkObj[len];
        for (int i =0;i<len; i++)
        {
            objects[i] = Read(path[i], vectors);
        }
        return objects;

    }

    static public vtkObj Read(string path, string[] vectors)
    {
        System.IO.StreamReader file = new System.IO.StreamReader(path);
        vtkObj obj = vtkParser.Parse(file, true, vectors);
        file.Close();
        return obj;
    }




}
