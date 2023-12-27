using System.Collections.Generic;
using UnityEngine;

public static class MeshUtils
{
    public static List<Vector3> FindAdjacentNeighbors(Vector3[] vertices, int[] triangles, Vector3 vertex)
    {
        List<Vector3> adjacentVertices = new List<Vector3>();
        List<int> faceMarker = new List<int>();
        int faceCount = 0;

        for (int i = 0; i < vertices.Length; i++)
        {
            if (Mathf.Approximately(vertex.x, vertices[i].x) &&
                Mathf.Approximately(vertex.y, vertices[i].y) &&
                Mathf.Approximately(vertex.z, vertices[i].z))
            {
                int v1 = 0;
                int v2 = 0;
                bool marker = false;

                for (int k = 0; k < triangles.Length; k += 3)
                {
                    if (!faceMarker.Contains(k))
                    {
                        v1 = 0;
                        v2 = 0;
                        marker = false;

                        if (i == triangles[k])
                        {
                            v1 = triangles[k + 1];
                            v2 = triangles[k + 2];
                            marker = true;
                        }
                        else if (i == triangles[k + 1])
                        {
                            v1 = triangles[k];
                            v2 = triangles[k + 2];
                            marker = true;
                        }
                        else if (i == triangles[k + 2])
                        {
                            v1 = triangles[k];
                            v2 = triangles[k + 1];
                            marker = true;
                        }

                        faceCount++;
                        if (marker)
                        {
                            faceMarker.Add(k);

                            if (!IsVertexExist(adjacentVertices, vertices[v1]))
                            {
                                adjacentVertices.Add(vertices[v1]);
                            }

                            if (!IsVertexExist(adjacentVertices, vertices[v2]))
                            {
                                adjacentVertices.Add(vertices[v2]);
                            }

                            marker = false;
                        }
                    }
                }
            }
        }

        return adjacentVertices;
    }

    public static List<int> FindAdjacentNeighborIndexes(Vector3[] vertices, int[] triangles, Vector3 vertex)
    {
        List<int> adjacentIndexes = new List<int>();
        List<Vector3> adjacentVertices = new List<Vector3>();
        List<int> faceMarker = new List<int>();
        int faceCount = 0;

        for (int i = 0; i < vertices.Length; i++)
        {
            if (Mathf.Approximately(vertex.x, vertices[i].x) &&
                Mathf.Approximately(vertex.y, vertices[i].y) &&
                Mathf.Approximately(vertex.z, vertices[i].z))
            {
                int v1 = 0;
                int v2 = 0;
                bool marker = false;

                for (int k = 0; k < triangles.Length; k += 3)
                {
                    if (!faceMarker.Contains(k))
                    {
                        v1 = 0;
                        v2 = 0;
                        marker = false;

                        if (i == triangles[k])
                        {
                            v1 = triangles[k + 1];
                            v2 = triangles[k + 2];
                            marker = true;
                        }
                        else if (i == triangles[k + 1])
                        {
                            v1 = triangles[k];
                            v2 = triangles[k + 2];
                            marker = true;
                        }
                        else if (i == triangles[k + 2])
                        {
                            v1 = triangles[k];
                            v2 = triangles[k + 1];
                            marker = true;
                        }

                        faceCount++;
                        if (marker)
                        {
                            faceMarker.Add(k);

                            if (!IsVertexExist(adjacentVertices, vertices[v1]))
                            {
                                adjacentVertices.Add(vertices[v1]);
                                adjacentIndexes.Add(v1);
                            }

                            if (!IsVertexExist(adjacentVertices, vertices[v2]))
                            {
                                adjacentVertices.Add(vertices[v2]);
                                adjacentIndexes.Add(v2);
                            }

                            marker = false;
                        }
                    }
                }
            }
        }

        return adjacentIndexes;
    }

    private static bool IsVertexExist(List<Vector3> adjacentVertices, Vector3 v)
    {
        foreach (Vector3 vec in adjacentVertices)
        {
            if (Mathf.Approximately(vec.x, v.x) && Mathf.Approximately(vec.y, v.y) && Mathf.Approximately(vec.z, v.z))
            {
                return true;
            }
        }

        return false;
    }
}

public class SmoothFilter
{
    public static Vector3[] LaplacianFilter(Vector3[] originalVertices, int[] triangles)
    {
        Vector3[] smoothedVertices = new Vector3[originalVertices.Length];

        for (int vi = 0; vi < originalVertices.Length; vi++)
        {
            List<Vector3> adjacentVertices = MeshUtils.FindAdjacentNeighbors(originalVertices, triangles, originalVertices[vi]);

            if (adjacentVertices.Count != 0)
            {
                float dx = 0.0f;
                float dy = 0.0f;
                float dz = 0.0f;

                for (int j = 0; j < adjacentVertices.Count; j++)
                {
                    dx += adjacentVertices[j].x;
                    dy += adjacentVertices[j].y;
                    dz += adjacentVertices[j].z;
                }

                smoothedVertices[vi] = new Vector3(dx / adjacentVertices.Count, dy / adjacentVertices.Count, dz / adjacentVertices.Count);
            }
        }

        return smoothedVertices;
    }

    public static Vector3[] HCFilter(Vector3[] originalVertices, Vector3[] previousVertices, int[] triangles, float alpha, float beta)
    {
        Vector3[] smoothedVertices = LaplacianFilter(originalVertices, triangles);
        Vector3[] differences = new Vector3[originalVertices.Length];

        for (int i = 0; i < originalVertices.Length; i++)
        {
            differences[i] = new Vector3(
                smoothedVertices[i].x - (alpha * originalVertices[i].x + (1 - alpha) * originalVertices[i].x),
                smoothedVertices[i].y - (alpha * originalVertices[i].y + (1 - alpha) * originalVertices[i].y),
                smoothedVertices[i].z - (alpha * originalVertices[i].z + (1 - alpha) * originalVertices[i].z)
            );
        }

        for (int j = 0; j < differences.Length; j++)
        {
            List<int> adjacentIndexes = MeshUtils.FindAdjacentNeighborIndexes(originalVertices, triangles, originalVertices[j]);

            float dx = 0.0f;
            float dy = 0.0f;
            float dz = 0.0f;

            for (int k = 0; k < adjacentIndexes.Count; k++)
            {
                dx += differences[adjacentIndexes[k]].x;
                dy += differences[adjacentIndexes[k]].y;
                dz += differences[adjacentIndexes[k]].z;
            }

            smoothedVertices[j] = new Vector3(
                smoothedVertices[j].x - beta * differences[j].x + ((1 - beta) / adjacentIndexes.Count) * dx,
                smoothedVertices[j].y - beta * differences[j].y + ((1 - beta) / adjacentIndexes.Count) * dy,
                smoothedVertices[j].z - beta * differences[j].z + ((1 - beta) / adjacentIndexes.Count) * dz
            );
        }

        return smoothedVertices;
    }
}

public class MeshSmoothener
{
    public enum Filter { Laplacian = 1, HC = 2 };

    public static Mesh SmoothMesh(Mesh mesh, int power, Filter filterType)
    {
        for (int i = 0; i < power; ++i)
        {
            if (filterType == Filter.HC)
                mesh.vertices = SmoothFilter.HCFilter(mesh.vertices, mesh.vertices, mesh.triangles, 0.0f, 0.5f);
            if (filterType == Filter.Laplacian)
                mesh.vertices = SmoothFilter.LaplacianFilter(mesh.vertices, mesh.triangles);
        }
        return mesh;
    }
}