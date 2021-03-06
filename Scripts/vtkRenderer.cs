using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class vtkRenderer : MonoBehaviour
{
    Mesh mesh;
    public Gradient grad;
    GradientColorKey[] colorKey;
    GradientAlphaKey[] alphaKey;
    public float[] bic;
    public float factor = 1;
    bool single = false;
    public bool play = false;
    public int frameTimeMili = 400;
    public int currentVector = -1;
    public int currentScalar =-1;
    public string[] frames;
    public string[] vectors = null;
    public string[] scalars = null;
    public bool showVertex = false;
    public bool autoScale = false;
    private bool oneShot = true;
    private GameObject verteces;

    public void SetFrames(string[] frames)
    {
        this.frames = frames;
        CreateMesh();
        Render();
    }
    public void CreateMesh()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        grad = new Gradient();
        verteces = new GameObject();
        verteces.transform.parent = this.transform;
        // Populate the color keys at the relative time 0 and 1 (0 and 100%)
        colorKey = new GradientColorKey[2];
        colorKey[0].color = Color.red;
        colorKey[0].time = 0.0f;
        colorKey[1].color = Color.blue;
        colorKey[1].time = 1.0f;

        // Populate the alpha  keys at relative time 0 and 1  (0 and 100%)
        alphaKey = new GradientAlphaKey[2];
        alphaKey[0].alpha = 1.0f;
        alphaKey[0].time = 0.0f;
        alphaKey[1].alpha = 1.0f;
        alphaKey[1].time = 1.0f;

        grad.SetKeys(colorKey, alphaKey);
    }

    // Update is called once per frame
    public void Render(bool lessMem = false)
    {
        StopAllCoroutines();
        vtkObj[] objs = vtkReader.Read(frames, vectors, scalars);
        StartCoroutine(AnimateCoroutine(objs));
    }

    // Animation
    IEnumerator AnimateCoroutine(vtkObj[] frames)
    {
        int i = 0;
        int frameCount = frames.Length;
        Debug.Log("Frame count: " + frameCount);

        do
        {
            mesh.Clear();
            mesh.vertices = Vertices(frames[i], showVertex);
            mesh.triangles = frames[i].tris;
            mesh.colors = ColorMap(frames[i]);
            mesh.RecalculateNormals();
            if (autoScale && oneShot && i == 0)
            {
                Debug.LogWarning("Object size: " + GetComponent<MeshRenderer>().bounds.size);
                Vector3 v3 = GetComponent<MeshRenderer>().bounds.size;
                float size = Mathf.Max(Mathf.Max(v3.x, v3.y), v3.z);
                Debug.LogWarning("Max size: " + size);

                Debug.LogWarning("Current scale: " + gameObject.transform.localScale);
                gameObject.transform.localScale = 0.3f * Vector3.one / size;
                Debug.LogWarning("New scale: " + gameObject.transform.localScale);
                oneShot = false;
            }
            i++;
            i %= frameCount;
            yield return new WaitForSeconds(frameTimeMili / 1000.0f);
            if (!play)
                yield return new WaitUntil(new System.Func<bool>(() => play));
        } while (true);
    }
    IEnumerator AnimateCoroutine(string[] frames, string[] vectors = null, string[] scalars = null)
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
    Color[] ColorMap(vtkObj data)
    {

        //Coloring
        //There need to be three types of coloring
        //Vector field, Scalar field and none
        float[] vals = new float[data.points.Length];
        //Easiest first NONE
       
        if (data.is2D && currentScalar != -1)
        {
            vals = data.scals[currentScalar];
        }
        if (!data.is2D && currentVector != -1)
        {
            for (int i = 0; i < data.points.Length; i++)
            {
                vals[i] = data.vecs[currentVector][i].x;
            }
        }
        int vertexCount = vals.Length;
        Color[] color = new Color[vertexCount];
        float max = vals.Max();
        float min = vals.Min();
        for (int j = 0; j < vertexCount; j++)
        {
            if (max != min)
            {
                float c = (vals[j] - min) / (max - min);
                color[j] = grad.Evaluate(c);
            }
            else
                color[j] = grad.Evaluate(0);
        }
        return color;

    }
    public List<GameObject> CreateSpheres(Vector3[] vertices)
    {
        float threshold = 0.0f;
        List<GameObject> cubes = new List<GameObject>();
        foreach (Vector3 vert in vertices)
        {
            if (cubes.Any(sph => (sph.transform.position - vert).magnitude < threshold)) continue;
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.position = vert;
            cube.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
            cube.transform.parent = verteces.transform;
            cubes.Add(cube);
        }
        return cubes;
    }
    Vector3[] Vertices(vtkObj data, bool showVertex)
    {
        int len = data.points.Length;
        Vector3[] ret = new Vector3[len];
        if (data.is2D && currentScalar != -1)
        {
            for (int i = 0; i < len; i++)
            {
                ret[i] = data.points[i] + new Vector3(0, factor * data.scals[currentScalar][i]);
            }
        }
        else if (!data.is2D && currentVector != -1)
        {
            for (int i = 0; i < len; i++)
            {
                ret[i] = data.points[i] + factor * data.vecs[currentVector][i];
            }
        }
        else
        {
            if (showVertex)
                CreateSpheres(data.points);
            return data.points;
        }
        if (showVertex)
            CreateSpheres(ret);
        return ret;
    }

    //Vector fields
    //Calculates value from vector field
    //call vec field, then pass to color and verts

    //Scalar fields
    //just returns maybe
    //call Scalar field, then pass to color and verts

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
