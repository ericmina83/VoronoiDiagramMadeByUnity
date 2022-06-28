using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point : MonoBehaviour
{
    private LineRenderer lr;
    public List<Parabola.Solution> solutions = new List<Parabola.Solution>();

    public Vector3 position
    {
        get
        {
            return transform.position;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        lr = GetComponent<LineRenderer>();
    }

    public void SolvePoint(List<Parabola> parabolas, float scanY)
    {
        var newParbola = new Parabola(position, scanY);

        foreach (var parabola in parabolas)
        {
            var solution = newParbola.SolveTwoParabola(parabola);

            // becuase I'm higher than other points, I must between my solution.x1 and solution.x2
            if (newParbola.IsPointInParabola(solution.from))
                newParbola.frPoint = solution.from;

            if (newParbola.IsPointInParabola(solution.to))
                newParbola.toPoint = solution.to;
        }

        parabolas.Add(newParbola);

        if (newParbola.frPoint.parabola == newParbola.toPoint.parabola)
        {
            var oldParabola = newParbola.frPoint.parabola;

            //    old       new       old
            // ---------|---------|---------
            // we must cut the old parabola

            //  oldFrom     new      newTo
            // ---------|---------|---------

            if (oldParabola != null)
            {
                // handle to (copy a new one)
                var newTo = new Parabola(oldParabola.focus, oldParabola.standardLineY);
                newTo.frPoint = new Parabola.SolutionPoint(newParbola.toPoint.x, newParbola);
                newTo.toPoint = new Parabola.SolutionPoint(oldParabola.toPoint.x, oldParabola.toPoint.parabola);
                parabolas.Add(newTo);
                newParbola.toPoint.parabola = newTo;

                // handle from
                var oldFr = oldParabola;
                oldFr.toPoint.parabola = newParbola;
                oldFr.toPoint.x = newParbola.frPoint.x;
            }
        }
        else
        {
            Debug.Log("fhdsjlfhjksadhksd");
            if (newParbola.frPoint.parabola != null)
            {
                var frParbola = newParbola.frPoint.parabola;
                frParbola.toPoint.parabola = newParbola;
                frParbola.toPoint.x = newParbola.frPoint.x;
            }

            if (newParbola.toPoint.parabola != null)
            {
                var toParbola = newParbola.toPoint.parabola;
                toParbola.frPoint.parabola = newParbola;
                toParbola.frPoint.x = newParbola.toPoint.x;
            }
        }

    }
}
