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
        public IEdge edge;

        public SolutionPoint(float x, Line line)
        {
            this.x = x;
            this.line = line;
        }

        public SolutionPoint CopySelf()
        {
            return new SolutionPoint(x, line);
        }

        public void Update(SolutionPoint point)
        {
            this.x = point.x;
            this.line = point.line;
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
        toPoint = new SolutionPoint(+VoronoiDiagram.instance.scanRange, null);

        drawLine = GameObject.Instantiate(VoronoiDiagram.instance.linePrefab);
        drawLine.line = this;
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
    public void Update(List<Line> lines, List<Edge> edges)
    {
        // solve from parabola
        var frParabola = frPoint.line;
        if (frParabola != null)
        {
            var frSolution = Solve(frParabola);
            var xForYJudgement = Random.Range(frSolution.frPoint.x, frSolution.toPoint.x);

            if (GetY(xForYJudgement) > frParabola.GetY(xForYJudgement))
                frPoint.Update(frSolution.frPoint);
            else
                frPoint.Update(frSolution.toPoint);

            if (frPoint.edge != null)
                frPoint.edge.SetEndPoint(new Vector3(frPoint.x, GetY(frPoint.x), 0));
        }

        // solve to parabola
        var toParabola = toPoint.line;
        if (toParabola != null)
        {
            var toSolution = Solve(toParabola);
            var xForYJudgement = Random.Range(toSolution.frPoint.x, toSolution.toPoint.x);

            if (GetY(xForYJudgement) > toParabola.GetY(xForYJudgement))
                toPoint.Update(toSolution.toPoint);
            else
                toPoint.Update(toSolution.frPoint);

            if (toPoint.edge != null)
                toPoint.edge.SetEndPoint(new Vector3(toPoint.x, GetY(toPoint.x), 0));
        }

        if (toPoint.x < frPoint.x)
        {
            if (frPoint.line != null)
                frPoint.line.toPoint = toPoint.CopySelf();

            if (toPoint.line != null)
                toPoint.line.frPoint = frPoint.CopySelf();

            lines = RemoveSelf(lines);

            if (frPoint.line != null)
                frPoint.line.Update(lines, edges);

            if (toPoint.line != null)
                toPoint.line.Update(lines, edges);

            if (toPoint.line != null && frPoint.line != null)
            {
                var newEdge = GameObject.Instantiate(VoronoiDiagram.instance.edgePrefab);
                toPoint.line.frPoint.edge = frPoint.line.toPoint.edge = newEdge;
                toPoint.line.frPoint.edge.SetStartPoint(new Vector3(toPoint.line.frPoint.x, toPoint.line.GetY(toPoint.line.frPoint.x), 0.0f));

                edges.Add(newEdge);
            }

        }
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

    public bool IsXInLine(float x)
    {
        return x < toPoint.x && x > frPoint.x;
    }

    public void DeleteLine()
    {
        drawLine.DeleteSelf();
    }

    public List<Line> RemoveSelf(List<Line> lines)
    {
        lines.Remove(this);
        DeleteLine();
        return lines;
    }
}