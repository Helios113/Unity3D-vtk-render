using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(MeshFilter))]
public class vtkRenderer : MonoBehaviour
{
    Mesh mesh;
    public Gradient grad;
    bool single = false;
    bool play = false;
    int frameTimeMili = 1000;
    void Start()
    {
        GetComponent<MeshFilter>().mesh = mesh;
    }

    // Update is called once per frame
    void Render(string[] frames, string[] vectors = null, bool lessMem = false)
    {
       StopAllCoroutines();
       if(lessMem)
            StartCoroutine(AnimateCoroutine(frames,vectors));
       else
        {
            vtkObj[] objs = vtkReader.Read(frames, vectors);
            StartCoroutine(AnimateCoroutine(objs));
        }

    }

    // Animation
    IEnumerator AnimateCoroutine(vtkObj[] frames)
    {
        int i = 0;
        int len = frames.Length;
        while (true)
        {
            mesh.vertices = frames[i].points;
            mesh.triangles = frames[i].tris;
            //mesh.colors = frames[i].colors;
            mesh.RecalculateNormals();
            i++;
            i %= len;
            if (!play)
                yield return new WaitUntil(new System.Func<bool>(() => play));
            yield return new WaitForSeconds(frameTimeMili/1000.0f);
        }
    }
    IEnumerator AnimateCoroutine(string[] frames, string[] vectors = null)
    {
        int i = 0;
        int len = frames.Length;
        vtkObj obj;
        while (true)
        {
            obj = vtkReader.Read(frames[i], vectors);
            mesh.vertices = obj.points;
            mesh.triangles = obj.tris;
            //mesh.colors = obj[i].colors;
            mesh.RecalculateNormals();
            i++;
            i %= len;
            if (!play)
                yield return new WaitUntil(new System.Func<bool>(() => play));
            yield return new WaitForSeconds(frameTimeMili / 1000.0f);
        }
    }

    //Colouring



    //Communication
    public void SetFrameTime(int time)
    {
        frameTimeMili = time;
    }
    public void TogglePlay()
    {
        play = !play;
    }
    public void ToggleMode()
    {
        single = !single;
    }
}
