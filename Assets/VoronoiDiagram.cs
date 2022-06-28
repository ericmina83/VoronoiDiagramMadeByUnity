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
    public DrawHandler.LineHandler lineHandler;
    List<Parabola> parabolas = new List<Parabola>();

    LineRenderer lr;

    string lineParent = "haha";

    public void Awake()
    {
        instance = this;
    }

    public void Start()
    {
        lr = GetComponent<LineRenderer>();
        lineHandler = GetComponent<DrawHandler.LineHandler>();
        ResetScanY();
    }

    void ResetScanY()
    {
        scanY = -scanRange;
        addedPoints.Clear();
        parabolas.Clear();
    }

    // Update is called once per frame
    void Update()
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

        RunScanY();

        foreach (var point in points)
        {
            if (point.position.y < scanY)
            {
                if (!addedPoints.Contains(point))
                {
                    point.SolvePoint(parabolas, scanY);
                    addedPoints.Add(point);
                    break;
                }
            }
        }

        foreach (var parabola in new List<Parabola>(parabolas))
        {
            parabola.SolveToParabola(parabolas);
        }

        lineHandler.ClearLines(lineParent);

        foreach (var parabola in parabolas)
        {
            lineHandler.DrawPoints(parabola.GetDrawPoints(), lineParent, Color.white);
        }
    }

    void RunScanY()
    {
        scanY += scanSpeed * Time.deltaTime;
        if (scanY >= scanRange)
            ResetScanY();

        foreach (var parabola in parabolas)
            parabola.SetStandardLineY(scanY);

        lr.positionCount = 2;
        lr.SetPosition(0, new Vector3(-scanRange, scanY));
        lr.SetPosition(1, new Vector3(scanRange, scanY));
    }
}
