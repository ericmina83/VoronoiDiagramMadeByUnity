using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class VoronoiDiagram : MonoBehaviour
{
    public float scanRange = 5f;
    public float scanY = -10.0f;
    public float scanSpeed = 1.0f;

    public Point point;
    public List<Point> points = new List<Point>();
    public List<Point> addedPoints = new List<Point>();
    public static VoronoiDiagram instance;

    [SerializeField]
    List<Line> lines = new List<Line>();
    public DrawLine linePrefab;
    public Edge edgePrefab;
    public List<Edge> edges = new List<Edge>();


    LineRenderer lr;

    public void Awake()
    {
        instance = this;
    }

    public void Start()
    {
        lr = GetComponent<LineRenderer>();
        ResetScanY();
    }

    void ResetScanY()
    {
        scanY = -scanRange;

        foreach (var line in lines)
            line.DeleteLine();
        lines.Clear();

        foreach (var edge in edges)
            edge.DeleteSelf();
        edges.Clear();

        addedPoints.Clear();
        lines.Add(new BottomLine(-scanRange));
    }

    public void HandleAddNewPoint()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var mousePosition = Input.mousePosition;
            mousePosition.z = Camera.main.nearClipPlane + 2;
            var worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

            if (Mathf.Abs(worldPosition.x) < 5 && Mathf.Abs(worldPosition.y) < 5)
            {
                points.Add(Instantiate(point, worldPosition, Quaternion.identity));
                points = (from point in points orderby -point.position.y select point).ToList();

                ResetScanY();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        HandleAddNewPoint();

        var newScanPoint = RunScanY();

        foreach (var line in new List<Line>(lines))
            line.Update(lines, edges);

        if (newScanPoint != null)
            newScanPoint.SolvePoint(lines, scanY, edges);

        foreach (var parabola in lines)
        {
            parabola.UpdateDrawPoints();
        }
    }

    Point RunScanY()
    {
        scanY += scanSpeed * Time.deltaTime;

        if (scanY >= scanRange)
            ResetScanY();

        Point newScanPoint = null;

        foreach (var point in points)
        {
            if (point.position.y < scanY)
            {
                if (!addedPoints.Contains(point))
                {
                    newScanPoint = point;
                    scanY = newScanPoint.position.y;
                    addedPoints.Add(newScanPoint);
                    break;
                }
            }
        }

        foreach (var line in lines)
        {
            if (line is Parabola)
                (line as Parabola).SetStandardLineY(scanY);
        }

        lr.positionCount = 2;
        lr.SetPosition(0, new Vector3(-scanRange, scanY));
        lr.SetPosition(1, new Vector3(scanRange, scanY));

        return newScanPoint;
    }

}
