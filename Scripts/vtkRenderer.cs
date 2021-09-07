using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class vtkRenderer : MonoBehaviour
{
    Mesh mesh;
    public Gradient grad;
    public Color[] col;
    public float[] bic;
    public float factor = 1;
    bool single = false;
    bool play = false;
    int frameTimeMili = 1000;
    int currentVector = 0;
    public void GetMesh()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
    }

    // Update is called once per frame
    public void Render(string[] frames, string[] vectors = null, bool lessMem = false)
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
            mesh.Clear();
            Vector3[] a = new Vector3[frames[i].points.Length];
            //Debug.LogError(frames[i].colors.Length);
            //Debug.LogError(frames[i].colors[0].Length);
            //Debug.LogError(frames[i].points.Length);
            //Debug.LogError(currentVector);
            for (int j=0;j < frames[i].points.Length;j++)
            {
                //Debug.Log(frames[i].points[j].ToString()+" + "+ frames[i].colors[currentVector][j].ToString());
                a[j] = frames[i].points[j] + factor*frames[i].colors[currentVector][j];
            }
            mesh.vertices = a;
            mesh.triangles = frames[i].tris;
            bic = new float[frames[i].points.Length];
            float max = float.MinValue;
            float min = float.MaxValue;
            for (int j = 0; j < frames[i].points.Length; j++)
            {
                bic[j] = Mathf.Sqrt(Mathf.Pow(frames[i].colors[currentVector][j].x, 2) + Mathf.Pow(frames[i].colors[currentVector][j].y, 2) + Mathf.Pow(frames[i].colors[currentVector][j].z, 2));
                //bic[j] = frames[i].colors[currentVector][j].x;
                if (bic[j] > max)
                {
                    max = bic[j];
                }
                if(bic[j] < min)
                {
                    min = bic[j];
                }
            }
            col = new Color[frames[i].points.Length];
            for (int j = 0; j < frames[i].points.Length; j++)
            {
                if (max != min)
                {
                    float c = (bic[j] - min) / (max - min);
                    col[j] = grad.Evaluate(c);
                }
                else
                    col[j] = grad.Evaluate(0);
            }

            mesh.colors = col;
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
    public void SetVector(int a)
    {
        currentVector = a;
    }
}
