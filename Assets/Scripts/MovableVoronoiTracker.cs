using System;
using UnityEngine;

public class MovableVoronoiTracker : MonoBehaviour
{
    public Voronoi3D voronoi;
    public Color lineColor = Color.green;
    [SerializeField] private bool voronoiCalculation;

    private int nearest = -1;

    private void Update()
    {
        if (!voronoi)
            return;

        Vec3 pos = Vec3.ToVec3(transform.position);

        for (int i = 0; i < voronoi.nodeTransforms.Length; i++)
        {
            if (voronoi.IsPointInsideCell(i, pos))
            {
                Debug.Log($"[{GetType()}] El objeto está dentro de la celda del nodo {i}");

                nearest = i;

                break;
            }
        }
    }


    private void OnDrawGizmos()
    {
        if (voronoi == null || voronoi.nodeTransforms == null)
            return;

        Vec3 p = Vec3.ToVec3(transform.position);

        Vec3 nearstVec3 = voronoi._nodes[0];

        foreach (Vec3 voronoi in voronoi._nodes)
        {
            if (Vec3.Distance(nearstVec3, p) > Vec3.Distance(voronoi, p))
                nearstVec3 = voronoi;

            Gizmos.color = Color.red;
            Gizmos.DrawLine(p, voronoi);
        }

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(p, nearstVec3);

        if (nearest < 0 || nearest >= voronoi.nodeTransforms.Length)
            return;

        Vec3 nodePos = Vec3.ToVec3(voronoi.nodeTransforms[nearest].position);

        Gizmos.color = lineColor;
        Gizmos.DrawLine(p, voronoi.nodeTransforms[nearest].position);
        Gizmos.DrawSphere(voronoi.nodeTransforms[nearest].position, 0.06f);

#if UNITY_EDITOR
        UnityEditor.Handles.Label(p + Vec3.Up, $"Index: {nearest}. Distance: {MathF.Sqrt((nodePos - p).magnitude)}");
#endif
    }
}