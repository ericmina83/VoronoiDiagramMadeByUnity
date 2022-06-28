using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public class Parabola
{
    public class SolutionPoint
    {
        public float x; // x postion
        public Parabola parabola;

        public SolutionPoint(float x, Parabola target)
        {
            this.x = x;
            this.parabola = target;
        }
    }

    public class Solution
    {
        public SolutionPoint from;
        public SolutionPoint to;

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
    }


    static int segmentCount = 40;

    public float standardLineY; // k - c
    public Vector2 focus; // (h, k + c)
    public Vector2 vertex; // (h, k)
    public float c;
    public SolutionPoint frPoint; // from point
    public SolutionPoint toPoint; // to point

    public Parabola(Parabola copy)
    {
        this.standardLineY = copy.standardLineY;
        this.focus = copy.focus;
        this.vertex = copy.vertex;
        this.c = copy.c;
        this.frPoint = copy.frPoint;
        this.toPoint = copy.toPoint;
    }

    public Parabola(Vector2 focus, float standardLineY)
    {
        this.focus = focus;
        this.standardLineY = standardLineY;
        this.frPoint = new SolutionPoint(-VoronoiDiagram.instance.scanRange, null);
        this.toPoint = new SolutionPoint(VoronoiDiagram.instance.scanRange, null);
        CalculateVertexAndC();
    }

    public void SetStandardLineY(float standardLineY)
    {
        this.standardLineY = standardLineY;
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
        var from = new SolutionPoint((-B - sqrDelta) / 2 / A, another);
        var to = new SolutionPoint((-B + sqrDelta) / 2 / A, another);

        return new Solution(from, to);
    }


    // if return true, means this parablo need to remove
    public bool SolveToParabola(List<Parabola> parabolas)
    {
        if (!parabolas.Contains(this))
            return false;

        var anotherParabola = toPoint.parabola;

        if (anotherParabola == null)
            return false;

        var solution = SolveTwoParabola(anotherParabola);
        SolutionPoint targetPoint;

        if (focus.y > anotherParabola.focus.y)
            targetPoint = solution.to;
        else
            targetPoint = solution.from;

        anotherParabola.frPoint.x = toPoint.x = targetPoint.x;

        if (toPoint.x < frPoint.x)
        {
            if (frPoint.parabola != null)
                frPoint.parabola.toPoint.parabola = toPoint.parabola;

            if (toPoint.parabola != null)
                toPoint.parabola.frPoint.parabola = frPoint.parabola;

            parabolas.Remove(this);

            if (frPoint.parabola != null)
                frPoint.parabola.SolveToParabola(parabolas);
        }
        else if (toPoint.x > VoronoiDiagram.instance.scanRange)
        {
            RemoveTo(parabolas);
        }

        return false;
    }

    public void RemoveTo(List<Parabola> parabolas)
    {
        if (toPoint.parabola != null)
        {
            toPoint.parabola.RemoveTo(parabolas);
            parabolas.Remove(toPoint.parabola);
            toPoint.parabola = null;
            toPoint.x = VoronoiDiagram.instance.scanRange;
        }
    }

    public bool IsPointInParabola(SolutionPoint point)
    {
        return point.x < toPoint.x && point.x > frPoint.x;
    }

    public List<Vector3> GetDrawPoints()
    {
        List<Vector3> line = new List<Vector3>();

        float x_step = (toPoint.x - frPoint.x) / segmentCount;

        for (int i = 0; i <= segmentCount; i++)
        {
            float x = frPoint.x + i * x_step;
            line.Add(new Vector3(x, GetY(x), 0f));
        }

        return line;
    }
}