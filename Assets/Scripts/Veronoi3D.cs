using System;
using System.Collections.Generic;
using UnityEngine;

public class Voronoi3D : MonoBehaviour
{
    public Transform[] nodeTransforms;
    
    private Dictionary<int, List<Plane3>> _nodePlanes = new();

    public static Vec3 ToVec3(Vector3 v) => new(v.x, v.y, v.z);

    private Vec3[] GetNodePositions()
    {
        if (nodeTransforms == null)
            return Array.Empty<Vec3>();

        Vec3[] arr = new Vec3[nodeTransforms.Length];

        for (int i = 0; i < nodeTransforms.Length; i++)
            if (nodeTransforms[i])
                arr[i] = ToVec3(nodeTransforms[i].position);

        return arr;
    }
    
    [ContextMenu("Create Planes")]
    public void BuildAllNodePlanes()
    {
        _nodePlanes.Clear();
        var nodes = GetNodePositions();

        for (int i = 0; i < nodes.Length; i++)
        {
            List<Plane3> planes = new();
            for (int j = 0; j < nodes.Length; j++)
            {
                if (i == j)
                    continue;

                Plane3 bis = Plane3.BisectorPlane(nodes[i], nodes[j]);
                planes.Add(bis);
            }
            _nodePlanes[i] = planes;
        }
    }
    
    public bool IsPointInsideCell(int index, Vec3 point)
    {
        if (!_nodePlanes.ContainsKey(index))
            return false;

        foreach (var plane in _nodePlanes[index])
        {
            if (plane.IsPositiveSide(point))
                return false;
        }

        return true;
    }

    private void Start()
    {
        BuildAllNodePlanes();
    }
}