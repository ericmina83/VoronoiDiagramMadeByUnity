using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point : MonoBehaviour
{
    LineRenderer lr;

    private float scanY;
    int segmentCount = 100;
    List<Parabola.Solution> solutions = new List<Parabola.Solution>();
    public Parabola parabola;

    public DrawHandler.LineHandler lineHandler;

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

        if (parabola == null)
            parabola = new Parabola(transform.position);

        lineHandler = GetComponent<DrawHandler.LineHandler>();
    }

    public void SetScanY(float scanY)
    {
        this.scanY = scanY;
        if (parabola == null)
            parabola = new Parabola(scanY, transform.position);
        else
            parabola.SetStandardLineY(scanY);

        solutions.Clear(); // clear all solution for next round
        solutions.Add(new Parabola.Solution(
            new Parabola.Solution.SolutionPoint(-VoronoiDiagram.instance.scanRange, null),
            new Parabola.Solution.SolutionPoint(VoronoiDiagram.instance.scanRange, null)
        ));
    }

    public void SolvePoint(Point another)
    {
        var newSolution = parabola.SolveTwoParabola(another.parabola);

        foreach (var thisSolution in solutions)
        {
            if (thisSolution.isPointBetweenSolution(newSolution.from))
                thisSolution.from = newSolution.from;

            if (thisSolution.isPointBetweenSolution(newSolution.to))
                thisSolution.to = newSolution.to;

            // var newThatSolutions = new List<Parabola.Solution>();

            // foreach (var thatSolution in another.solutions)
            // {
            //     // handle from solution point
            //     if (thatSolution.isPointBetweenSolution(thisSolution.from) && // if
            //         thisSolution.from.targetParabola == another.parabola)

            //         // newSolution
            //         if (thisSolution.from < thatSolution.to && thisSolution.from > thatSolution.from)
            //         {
            //             newThatSolutions.Add(new Parabola.Solution())
            //         }
            // }
            // another.solutions = newThatSolutions;
        }


        // becuase I'm higher than other points, I must between my solution.x1 and solution.x2


        // return
        //     from_x < VoronoiDiagram.instance.scanRange &&
        //     to_x > VoronoiDiagram.instance.scanRange;
    }


    // Update is called once per frame
    void Update()
    {
        if (scanY < position.y)
        {
            lr.positionCount = 0;
            return;
        }


        // foreach (var another in VoronoiDiagram.instance.points)
        // {
        //     if (another == this)
        //         break;

        //     foreach (var thisSolution in solutions)
        //     {
        //         foreach (var thatSolution in another.solutions)
        //         {
        //             SolvePoint(another);
        //         }
        //     }
        // }

        lr.positionCount = segmentCount + 1;

        foreach (var solution in solutions)
        {
            float x_step = (solution.to.x - solution.from.x) / segmentCount;

            for (int i = 0; i <= segmentCount; i++)
            {
                float x = solution.from.x + i * x_step;
                lr.SetPosition(i, new Vector3(x, parabola.GetY(x), 0f));
            }
        }

        // lr.positionCount = segmentCount + 1;


    }
}
