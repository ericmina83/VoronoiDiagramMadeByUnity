using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(LineRenderer))]
public class DrawLine : MonoBehaviour
{
    public LineRenderer lr;
    public string parent;
    public Vector3 position;
    public float width = 0.1f;
    public Line line;

    public void SetColor(Color color)
    {
        lr.material.color = color;
    }

    void Awake()
    {
        lr = gameObject.GetComponent<LineRenderer>();
        // lr.material = lineMaterial;
        lr.startColor = Color.red;
        lr.endColor = Color.red;

        lr.startWidth = width;
        lr.endWidth = width;
    }

    #region Drawer

    public DrawLine DrawPoints(List<Vector3> points)
    {
        return DrawPoints(points.ToArray());
    }

    public DrawLine DrawPoints(Vector3[] points)
    {
        gameObject.SetActive(true);
        lr.positionCount = points.Length;

        lr.SetPositions(points);

        return this;
    }

    public void DeleteSelf()
    {
        Destroy(gameObject);
    }

    #endregion
}