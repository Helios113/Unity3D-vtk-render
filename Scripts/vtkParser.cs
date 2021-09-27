using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using System.Linq;
using System;

public class vtkParser : MonoBehaviour
{
    static public vtkObj Parse(string fileData, string[] vectors = null, string[] scalars = null)
    {
        string[] lines = fileData.Split('\n');   

        if(!lines[2].Trim().Equals("ASCII"))
        {
            Debug.LogError("Non ASCII file used.\nThis is not currently supported.\nPlease raise an issue on github.");
            throw new NotSupportedException("Non ASCII file used.\nThis is not currently supported.\nPlease raise an issue on github.");
        }
        String type = lines[3].Trim().ToUpper();
        switch (type)
        {
            case "DATASET UNSTRUCTURED_GRID":
                return UnstructuredGrid_analyser(lines, vectors, scalars);
            case "DATASET POLYDATA":
                return PolyData_analyser(lines,vectors,scalars);
            default:
                Debug.LogError("Non supported file used " + type+ ".\nPlease raise an issue on github.");
                throw new NotSupportedException("Non supported file used " + type + ".\nPlease raise an issue on github.");
        }

        

    }

    // Type methods
    static vtkObj UnstructuredGrid_analyser(string[] lines, string[] vectors = null, string[] scalars = null)
    {
        Vector3[] points = null; // store vertex information
        List<int>[] cells = null; // store cell information
        int len = lines.Length;
        int[] tris = null; // store triangles
        Vector3[][] vectorsData = null; // place to store all required vector data
        float[][] scalarData = null; // place to store all required vector data
        int vertexSize = 0;
        int cellSize = 0;
        bool is2D = false;

        int numberOfVectors = 0;
        if (vectors != null)
        {
            Debug.Log("Looking for " + vectors.Length + " vectors");
            numberOfVectors = vectors.Length;
            vectorsData = new Vector3[numberOfVectors][];
        }

        int numberOfScalars = 0;
        if (scalars != null)
        {
            Debug.Log("Looking for " + scalars.Length + " scalars");
            numberOfScalars = scalars.Length;
            scalarData = new float[numberOfScalars][];
        }

        for (int i = 4; i < len; i++)
        {
            if (lines[i].Contains("POINTS"))
            {
                vertexSize = Convert.ToInt32(lines[i].Split(' ')[1]);
                Debug.Log("Points size: " + vertexSize);
                ++i;
                (points, is2D) = GetPoints(lines, i, vertexSize);
            }
            if (lines[i].Contains("CELLS"))
            {
                cellSize = Convert.ToInt32(lines[i].Split(' ')[1]);
                Debug.Log("Cell size: " + cellSize);
                ++i;
                cells = GetCells(lines, i, cellSize);
            }
            if (lines[i].Contains("CELL_TYPES"))
            {
                cellSize = Convert.ToInt32(lines[i].Split(' ')[1]);
                ++i;
                tris = GetUnstructuredGridCellTtriangles(lines, i, cellSize, cells);
            }
            if (numberOfVectors != 0 && lines[i].Contains("VECTORS"))
            {
                if (vectors.Any(lines[i].Split(' ')[1].Equals))
                {
                    vectorsData[Array.IndexOf(vectors, lines[i].Split(' ')[1])] = GetVector(lines, i, vertexSize);
                }
            }
            if (numberOfScalars != 0 && lines[i].Contains("SCALARS"))
            {
                if (scalars.Any(lines[i].Split(' ')[1].Equals))
                {
                    //Debug.Log(scalars);
                    //Debug.Log(lines[i]);
                    scalarData[Array.IndexOf(scalars, lines[i].Split(' ')[1])] = GetScalar(lines, i + 2, vertexSize);
                }
            }
        }
        //Debug.LogError(vectorsData.Length);
        return new vtkObj(points, tris, vectorsData, scalarData, is2D);
    }


    static vtkObj PolyData_analyser(string[] lines, string[] vectors = null, string[] scalars = null)
    {
        Vector3[] points = null; // store point information
        int[] tris = null;
        int[] verts = null;
        int len = lines.Length;
        int pointSize = 0;
        int vertexSize = 0;
        int polygonSize = 0;
        bool is2D = false;
        
        for (int i = 4; i < len; i++)
        {
            if (lines[i].Contains("POINTS"))
            {
                pointSize = Convert.ToInt32(lines[i].Split(' ')[1]);
                Debug.Log("Number of points: " + pointSize);
                ++i;
                (points, is2D) = GetPoints(lines, i, pointSize);
            }
            if (lines[i].Contains("VERTEX"))
            {
                i += pointSize;
                continue;
                vertexSize = Convert.ToInt32(lines[i].Split(' ')[1]);
                Debug.Log("Number of verteces: " + vertexSize);
                ++i;
                (points, is2D) = GetPoints(lines, i, pointSize);
            }
            if (lines[i].Contains("POLYGONS"))
            {
                polygonSize = Convert.ToInt32(lines[i].Split(' ')[1]);
                Debug.Log("Number of polygons: " + polygonSize);
                ++i;
                tris = GetPolyDataPolyTtriangles(lines, i, polygonSize);
            }
        }
        return new vtkObj(points, tris, null, null, is2D);
    }

    // Utility methods

    static (Vector3[], bool) GetPoints(string[] lines, int cnt, int size)
    {
        double[] test = new double[size];
        Vector3[] array = new Vector3[size];
        for (int i = cnt; i < cnt + size; i++)
        {
            try
            {
                double[] pnts = lines[i].Split(' ').Select(Convert.ToDouble).ToArray();
                //Debug.LogWarning("Points: " + pnts[0]+ " " + pnts[1]+ " "+ pnts[2]);
                array[i - cnt] = new Vector3((float)pnts[0], (float)pnts[2], (float)pnts[1]);
                test[i - cnt] = pnts[2];
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
                Debug.LogError("GetPoints Error");
                Debug.LogError("Current index: " + i);
                Debug.LogError("Start index: " + cnt);
                Debug.LogError("Desired legth: " + (size + cnt));
                Debug.LogError("Line that failed: " + lines[i]);
            }

        }
        return (array, (test.Distinct().Count() == 1));
    }

    /** Method for gathering all
     *  cells in an arraylist
     */
    static List<int>[] GetCells(string[] lines, int cnt, int size)
    {
        List<int>[] array = new List<int>[size];
        for (int i = cnt; i < cnt + size; i++)
        {
            List<int> pnts = lines[i].Split(' ').Select(int.Parse).ToList();
            pnts.RemoveAt(0);
            // the first element of this list is the number of following elements
            // so it can be removed to save space and time
            array[i - cnt] = pnts;
            /*string l = "";
            foreach (var x in array[i])
            {
                l += x.ToString() + " ";
            }
            Debug.Log(l);*/
        }

        return array;
    }

    /**DEPRICATED
     * Method for gathering all
     * cell types in an array
     */
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

    /** Method for converting vtk geometric objects to
     *  triangles from the source file
     */
    static int[] GetUnstructuredGridCellTtriangles(string[] lines, int cnt, int size, List<int>[] cells)
    {
        List<int> res = new List<int>();
        for (int i = cnt; i < cnt + size; i++)
        {
            res.AddRange(vtkCellToTris.GetTrianglesFromData(cells[i - cnt], int.Parse(lines[i])));
        }
        return res.ToArray<int>();
    }

    /** DERPICATED method for converting vtk geometric objects to
     *  triangles from loaded data
     */
    static int[] GetCellTtriangles(int size, List<int>[] cells, int[] cellTypes)
    {
        return vtkCellToTris.GetTrianglesFromData(size, cells, cellTypes).ToArray<int>();
    }

    /** Method for loading vector data into memory
     */
    static Vector3[] GetVector(string[] lines, int cnt, int vertexSize)
    {
        Vector3[] array = new Vector3[vertexSize];
        double[] vals;
        for (int i = cnt; i < cnt + vertexSize; i++)
        {
            vals = lines[i].Trim().Split(' ').Select(Convert.ToDouble).ToArray();
            array[i] = new Vector3((float)vals[0], (float)vals[2], (float)vals[1]);
        }

        return array;
    }

    static float[] GetScalar(string[] lines, int cnt, int vertexSize)
    {
        float[] array = new float[vertexSize];
        for (int i = cnt; i < cnt + vertexSize; i++)
        {
            print(lines[i]);
            array[i - cnt] = float.Parse(lines[i].Trim());
        }
        return array;
    }

    static int[] GetPolyDataPolyTtriangles(string[] lines, int cnt, int size)
    {
        List<int> res = new List<int>();
        for (int i = cnt; i < cnt + size; i++)
        {
            List<int> cell = new List<int>();
            string[] a = lines[i].Trim().Split(' ');
            cell.Add(int.Parse(a[1]));
            cell.Add(int.Parse(a[2]));
            cell.Add(int.Parse(a[3]));
            res.AddRange(vtkCellToTris.GetTrianglesFromData(cell, 7));
        }
        return res.ToArray<int>();
    }


}
public class vtkObj
{
    public Vector3[] points;
    public int[] tris;
    public Vector3[][] vecs;
    public float[][] scals;
    public bool is2D = false;
    public vtkObj(Vector3[] points, int[] tris, Vector3[][] vecss, float[][] scals, bool is2D)
    {
        this.points = points;
        this.tris = tris;
        this.vecs = vecss;
        this.scals = scals;
        this.is2D = is2D;
    }


}