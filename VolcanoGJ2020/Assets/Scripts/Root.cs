using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Root : MonoBehaviour
{
    public LineRenderer line;
    private float step = 0.5f;
    private float amplitude = 3;

    public Tree connectedTree { get; set; }

    public void TraceRoot(Vector3 start, Vector3 end)
    {
        var points = PerlinNoise(start, end);

        line.positionCount = points.Length + 1;
        for (int i = 0; i < points.Length; i++)
        {
            line.SetPosition(i, points[i]);
        }
        line.SetPosition(line.positionCount - 1, end);
    }

    public void ProlongateRoot(Vector3 newPos)
    {
        int k = line.positionCount;
        var points = PerlinNoise(line.GetPosition(k - 1), newPos);

        line.positionCount = k + points.Length + 1;
        for (int i = 0; i < points.Length; i++)
        {
            line.SetPosition(i + k, points[i]);
        }
        line.SetPosition(line.positionCount - 1, newPos);

        //int k = line.positionCount;
        //line.positionCount = k + 1;
        //line.SetPosition(k, newPos);
    }

    public Vector3[] PerlinNoise(Vector3 start, Vector3 end)
    {
        Vector3 direction = end - start;
        int pointCount = Mathf.FloorToInt(direction.magnitude / step);
        Vector3[] points = new Vector3[pointCount];
        float x = Random.Range(0f, 1f);
        float y = 0;
        for (int i = 0; i < pointCount; i++)
        {
            Vector3 lateralDir = Vector3.Cross(direction.normalized, Vector3.up);
            y = i * step;
            float p = (Mathf.PerlinNoise(x, y) - 0.5f) * amplitude;
            Vector3 point = start + direction.normalized * step * i;
            point += lateralDir * p;
            points[i] = point;
        }
        return points;
    }
}
