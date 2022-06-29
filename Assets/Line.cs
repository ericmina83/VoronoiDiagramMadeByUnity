using System.Collections;
using System.Collections.Generic;
using UnityEngine;


abstract public class Line
{
    private static int segmentCount = 40;

    public class SolutionPoint
    {
        public float x; // x postion
        public Line line;

        public SolutionPoint(float x, Line line)
        {
            this.x = x;
            this.line = line;
        }

        public SolutionPoint CopySelf()
        {
            return new SolutionPoint(x, line);
        }
    }

    public class Solution
    {
        public SolutionPoint frPoint;
        public SolutionPoint toPoint;

        public Solution(SolutionPoint frPoint, SolutionPoint toPoint)
        {
            if (frPoint.x > toPoint.x)
            {
                this.toPoint = frPoint;
                this.frPoint = toPoint;
            }
            else
            {
                this.frPoint = frPoint;
                this.toPoint = toPoint;
            }
        }

        public bool IsXInSolution(float x)
        {
            return x < toPoint.x && x > frPoint.x;
        }
    }

    public Line()
    {
        frPoint = new SolutionPoint(-VoronoiDiagram.instance.scanRange, null);
        frPoint = new SolutionPoint(+VoronoiDiagram.instance.scanRange, null);

        drawLine = GameObject.Instantiate(VoronoiDiagram.instance.linePrefab);
    }

    public SolutionPoint frPoint; // from point
    public SolutionPoint toPoint; // to point
    public DrawLine drawLine;

    public Solution Solve(Line line)
    {
        if (line is Parabola)
            return SolveParabola((Parabola)line);
        else if (line is BottomLine)
            return SolveBottomLine((BottomLine)line);

        return null;
    }

    // if return true, means this parablo need to remove
    public bool Update(List<Line> lines)
    {
        if (!lines.Contains(this))
            return false;

        // solve from parabola
        var frParabola = frPoint.line;
        if (frParabola != null)
        {
            var frSolution = Solve(frParabola);
            var xForYJudgement = Random.Range(frSolution.frPoint.x, frSolution.toPoint.x);

            if (GetY(xForYJudgement) > frParabola.GetY(xForYJudgement))
                frPoint = frSolution.frPoint;
            else
                frPoint = frSolution.toPoint;
        }


        // solve to parabola
        var toParabola = toPoint.line;
        if (toParabola != null)
        {
            var toSolution = Solve(toParabola);
            var xForYJudgement = Random.Range(toSolution.frPoint.x, toSolution.toPoint.x);

            if (GetY(xForYJudgement) > toParabola.GetY(xForYJudgement))
                toPoint = toSolution.toPoint;
            else
                toPoint = toSolution.frPoint;
        }

        if (toPoint.x < frPoint.x)
        {
            if (frPoint.line != null)
                frPoint.line.toPoint.line = toPoint.line;

            if (toPoint.line != null)
                toPoint.line.frPoint.line = frPoint.line;

            lines.Remove(this);

            if (frPoint.line != null)
                frPoint.line.Update(lines);

            if (toPoint.line != null)
                toPoint.line.Update(lines);
        }

        return false;
    }

    abstract public Solution SolveParabola(Parabola line);
    abstract public Solution SolveBottomLine(BottomLine line);

    public void UpdateDrawPoints()
    {
        List<Vector3> points = new List<Vector3>();

        float x_step = (toPoint.x - frPoint.x) / segmentCount;

        for (int i = 0; i <= segmentCount; i++)
        {
            float x = frPoint.x + i * x_step;
            points.Add(new Vector3(x, GetY(x), 0f));
        }

        drawLine.DrawPoints(points);
    }

    abstract public float GetY(float x);
    abstract public Line CopySelf();

    public bool IsXInParabola(float x)
    {
        return x < toPoint.x && x > frPoint.x;
    }

    public void DeleteLine()
    {
        drawLine.DeleteSelf();
    }
}