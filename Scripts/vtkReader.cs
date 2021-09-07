using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vtkReader : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        System.IO.StreamReader file = new System.IO.StreamReader(Application.dataPath + "/out_skin_100.vtk"); //load text file with data
        obj[0] = Read(file);
        file.Close();
        mesh.vertices = obj[0].points;
        mesh.triangles = obj[0].tris;
        mesh.colors = obj[0].colors;
        mesh.RecalculateNormals();
        stopWatch.Stop();
        Debug.Log("Time taken: " + stopWatch.ElapsedMilliseconds);
        file = new System.IO.StreamReader(Application.dataPath + "/out_skin_200.vtk"); //load text file with data
        obj[1] = Read(file);
        file.Close();
        file = new System.IO.StreamReader(Application.dataPath + "/out_skin_300.vtk"); //load text file with data
        obj[2] = Read(file);
        file.Close();
        file = new System.IO.StreamReader(Application.dataPath + "/out_skin_400.vtk"); //load text file with data
        obj[2] = Read(file);
        file.Close();
        file = new System.IO.StreamReader(Application.dataPath + "/out_skin_500.vtk"); //load text file with data
        obj[2] = Read(file);
        file.Close();

        StartCoroutine(ExampleCoroutine());

    }
    IEnumerator ExampleCoroutine()
    {
        //Print the time of when the function is first called.
        Debug.Log("Started Coroutine at timestamp : " + Time.time);
        int i = 0;
        while (true)
        {
            mesh.vertices = obj[i].points;
            mesh.triangles = obj[i].tris;
            mesh.colors = obj[i].colors;
            mesh.RecalculateNormals();
            i++;
            i %= 3;
            //yield on a new YieldInstruction that waits for 5 seconds.
            yield return new WaitForSeconds(1);
        }
    }


}
