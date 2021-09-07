using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using System.Linq;
using System;

public class vtkParser : MonoBehaviour
{
    static public vtkObj Parse(System.IO.StreamReader file, bool lessMem = true, string[] vectors = null)
    {
        int vertixSize = 0;
        int cellSize = 0;

        Vector3[] points = null; // store vertex information
        List<int>[] cells = null; // store cell information
        int[] cellTypes = null; // store cell types
        int[] tris = null; // store triangles
        Vector3[][] vectorsData = null; // place to store all required vector data

        string line = "";

        int nuberOfVectors = 0;
        if (vectors != null)
        {
            Debug.Log("We got to here " + vectors.Length);
            nuberOfVectors = vectors.Length;
            vectorsData = new Vector3[nuberOfVectors][];
        }
        while ((line = file.ReadLine()) != null)
        {
            
            if (line.Contains("POINTS"))
            {
                vertixSize = Convert.ToInt32(line.Split(' ')[1]);
                Debug.Log("Points size: "+ vertixSize);
                points = GetPoints(file, vertixSize);
            }
            if (line.Contains("CELLS"))
            {
                cellSize = Convert.ToInt32(line.Split(' ')[1]);
                Debug.Log("Cell size: " + cellSize);
                cells = GetCells(file, cellSize);
            }
            if (line.Contains("CELL_TYPES"))
            {
                cellSize = Convert.ToInt32(line.Split(' ')[1]);
                Debug.Log("Cell type size: " + cellSize);
                if (lessMem)
                    tris = GetCellTtriangles(file, cellSize, cells);
                else
                    cellTypes = GetCellTypes(file, cellSize);
            }
            if (nuberOfVectors!=0 && line.Contains("VECTORS"))
            {
                Debug.Log("And to here");
                vectorsData[Array.IndexOf(vectors, line.Split(' ')[1])] = GetVector(file, vertixSize);
            }
            if (nuberOfVectors != 0 && line.Contains("SCALARS"))
            {
                Debug.Log("And to here");
                //vectorsData[Array.IndexOf(vectors, line.Split(' ')[1])] = GetVector(file, vertixSize);
            }
        }
        if (!lessMem && cells.Length != 0 && cellTypes.Length != 0)
        {
            tris = GetCellTtriangles(cellSize, cells, cellTypes);
        }
        return new vtkObj(points, tris, vectorsData);

    }
    static Vector3[] GetPoints(System.IO.StreamReader file, int size)
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

    static List<int>[] GetCells(System.IO.StreamReader file, int size)
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
    static int[] GetCellTypes(System.IO.StreamReader file, int size)
    {
        int[] array = new int[size];
        for (int i = 0; i < size; i++)
        {
            array[i] = int.Parse(file.ReadLine());
            //Debug.Log(array[i].ToString());
        }
        return array;
    }

    static int[] GetCellTtriangles(System.IO.StreamReader file, int size, List<int>[] cells)
    {
        List<int> res = new List<int>();
        for (int i = 0; i < size; i++)
        {
            res.AddRange(vtkCellToTris.GetTrianglesFromData(cells[i], int.Parse(file.ReadLine())));
        }
        return res.ToArray<int>();
    }
    static int[] GetCellTtriangles(int size, List<int>[] cells, int[] cellTypes)
    {
        return vtkCellToTris.GetTrianglesFromData(size, cells, cellTypes).ToArray<int>();
    }

    static Vector3[] GetVector(System.IO.StreamReader file, int vertixSize)
    {
        Vector3[] array = new Vector3[vertixSize];
        string line;
        double[] vals;
        for (int i = 0; i < vertixSize; i++)
        {
            line = file.ReadLine();
            vals = line.Trim().Split(' ').Select(Convert.ToDouble).ToArray();
            array[i] = new Vector3((float)vals[0], (float)vals[1], (float)vals[2]);
        }
        return array;
    }
}
public class vtkObj
{
    public Vector3[] points;
    public int[] tris;
    public Vector3[][] colors;
    public vtkObj(Vector3[] points, int[] tris, Vector3[][] colors)
    {
        this.points = points;
        this.tris = tris;
        this.colors = colors;
    }
    

}