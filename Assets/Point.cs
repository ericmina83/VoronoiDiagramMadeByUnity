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

            if (newParbola.IsXInParabola(solution.frPoint.x))
                newParbola.frPoint = solution.frPoint;

            if (newParbola.IsXInParabola(solution.toPoint.x))
                newParbola.toPoint = solution.toPoint;
        }

        lines.Add(newParbola);

        if (newParbola.frPoint.line == newParbola.toPoint.line)
        {
            var oldParabola = newParbola.frPoint.line;

            //    old       new       old
            // ---------|---------|---------
            // we must cut the old parabola

            //  oldFrom     new      newTo
            // ---------|---------|---------

            if (oldParabola != null)
            {
                // handle to (copy a new one)
                var newTo = oldParabola.CopySelf();
                newTo.frPoint = new Line.SolutionPoint(newParbola.toPoint.x, newParbola);
                lines.Add(newTo);
                newParbola.toPoint.line = newTo;

                // handle from
                var oldFr = oldParabola;
                oldFr.toPoint.line = newParbola;
                oldFr.toPoint.x = newParbola.frPoint.x;
            }
        }
        else
        {
            Debug.Log("fhdsjlfhjksadhksd");
            if (newParbola.frPoint.line != null)
            {
                var frParbola = newParbola.frPoint.line;
                frParbola.toPoint.line = newParbola;
                frParbola.toPoint.x = newParbola.frPoint.x;
            }

            if (newParbola.toPoint.line != null)
            {
                var toParbola = newParbola.toPoint.line;
                toParbola.frPoint.line = newParbola;
                toParbola.frPoint.x = newParbola.toPoint.x;
            }
        }
    }
}
