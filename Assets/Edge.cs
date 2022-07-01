using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edge : MonoBehaviour, IEdge
{
    Vector3 startPoint = Vector3.zero;
    Vector3 endPoint = Vector3.zero;
    LineRenderer lr;

    // Start is called before the first frame update
    void Start()
    {
        lr = GetComponent<LineRenderer>();
        lr.positionCount = 2;
    }

    public void SetStartPoint(Vector3 startPoint)
    {
        this.startPoint = startPoint;
    }

    public void SetEndPoint(Vector3 endPoint)
    {
        this.endPoint = endPoint;
    }

    void Update()
    {
        lr.SetPosition(0, startPoint);
        lr.SetPosition(1, endPoint);
    }

    public void DeleteSelf()
    {
        Destroy(this.gameObject);
    }
}
