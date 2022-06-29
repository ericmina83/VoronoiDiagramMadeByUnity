using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottomLine : Line
{
    public float y;

    public BottomLine(float y) : base()
    {
        this.y = y;
    }

    override public float GetY(float x)
    {
        return y;
    }

    override public Solution SolveParabola(Parabola parabola)
    {
        float delta = Mathf.Sqrt((y - parabola.vertex.y) * 4 * parabola.c);
        var fr = new SolutionPoint(-delta + parabola.vertex.x, parabola);
        var to = new SolutionPoint(+delta + parabola.vertex.x, parabola);

        return new Solution(fr, to);
    }

    override public Solution SolveBottomLine(BottomLine bottomLine)
    {
        return new Solution(frPoint.CopySelf(), toPoint.CopySelf());
    }

    public override Line CopySelf()
    {
        var line = new BottomLine(y);
        return line;
    }
}