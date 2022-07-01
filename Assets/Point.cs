using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point : MonoBehaviour
{
    public Vector3 position
    {
        get
        {
            return transform.position;
        }
    }

    // Start is called before the first frame update

    public void SolvePoint(List<Line> lines, float scanY, List<Edge> edges)
    {
        var newParbola = new Parabola(position, scanY);

        Line highestLine = null;
        var highestY = -VoronoiDiagram.instance.scanRange - 1;

        foreach (var line in lines)
        {
            var newY = line.GetY(position.x);
            if (newY > highestY)
            {
                if (line.IsXInLine(position.x))
                {
                    highestLine = line;
                    highestY = newY;
                }
            }
        }

        newParbola.frPoint.x = newParbola.toPoint.x = position.x;
        newParbola.frPoint.line = newParbola.toPoint.line = highestLine;

        lines.Add(newParbola);

        if (highestLine != null)
        {
            //  highest     new     highest 
            // ---------|---------|---------|-----
            // we must cut the old parabola

            //  highest     new      newTo
            // ---------|---------|---------|-----

            // handle to (copy a new one)
            var newTo = highestLine.CopySelf();
            newTo.frPoint = new Line.SolutionPoint(position.x, newParbola);
            newTo.toPoint = highestLine.toPoint;
            lines.Add(newTo);
            newParbola.toPoint.line = newTo;

            if (highestLine.toPoint.line != null)
                highestLine.toPoint.line.frPoint.line = newTo;

            // handle highest
            highestLine.toPoint = new Line.SolutionPoint(position.x, newParbola);
            var newEdge = GameObject.Instantiate(VoronoiDiagram.instance.edgePrefab);

            newParbola.toPoint.edge = newTo.frPoint.edge = newEdge;
            newParbola.frPoint.edge = highestLine.toPoint.edge = new InvertEdge(newEdge);

            newParbola.frPoint.edge.SetStartPoint(new Vector3(position.x, highestY, 0));
            newParbola.toPoint.edge.SetStartPoint(new Vector3(position.x, highestY, 0));

            edges.Add(newEdge);
        }
        // else
        // {
        //     if (newParbola.frPoint.line != null)
        //     {
        //         var frParabola = newParbola.frPoint.line;
        //         frParabola.toPoint.line = newParbola;
        //         frParabola.toPoint.x = newParbola.frPoint.x;

        //         newParbola.frPoint.edge = frParabola.toPoint.edge = GameObject.Instantiate(VoronoiDiagram.instance.edgePrefab);
        //         newParbola.frPoint.edge.AddPoint(new Vector3(newParbola.frPoint.x, newParbola.GetY(newParbola.frPoint.x), 0));
        //     }

        //     if (newParbola.toPoint.line != null)
        //     {
        //         var toParabola = newParbola.toPoint.line;
        //         toParabola.frPoint.line = newParbola;
        //         toParabola.frPoint.x = newParbola.toPoint.x;

        //         newParbola.toPoint.edge = toParabola.frPoint.edge = GameObject.Instantiate(VoronoiDiagram.instance.edgePrefab);
        //         newParbola.toPoint.edge.AddPoint(new Vector3(newParbola.toPoint.x, newParbola.GetY(newParbola.toPoint.x), 0));
        //     }
        // }
    }
}
