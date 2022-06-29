using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edge : MonoBehaviour
{
    List<Vector3> points;
    LineRenderer lr;

    // Start is called before the first frame update
    void Start()
    {
        lr = GetComponent<LineRenderer>();
    }

    public void AddPoint(Vector3 point)
    {
        points.Add(point);
    }

    // Update is called once per frame
    void Update()
    {
        if (points.Count > 1)
        {
            lr.positionCount = points.Count;
            lr.SetPositions(points.ToArray());
        }
    }
}
