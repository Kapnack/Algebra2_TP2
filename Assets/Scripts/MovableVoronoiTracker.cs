using UnityEngine;

public class MovableVoronoiTracker : MonoBehaviour
{
    public Voronoi3D voronoi;
    public Color lineColor = Color.green;

    private void Update()
    {
        if (!voronoi)
            return;

        Vec3 pos = Voronoi3D.ToVec3(transform.position);
        
        for (int i = 0; i < voronoi.nodeTransforms.Length; i++)
        {
            if (voronoi.IsPointInsideCell(i, pos))
            {
                Debug.Log($"[Voronoi] El objeto está dentro de la celda del nodo {i}");
                break;
            }
        }
    }


    private void OnDrawGizmos()
    {
        if (voronoi == null || voronoi.nodeTransforms == null)
            return;

        Vec3 p = Voronoi3D.ToVec3(transform.position);

        int idx = voronoi.GetNearestNodeIndex(p, out var distance);
        if (idx >= 0 && idx < voronoi.nodeTransforms.Length && voronoi.nodeTransforms[idx] != null)
        {
            Gizmos.color = lineColor;
            Gizmos.DrawLine(transform.position, voronoi.nodeTransforms[idx].position);
            Gizmos.DrawSphere(voronoi.nodeTransforms[idx].position, 0.06f);
#if UNITY_EDITOR
            UnityEditor.Handles.Label(p + Vector3.up, $"Distance: {distance}");
#endif
        }
    }
}