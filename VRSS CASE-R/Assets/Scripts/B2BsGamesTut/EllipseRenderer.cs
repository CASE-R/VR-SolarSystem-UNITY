using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class EllipseRenderer : MonoBehaviour //Using: https://www.youtube.com/watch?v=mQKGRoV_jBc&list=PL5KbKbJ6Gf982bozKUYrX9C4qN_IQYTXZ&index=4
{
    LineRenderer lr;

    [Range(3, 36)]
    public int segments;
    public Ellipse ellipse;
    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
        CalculateEllipse();
    }

    void CalculateEllipse()
    {
        Vector3[] points = new Vector3[segments + 1];
        for (int i = 0; i < segments; i++)
        {
            /*
            float angle = ((float)i / (float)segments) * 360 * Mathf.Deg2Rad;
            float x = Mathf.Sin(angle) * xAxis;
            float y = Mathf.Cos(angle) * yAxis;
            */
            Vector2 position2D = ellipse.Evaluate((float)i / (float)segments);
            points[i] = new Vector3(position2D.x, position2D.y, 0f);
        }
        points[segments] = points[0];

        lr.positionCount = segments + 1;
        lr.SetPositions(points);
    }

    private void OnValidate()
    {
        CalculateEllipse();
    }
    
}
