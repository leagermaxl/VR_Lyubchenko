using System.Collections.Generic;
using UnityEngine;

public static class MeshExtensions
{
    private class Vertices
    {
        private List<Vector3> verts;
        private List<Vector2> uv1;
        private List<Vector2> uv2;
        private List<Vector2> uv3;
        private List<Vector2> uv4;
        private List<Vector3> normals;
        private List<Vector4> tangents;
        private List<Color32> colors;
        private List<BoneWeight> boneWeights;

        public Vertices()
        {
            verts = new List<Vector3>();
        }

        public Vertices(Mesh aMesh)
        {
            verts = CreateList(aMesh.vertices);
            uv1 = CreateList(aMesh.uv);
            uv2 = CreateList(aMesh.uv2);
            uv3 = CreateList(aMesh.uv3);
            uv4 = CreateList(aMesh.uv4);
            normals = CreateList(aMesh.normals);
            tangents = CreateList(aMesh.tangents);
            colors = CreateList(aMesh.colors32);
            boneWeights = CreateList(aMesh.boneWeights);
        }

        private List<T> CreateList<T>(T[] aSource)
        {
            return aSource != null && aSource.Length > 0 ? new List<T>(aSource) : null;
        }

        private void Copy<T>(ref List<T> aDest, List<T> aSource, int aIndex)
        {
            if (aSource == null) return;

            if (aDest == null)
                aDest = new List<T>();

            aDest.Add(aSource[aIndex]);
        }

        public int Add(Vertices aOther, int aIndex)
        {
            int i = verts.Count;
            Copy(ref verts, aOther.verts, aIndex);
            Copy(ref uv1, aOther.uv1, aIndex);
            Copy(ref uv2, aOther.uv2, aIndex);
            Copy(ref uv3, aOther.uv3, aIndex);
            Copy(ref uv4, aOther.uv4, aIndex);
            Copy(ref normals, aOther.normals, aIndex);
            Copy(ref tangents, aOther.tangents, aIndex);
            Copy(ref colors, aOther.colors, aIndex);
            Copy(ref boneWeights, aOther.boneWeights, aIndex);
            return i;
        }

        public void AssignTo(Mesh aTarget)
        {
            if (verts.Count > 65535)
                aTarget.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;

            aTarget.SetVertices(verts);
            SetMeshDataIfNotNull(aTarget, 0, uv1);
            SetMeshDataIfNotNull(aTarget, 1, uv2);
            SetMeshDataIfNotNull(aTarget, 2, uv3);
            SetMeshDataIfNotNull(aTarget, 3, uv4);
            SetMeshDataIfNotNull(aTarget, 4, normals);
            SetMeshDataIfNotNull(aTarget, 5, tangents);
            SetMeshDataIfNotNull(aTarget, 6, colors);

            if (boneWeights != null)
                aTarget.boneWeights = boneWeights.ToArray();
        }

        private static void SetMeshDataIfNotNull<T>(Mesh aTarget, int channel, List<T> data)
        {
            if (data != null)
            {
                if (typeof(T) == typeof(Vector2))
                {
                    aTarget.SetUVs(channel, data as List<Vector2>);
                }
                else if (typeof(T) == typeof(Vector3))
                {
                    aTarget.SetUVs(channel, data as List<Vector3>);
                }
            }
        }
    }

    public static Mesh GetSubmesh(this Mesh aMesh, int aSubMeshIndex)
    {
        if (aSubMeshIndex < 0 || aSubMeshIndex >= aMesh.subMeshCount)
            return null;

        int[] indices = aMesh.GetTriangles(aSubMeshIndex);
        Vertices source = new Vertices(aMesh);
        Vertices dest = new Vertices();
        Dictionary<int, int> map = new Dictionary<int, int>();
        int[] newIndices = new int[indices.Length];

        for (int i = 0; i < indices.Length; i++)
        {
            int o = indices[i];
            int n;

            if (!map.TryGetValue(o, out n))
            {
                n = dest.Add(source, o);
                map.Add(o, n);
            }

            newIndices[i] = n;
        }

        Mesh m = new Mesh();
        dest.AssignTo(m);
        m.triangles = newIndices;
        return m;
    }
}