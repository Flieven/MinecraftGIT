using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshBuilder : MonoBehaviour
{
    private static Vector3[] vertecies =
    {
        new Vector3(1, 1, 1),
        new Vector3(-1, 1, 1),
        new Vector3(-1, -1, 1),

        new Vector3(1, -1, 1),
        new Vector3(-1, 1, -1),
        new Vector3(1, 1, -1),

        new Vector3(1, -1, -1),
        new Vector3(-1, -1, -1),
    };

    private static int[][] faceTriangles =
    {
        new int[] { 0, 1, 2, 3 },
        new int[] { 5, 0, 3, 6 },
        new int[] { 4, 5, 6, 7 },
        new int[] { 1, 4, 7, 2 },
        new int[] { 5, 4, 1, 0 },
        new int[] { 3, 2, 7, 6 }
    };

    public static Vector3[] faceVertices(int dir, float faceScale, Vector3 facePos)
    {
        Vector3[] fv = new Vector3[4];
        for (int i = 0; i < fv.Length; i++)
        {
            fv[i] = (vertecies[faceTriangles[dir][i]] * faceScale) + facePos;
        }
        return fv;
    }

    public static Vector3[] faceVertices(direction dir, float faceScale, Vector3 facePos)
    {
        return faceVertices((int)dir, faceScale, facePos);
    }

}
