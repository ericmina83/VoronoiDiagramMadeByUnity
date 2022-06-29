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

    public void SolvePoint(List<Line> lines, float scanY)
    {
        var newParbola = new Parabola(position, scanY);

        foreach (var line in lines)
        {
            var solution = newParbola.Solve(line);

            if (newParbola.IsXInLine(solution.frPoint.x) && line.IsXInLine(solution.frPoint.x))
                newParbola.frPoint = solution.frPoint;

            if (newParbola.IsXInLine(solution.toPoint.x) && line.IsXInLine(solution.toPoint.x))
                newParbola.toPoint = solution.toPoint;
        }

        lines.Add(newParbola);

        if (newParbola.frPoint.line == newParbola.toPoint.line)
        {
            var oldParabola = newParbola.frPoint.line;

            //    old       new       old
            // ---------|---------|---------|-----
            // we must cut the old parabola

            //  oldFrom     new      newTo
            // ---------|---------|---------|-----

            if (oldParabola != null)
            {
                // handle to (copy a new one)
                var newTo = oldParabola.CopySelf();
                newTo.frPoint = new Line.SolutionPoint(newParbola.toPoint.x, newParbola);
                newTo.toPoint = oldParabola.toPoint;
                lines.Add(newTo);
                newParbola.toPoint.line = newTo;

                if (oldParabola.toPoint.line != null)
                    oldParabola.toPoint.line.frPoint.line = newTo;

                // handle from
                var oldFr = oldParabola;
                oldFr.toPoint = new Line.SolutionPoint(newParbola.frPoint.x, newParbola);
            }
        }
        else
        {
            if (newParbola.frPoint.line != null)
            {
                var frParabola = newParbola.frPoint.line;
                frParabola.toPoint.line = newParbola;
                frParabola.toPoint.x = newParbola.frPoint.x;
            }

            if (newParbola.toPoint.line != null)
            {
                var toParabola = newParbola.toPoint.line;
                toParabola.frPoint.line = newParbola;
                toParabola.frPoint.x = newParbola.toPoint.x;
            }
        }
    }
}
