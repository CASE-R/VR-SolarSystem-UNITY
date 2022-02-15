using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Ellipse //Using: https://www.youtube.com/watch?v=Or3fA-UjnwU&list=PL5KbKbJ6Gf982bozKUYrX9C4qN_IQYTXZ&index=5
{
    public float xAxis;
    public float yAxis;

    public Ellipse (float xAxis, float yAxis)
    {
        this.xAxis = xAxis;
        this.yAxis = yAxis;
    }

    public Vector2 Evaluate (float t)
    {
        float angle = Mathf.Deg2Rad * 360f * t;
        float x = Mathf.Sin(angle) * xAxis;
        float y = Mathf.Cos(angle) * yAxis;
        return new Vector2(x, y);
    }

}
