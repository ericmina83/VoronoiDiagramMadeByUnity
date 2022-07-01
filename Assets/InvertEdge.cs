using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InvertEdge : IEdge
{
    public Edge edge;

    public InvertEdge(Edge edge)
    {
        this.edge = edge;
    }

    public void SetStartPoint(Vector3 startPoint)
    {
        edge.SetEndPoint(startPoint);
    }

    public void SetEndPoint(Vector3 endPoint)
    {
        edge.SetStartPoint(endPoint);
    }
}