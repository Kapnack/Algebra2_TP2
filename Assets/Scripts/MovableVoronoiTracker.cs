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

        Vec3 pos = Voronoi3D.ToVec3(transform.position);

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

        Vec3 p = Voronoi3D.ToVec3(transform.position);

        if (nearest < 0 || nearest >= voronoi.nodeTransforms.Length)
            return;

        Vec3 nodePos = Voronoi3D.ToVec3(voronoi.nodeTransforms[nearest].position);

        float distSqr = (nodePos - p).sqrMagnitude;

        Gizmos.color = lineColor;
        Gizmos.DrawLine(p, voronoi.nodeTransforms[nearest].position);
        Gizmos.DrawSphere(voronoi.nodeTransforms[nearest].position, 0.06f);

#if UNITY_EDITOR
        UnityEditor.Handles.Label(p + Vec3.Up, $"Index: {nearest}. Distance: {MathF.Sqrt(distSqr)}");
#endif
    }
}