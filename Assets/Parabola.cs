using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parabola : Line
{
    public float standardLineY; // k - c
    public Vector2 focus; // (h, k + c)
    public Vector2 vertex; // (h, k)
    public float c;

    public Parabola(Vector2 focus, float standardLineY) : base()
    {
        this.focus = focus;
        this.standardLineY = standardLineY;
        CalculateVertexAndC();
    }

    public void SetStandardLineY(float standardLineY)
    {
        this.standardLineY = standardLineY;
        CalculateVertexAndC();
    }

    private void CalculateVertexAndC()
    {
        c = (focus.y - standardLineY) / 2;
        vertex = new Vector2(focus.x, focus.y - c);
    }


    override public float GetY(float x)
    {
        if (c == 0)
            return focus.y;

        return (x - vertex.x) * (x - vertex.x) / 4 / c + vertex.y;
    }

    override public Solution SolveBottomLine(BottomLine bottomLine)
    {
        float delta = (bottomLine.y - vertex.y) * 4 * c;
        if (delta < 0)
            return null;

        float sqrDelta = Mathf.Sqrt(delta);
        var fr = new SolutionPoint(-sqrDelta + vertex.x, bottomLine);
        var to = new SolutionPoint(+sqrDelta + vertex.x, bottomLine);

        return new Solution(fr, to);
    }

    override public Solution SolveParabola(Parabola another)
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
        {
            return null;
        }

        float sqrDelta = Mathf.Sqrt(delta);
        var from = new SolutionPoint((-B - sqrDelta) / 2 / A, another);
        var to = new SolutionPoint((-B + sqrDelta) / 2 / A, another);

        return new Solution(from, to);
    }

    public override Line CopySelf()
    {
        var copy = new Parabola(focus, standardLineY);

        return copy;
    }

}