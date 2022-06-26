using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public class Parabola
{
    public class Solution
    {
        public SolutionPoint from;
        public SolutionPoint to;
        public DrawHandler.Line line;

        public Solution(SolutionPoint from, SolutionPoint to)
        {

            if (from.x > to.x)
            {
                this.to = from;
                this.from = to;
            }
            else
            {
                this.from = from;
                this.to = to;
            }
        }

        public bool isPointBetweenSolution(SolutionPoint point)
        {
            return point.x < to.x && point.x > from.x;
        }

        public class SolutionPoint
        {
            public float x; // x postion
            public Parabola targetParabola;

            public SolutionPoint(float x, Parabola targetParabola)
            {
                this.x = x;
                this.targetParabola = targetParabola;
            }
        }
    }

    private float standardLineY; // k - c
    private Vector2 focus; // (h, k + c)
    public Vector2 vertex; // (h, k)
    public float c;

    public Parabola(Vector2 focus)
    {
        this.focus = focus;
        this.standardLineY = 0.0f;
        CalculateVertexAndC();
    }


    public Parabola(float standardLineY, Vector2 focus)
    {
        this.standardLineY = standardLineY;
        this.focus = focus;
        CalculateVertexAndC();
    }

    public void SetStandardLineY(float standardLineY)
    {
        this.standardLineY = standardLineY;
        CalculateVertexAndC();
    }

    public void SetFocus(Vector2 focus)
    {
        this.focus = focus;
        CalculateVertexAndC();
    }

    public float GetY(float x)
    {
        return (x - vertex.x) * (x - vertex.x) / 4 / c + vertex.y;
    }

    private void CalculateVertexAndC()
    {
        c = (focus.y - standardLineY) / 2;
        vertex = new Vector2(focus.x, focus.y - c);
    }

    public Solution SolveTwoParabola(Parabola another)
    {
        float c1 = c;
        float c2 = another.c;
        float h1 = vertex.x;
        float h2 = another.vertex.x;
        float k1 = vertex.y;
        float k2 = another.vertex.y;

        float A = (c1 - c2);
        float B = 2 * (c2 * h1 - c1 * h2);
        float C = (c1 * h2 * h2 - c2 * h1 * h1 + 4 * (c1 * c2 * k2 - c1 * c2 * k1));

        float delta = B * B - 4 * A * C;

        if (delta < 0)
            return null;

        float sqrDelta = Mathf.Sqrt(delta);
        var from = new Solution.SolutionPoint((-B - sqrDelta) / 2 / A, another);
        var to = new Solution.SolutionPoint((-B + sqrDelta) / 2 / A, another);

        return new Solution(from, to);
    }
}