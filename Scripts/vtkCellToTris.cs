using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class vtkCellToTris : MonoBehaviour
{
    public static List<int> GetTrianglesFromData(int cellSize, List<int>[] cells, int[] types)
    {
        List<int> res = new List<int>();
        for (int i = 0; i < cellSize; i++)
        {
            res.AddRange(GetTrianglesFromData(cells[i], types[i]));
        }
        return res;
    }

    public static List<int> GetTrianglesFromData(List<int> cell, int type)
    {
        List<int> res = new List<int>();
        
        switch (type)
        {
            // Linear cells
            case 0:
                break;
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;
            case 4:
                break;
            case 5: //triangle
                res.AddRange(triangle(cell));
                break;
            case 6:
                res.AddRange(triangleStrip(cell));
                break;
            case 7:
                res.AddRange(polygon(cell));
                break;
            case 8:
                res.AddRange(pixel(cell));
                break;
            case 9:
                res.AddRange(quad(cell));
                break;
            case 10:
                res.AddRange(tetra(cell));
                break;
            case 11:
                res.AddRange(voxel(cell));
                break;
            case 12:
                res.AddRange(hexahedron(cell));
                break;
            case 13:
                //TODO vedge
                break;
            case 14:
                //TODO pyramid
                break;
            case 15:
                //TODO PENTAGONAL_PRISM
                break;
            case 16:
                //TODO HEXAGONAL_PRISM
                break;

            // Quadratic, isoparametric cells
            case 21:
                break;
            case 22:
                res.AddRange(quadraticTriangle(cell));
                break;
            case 23:
                res.AddRange(quadraticQuad(cell));
                break;
            case 36:
                //TODO
                break;
            case 24:
                //TODO
                break;
            case 25:
                //TODO
                break;
            case 26:
                //TODO
                break;
            case 27:
                //TODO
                break;
            case 28:
                //TODO
                break;
            case 29:
                //TODO
                break;
            case 37:
                //TODO
                break;
            case 30:
                //TODO
                break;
            case 31:
                //TODO
                break;
            case 32:
                //TODO
                break;
            case 33:
                //TODO
                break;
            case 34:
                //TODO
                break;

            // Cubic, isoparametric cell
            case 35:
                //TODO
                break;

            // Special class of cells formed by convex group of points
            case 41:
                //TODO
                break;

            // Polyhedron cell (consisting of polygonal faces)
            case 42:
                //TODO
                break;

            // Higher order cells in parametric form
            case 51:
                //TODO
                break;
            case 52:
                //TODO
                break;
            case 53:
                //TODO
                break;
            case 54:
                //TODO
                break;
            case 55:
                //TODO
                break;
            case 56:
                //TODO
                break;

            // Higher order cells
            case 60:
                //TODO
                break;
            case 61:
                //TODO
                break;
            case 62:
                //TODO
                break;
            case 63:
                //TODO
                break;
            case 64:
                //TODO
                break;
            case 65:
                //TODO
                break;
            case 66:
                //TODO
                break;
            case 67:
                //TODO
                break;

            // Arbitrary order Lagrange elements (formulated separated from generic higher order cells)
            case 68:
                //TODO
                break;
            case 69:
                //TODO
                break;
            case 70:
                //TODO
                break;
            case 71:
                //TODO
                break;
            case 72:
                //TODO
                break;
            case 73:
                //TODO
                break;
            case 74:
                //TODO
                break;

            // Arbitrary order Bezier elements (formulated separated from generic higher order cells)
            case 75:
                //TODO
                break;
            case 76:
                //TODO
                break;
            case 77:
                //TODO
                break;
            case 78:
                //TODO
                break;
            case 79:
                //TODO
                break;
            case 80:
                //TODO
                break;
            case 81:
                //TODO
                break;
            default:
                throw new ArgumentException("Unkown cell type in file");

            
        }

        return res;
    }

    static List<int> triangle(List<int> verts)
    {
        verts.Reverse();
        return verts;
    }
    static List<int> triangleStrip(List<int> verts)
    {
        List<int> work = new List<int>();
        for(int i =0; i<verts.Count-2;i++)
        {
            work.Add(verts[i]);
            if (i % 2 == 0)
            {
                work.Add(verts[i + 1]);
                work.Add(verts[i + 2]);
            }
            else
            {
                work.Add(verts[i + 2]);
                work.Add(verts[i + 1]);
            }
        }
        return work;
    }
    static List<int> polygon(List<int> verts)
    {
        List<int> work = new List<int>();
        for (int i = 1; i < verts.Count-1; i++)
        {
            work.Add(verts[0]);
            work.Add(verts[i + 1]);
            work.Add(verts[i]);
        }
        return work;
    }
    static List<int> pixel(List<int> verts)
    {
        List<int> work = new List<int>();
        work.Add(verts[0]);
        work.Add(verts[2]);
        work.Add(verts[1]);
        work.Add(verts[2]);
        work.Add(verts[3]);
        work.Add(verts[1]);
        return work;
    }
    static List<int> quad(List<int> verts)
    {
        List<int> work = new List<int>();
        work.Add(verts[0]);
        work.Add(verts[2]);
        work.Add(verts[1]);
        work.Add(verts[0]);
        work.Add(verts[3]);
        work.Add(verts[1]);
        return work;
    }
    static List<int> tetra(List<int> verts)
    {
        List<int> work = new List<int>();
        work.Add(verts[0]);
        work.Add(verts[2]);
        work.Add(verts[3]);

        work.Add(verts[0]);
        work.Add(verts[3]);
        work.Add(verts[1]);

        work.Add(verts[1]);
        work.Add(verts[2]);
        work.Add(verts[0]);

        work.Add(verts[1]);
        work.Add(verts[3]);
        work.Add(verts[2]);
        return work;
    }
    static List<int> voxel(List<int> verts)
    {
        List<int> work = new List<int>();
        work.Add(verts[0]);
        work.Add(verts[4]);
        work.Add(verts[5]);

        work.Add(verts[0]);
        work.Add(verts[5]);
        work.Add(verts[1]);

        work.Add(verts[1]);
        work.Add(verts[5]);
        work.Add(verts[3]);

        work.Add(verts[5]);
        work.Add(verts[7]);
        work.Add(verts[3]);

        work.Add(verts[3]);
        work.Add(verts[7]);
        work.Add(verts[2]);

        work.Add(verts[7]);
        work.Add(verts[6]);
        work.Add(verts[2]);

        work.Add(verts[6]);
        work.Add(verts[4]);
        work.Add(verts[2]);

        work.Add(verts[4]);
        work.Add(verts[0]);
        work.Add(verts[2]);

        work.Add(verts[4]);
        work.Add(verts[6]);
        work.Add(verts[5]);

        work.Add(verts[6]);
        work.Add(verts[7]);
        work.Add(verts[5]);

        work.Add(verts[2]);
        work.Add(verts[0]);
        work.Add(verts[1]);

        work.Add(verts[2]);
        work.Add(verts[1]);
        work.Add(verts[3]);
        return work;
    }
    static List<int> hexahedron(List<int> verts)
    {
        List<int> work = new List<int>();
        work.Add(verts[0]);
        work.Add(verts[4]);
        work.Add(verts[5]);

        work.Add(verts[0]);
        work.Add(verts[5]);
        work.Add(verts[1]);

        work.Add(verts[1]);
        work.Add(verts[5]);
        work.Add(verts[2]);

        work.Add(verts[2]);
        work.Add(verts[5]);
        work.Add(verts[6]);

        work.Add(verts[2]);
        work.Add(verts[6]);
        work.Add(verts[3]);

        work.Add(verts[6]);
        work.Add(verts[7]);
        work.Add(verts[3]);

        work.Add(verts[3]);
        work.Add(verts[7]);
        work.Add(verts[4]);

        work.Add(verts[3]);
        work.Add(verts[4]);
        work.Add(verts[0]);

        work.Add(verts[4]);
        work.Add(verts[7]);
        work.Add(verts[5]);

        work.Add(verts[7]);
        work.Add(verts[6]);
        work.Add(verts[5]);

        work.Add(verts[3]);
        work.Add(verts[0]);
        work.Add(verts[1]);

        work.Add(verts[3]);
        work.Add(verts[1]);
        work.Add(verts[2]);
        return work;
    }
    static List<int> quadraticTriangle(List<int> verts)
    {
        List<int> work = new List<int>();
        work.Add(verts[0]);
        work.Add(verts[5]);
        work.Add(verts[3]);

        work.Add(verts[5]);
        work.Add(verts[2]);
        work.Add(verts[4]);

        work.Add(verts[3]);
        work.Add(verts[5]);
        work.Add(verts[4]);

        work.Add(verts[3]);
        work.Add(verts[4]);
        work.Add(verts[1]);

        return work;
    }
    static List<int> quadraticQuad(List<int> verts)
    {
        List<int> work = new List<int>();
        work.Add(verts[7]);
        work.Add(verts[4]);
        work.Add(verts[0]);

        work.Add(verts[7]);
        work.Add(verts[6]);
        work.Add(verts[4]);

        work.Add(verts[7]);
        work.Add(verts[3]);
        work.Add(verts[6]);

        work.Add(verts[6]);
        work.Add(verts[5]);
        work.Add(verts[4]);

        work.Add(verts[6]);
        work.Add(verts[2]);
        work.Add(verts[5]);

        work.Add(verts[5]);
        work.Add(verts[1]);
        work.Add(verts[4]);

        return work;
    }

}
