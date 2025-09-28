using System.Collections.Generic;
using UnityEngine;

public class PointsGenerator : MonoBehaviour
{
    [SerializeField] private int _pointsAmount = 10;
    [SerializeField] private Vector3 _min = new(-10, 0, -10);
    [SerializeField] private Vector3 _max = new(10, 0, 10);
    [SerializeField] private GameObject _pointPrefab;
    [SerializeField] private Voronoi3D _voronoi;

    private List<Transform> _generatedPoints = new();

    [ContextMenu("Generate Points")]
    public void GeneratePoints()
    {
        ClearGeneratedPoints();

        List<Vector3> points = new();
        for (int i = 0; i < _pointsAmount; i++)
        {
            bool pointIsValid = false;
            Vector3 pos;

            if (i == 0)
            {
                pos = _min;
            }
            else if (i == _pointsAmount - 1)
            {
                pos = _max;
            }
            else
            {
                do
                {
                    pos = new Vector3(
                        Random.Range(_min.x, _max.x),
                        Random.Range(_min.y, _max.y),
                        Random.Range(_min.z, _max.z)
                    );

                    if (!points.Contains(pos))
                        pointIsValid = true;
                } while (!pointIsValid);
            }

            points.Add(pos);

            GameObject go;
            if (_pointPrefab)
                go = Instantiate(_pointPrefab, pos, Quaternion.identity);
            else
                go = new GameObject($"VoronoiPoint_{i}", typeof(Transform));
            
            go.transform.position = pos;
            _generatedPoints.Add(go.transform);
        }

        if (_voronoi)
        {
            _voronoi.nodeTransforms = _generatedPoints.ToArray();
            _voronoi.BuildAllNodePlanes();
        }
    }

    [ContextMenu("Clear Points")]
    private void ClearGeneratedPoints()
    {
        foreach (var t in _generatedPoints)
        {
            if (t)
            {
#if UNITY_EDITOR
                if (!Application.isPlaying)
                    DestroyImmediate(t.gameObject);
                else
                    Destroy(t.gameObject);
#else
                Destroy(t.gameObject);
#endif
            }
        }

        _generatedPoints.Clear();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector3 center = (_max + _min) * 0.5f;
        Vector3 size = _max - _min;
        Gizmos.DrawWireCube(center, size);
    }
}