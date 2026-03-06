using System;
using System.Collections.Generic;
using UnityEngine;

public class Voronoi3D : MonoBehaviour
{
    public Transform[] nodeTransforms;

    public Vec3[] _nodes;

    public List<VoronoiPlane> _planes = new();
    private Dictionary<int, List<int>> _nodePlanes = new();

    public List<VoronoiPlane> DebugGetPlanes()
    {
        return _planes;
    }

    public Dictionary<int, List<int>> DebugGetNodePlanes()
    {
        return _nodePlanes;
    }

    private Vec3[] GetNodePositions()
    {
        if (nodeTransforms == null)
            return Array.Empty<Vec3>();

        Vec3[] arr = new Vec3[nodeTransforms.Length];

        for (int i = 0; i < nodeTransforms.Length; i++)
        {
            if (nodeTransforms[i])
                arr[i] = Vec3.ToVec3(nodeTransforms[i].position);
        }

        return arr;
    }

    [ContextMenu("Build Voronoi")]
    public void BuildVoronoi()
    {
        _planes.Clear();
        _nodePlanes.Clear();

        _nodes = GetNodePositions();

        for (int i = 0; i < _nodes.Length; i++)
            _nodePlanes[i] = new List<int>();

        BuildGabrielPlanes();
    }

    private void BuildGabrielPlanes()
    {
        int n = _nodes.Length;

        for (int i = 0; i < n; i++)
        {
            for (int j = i + 1; j < n; j++)
            {
                if (!IsGabrielEdge(i, j))
                    continue;

                Plane3 plane = Plane3.BisectorPlane(_nodes[i], _nodes[j]);

                int id = _planes.Count;

                _planes.Add(new VoronoiPlane(i, j, plane));

                _nodePlanes[i].Add(id);
                _nodePlanes[j].Add(id);
            }
        }
    }

    private bool IsGabrielEdge(int i, int j)
    {
        Vec3 a = _nodes[i];
        Vec3 b = _nodes[j];

        Vec3 center = (a + b) * 0.5f;
        float radiusSq = Vec3.Distance(a, center);

        for (int k = 0; k < _nodes.Length; k++)
        {
            if (k == i || k == j)
                continue;

            float distSq = Vec3.Distance(_nodes[k], center);

            if (distSq < radiusSq || Mathf.Approximately(distSq, radiusSq))
                return false;
        }

        return true;
    }

    public bool IsPointInsideCell(int nodeIndex, Vec3 point)
    {
        if (!_nodePlanes.ContainsKey(nodeIndex))
            return false;

        foreach (int planeIndex in _nodePlanes[nodeIndex])
        {
            VoronoiPlane vp = _planes[planeIndex];

            float dist = vp.plane.SignedDistance(point);

            if (nodeIndex == vp.a)
            {
                if (dist > float.Epsilon || Mathf.Approximately(dist, float.Epsilon))
                    return false;
            }
            else
            {
                if (dist < float.Epsilon || Mathf.Approximately(dist, float.Epsilon))
                    return false;
            }
        }

        return true;
    }
}