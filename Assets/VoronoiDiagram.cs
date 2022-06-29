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

        addedPoints.Clear();
        lines.Clear();

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

        RunScanY();

        foreach (var point in points)
        {
            if (point.position.y < scanY)
            {
                if (!addedPoints.Contains(point))
                {
                    point.SolvePoint(lines, scanY);
                    addedPoints.Add(point);
                    break;
                }
            }
        }

        foreach (var line in new List<Line>(lines))
        {
            line.Update(lines);
        }

        foreach (var parabola in lines)
        {
            parabola.UpdateDrawPoints();
        }
    }

    void RunScanY()
    {
        scanY += scanSpeed * Time.deltaTime;
        if (scanY >= scanRange)
            ResetScanY();

        foreach (var line in lines)
        {
            if (line is Parabola)
                (line as Parabola).SetStandardLineY(scanY);
        }

        lr.positionCount = 2;
        lr.SetPosition(0, new Vector3(-scanRange, scanY));
        lr.SetPosition(1, new Vector3(scanRange, scanY));
    }
}
