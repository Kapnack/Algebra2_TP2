using System;
using System.Collections.Generic;
using UnityEngine;

public class Voronoi3D : MonoBehaviour
{ 
    public Transform[] nodeTransforms;

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

    private List<Plane3> GetBisectorPlanesForNode(int index)
    {
        List<Plane3> planes = new();
        var nodes = GetNodePositions();
        
        if (index < 0 || index >= nodes.Length) 
            return planes;
        
        Vec3 a = nodes[index];
        
        for (int i = 0; i < nodes.Length; i++)
        {
            if (i == index) 
                continue;
            
            Plane3 bis = Plane3.BisectorPlane(a, nodes[i]);
            planes.Add(bis);
        }
        return planes;
    }

    public bool IsPointInsideCell(int index, Vec3 point)
    {
        var planes = GetBisectorPlanesForNode(index);
        foreach (var plane in planes)
        {
            if (plane.IsPositiveSide(point))
                return false;
        }
        
        return true;
    }

    
    public int GetNearestNodeIndex(Vec3 point, out float distance)
    {
        var nodes = GetNodePositions();
        if (nodes.Length == 0)
        {
            distance = float.PositiveInfinity;
            return -1;
        }

        int best = 0;
        float bestSqr = Vec3.SqrMagnitude(nodes[0] - point);

        for (int i = 1; i < nodes.Length; i++)
        {
            float sq = Vec3.SqrMagnitude(nodes[i] - point);
            if (sq < bestSqr)
            {
                bestSqr = sq;
                best = i;
            }
        }

        distance = Mathf.Sqrt(bestSqr);
        return best;
    }
}