using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using System.Linq;
using System;

public class vtkParser : MonoBehaviour
{
    // Start is called before the first frame update
    
    Vector3[] vertices;
    public Color[] colors;
    int[] tris;
    int vertixSize = 0;
    int cellSize = 0;

    float min = float.MaxValue;
    float max = float.MinValue;
    
    vtkObj Parse(System.IO.StreamReader file, bool lessMem = false)
    {
        string line;
        dynamic displVector;
        if (lessMem)
            displVector = new Vector3[] { };
        else
            displVector = new Vector4[] { };
        List<int>[] cells = new List<int>[] { };
        Vector3[] points = new Vector3[] { };
        int[] cellTypes = new int[] { };
        int[] tris = new int[] { };
        while ((line = file.ReadLine()) != null)
        {
            if (line.Contains("POINTS"))
            {
                int num = Convert.ToInt32(line.Split(' ')[1]);
                Debug.Log("Points size: "+num);
                points = GetPoints(file, num);
                vertixSize = num;
                displVector = new Color[num];
            }
            if (line.Contains("CELLS"))
            {
                int num = Convert.ToInt32(line.Split(' ')[1]);
                cellSize = num;
                Debug.Log("Cell size: " + num);
                cells = GetCells(file, num);
            }
            if (line.Contains("CELL_TYPES"))
            {
                int num = Convert.ToInt32(line.Split(' ')[1]);
                Debug.Log("Cell type size: " + num);
                cellTypes = GetCellTypes(file, num);
            }
            if (line.Contains("VECTORS DISPLACEMENT"))
            {
                displVector = GetColors(file);
            }
        }
        if (cells.Length != 0 && cellTypes.Length != 0)
        {
            tris = vtkCellToTris.GetTrianglesFromData(cellSize, cells, cellTypes).ToArray<int>();
            for (int i = 0; i < vertixSize; i++)
            {
                colors[i] = grad.Evaluate((displVector[i].x - min) / (max - min));
            }
        }
        return new vtkObj(points, tris, colors);

    }
    Vector3[] GetPoints(System.IO.StreamReader file, int size)
    {
        Vector3[] array = new Vector3[size];
        for (int i = 0; i < size; i++)
        {
            double[] pnts = file.ReadLine().Split(' ').Select(Convert.ToDouble).ToArray();
            array[i] = new Vector3((float)pnts[0], (float)pnts[1], (float)pnts[2]);
            //Debug.Log(array[i].ToString());
        }

        return array;
    }

    List<int>[] GetCells(System.IO.StreamReader file, int size)
    {
        List<int>[] array = new List<int>[size];
        for (int i = 0; i < size; i++)
        {
            List<int> pnts = file.ReadLine().Split(' ').Select(int.Parse).ToList();
            pnts.RemoveAt(0);
            // the first element of this list is the number of following elements
            // so it can be removed to save space and time
            array[i] = pnts;
            /*string l = "";
            foreach (var x in array[i])
            {
                l += x.ToString() + " ";
            }
            Debug.Log(l);*/
        }

        return array;
    }
    int[] GetCellTypes(System.IO.StreamReader file, int size)
    {
        int[] array = new int[size];
        for (int i = 0; i < size; i++)
        {
            array[i] = int.Parse(file.ReadLine());
            //Debug.Log(array[i].ToString());
        }
        return array;
    }

    int[] GetCellTtriangles(System.IO.StreamReader file, int size, List<int>[] cells)
    {
        
        for (int i = 0; i < size; i++)
        {
            vtkCellToTris.GetTrianglesFromData(1, cells[i], int.Parse(file.ReadLine())).ToArray<int>();
        }
        return array;
    }

    Vector4[] GetColors(System.IO.StreamReader file)
    {
        Vector4[] array = new Vector4[vertixSize];
        
        for (int i = 0; i < vertixSize; i++)
        {
            string line = file.ReadLine();
            
            double[] vals = line.Trim().Split(' ').Select(Convert.ToDouble).ToArray();
            float avg = (float)vals.Average();
            if (avg > max)
                max = avg;
            if (avg < min)
                min = avg;
            array[i] = new Vector4((float)vals[0], (float)vals[1], (float)vals[2],avg ) ;
            //Debug.Log(array[i].w.ToString());
        }

        return array;
    }
}
public class vtkObj
{
    public Vector3[] points;
    public int[] tris;
    public Color[] colors;
    public vtkObj(Vector3[] points, int[] tris, Color[] colors)
    {
        this.points = points;
        this.tris = tris;
        this.colors = colors;
    }
    

}