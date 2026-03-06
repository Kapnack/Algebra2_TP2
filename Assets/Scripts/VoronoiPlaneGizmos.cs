using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[ExecuteAlways]
public class VoronoiPlaneGizmos : MonoBehaviour
{
    public Voronoi3D voronoi;

    [Header("Display")]
    public bool drawAllPlanes = false;
    public int nodeIndex = 0;

    [Header("Plane Visuals")]
    public float planeSize = 2f;
    public float normalLength = 1f;

    public Color planeColor = new Color(0, 1, 1, 0.25f);
    public Color normalColor = Color.red;

    void OnDrawGizmos()
    {
        if (!voronoi)
            return;

        List<VoronoiPlane> planes = voronoi.DebugGetPlanes();
        Dictionary<int, List<int>> nodePlanes = voronoi.DebugGetNodePlanes();

        if (planes == null)
            return;

        if (drawAllPlanes)
        {
            for (int i = 0; i < planes.Count; i++)
                DrawPlane(planes[i].plane);
        }
        else
        {
            if (!nodePlanes.ContainsKey(nodeIndex))
                return;

            foreach (int planeIndex in nodePlanes[nodeIndex])
            {
                DrawPlane(planes[planeIndex].plane);
            }
        }
    }

    void DrawPlane(Plane3 plane)
    {
        Vec3 normal = plane.normal;

        Vec3 center = normal * plane.d;

        Vec3 axisA = Vec3.Cross(normal, Vec3.Up);

        if (axisA.sqrMagnitude < 0.001f)
            axisA = Vec3.Cross(normal, Vec3.Right);

        axisA = axisA.normalized;
        Vec3 axisB = Vec3.Cross(normal, axisA);

        axisA *= planeSize;
        axisB *= planeSize;

        Vec3 p0 = center - axisA - axisB;
        Vec3 p1 = center + axisA - axisB;
        Vec3 p2 = center + axisA + axisB;
        Vec3 p3 = center - axisA + axisB;

        Gizmos.color = planeColor;

        Gizmos.DrawLine(p0, p1);
        Gizmos.DrawLine(p1, p2);
        Gizmos.DrawLine(p2, p3);
        Gizmos.DrawLine(p3, p0);

        Gizmos.color = normalColor;
        Gizmos.DrawLine(center, center + normal * normalLength);
    }
}