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
    List<Parabola> parabolas;
    public List<Point> points = new List<Point>();

    public static VoronoiDiagram instance;
    public DrawHandler.LineHandler lineHandler;

    LineRenderer lr;

    public void Awake()
    {
        instance = this;
    }

    public void Start()
    {
        lr = GetComponent<LineRenderer>();
        lineHandler = GetComponent<DrawHandler.LineHandler>();
    }

    void ResetScanY()
    {
        scanY = -scanRange;
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

        scanY += scanSpeed * Time.deltaTime;

        if (scanY >= scanRange)
            ResetScanY();


        lr.positionCount = 2;
        lr.SetPosition(0, new Vector3(-scanRange, scanY));
        lr.SetPosition(1, new Vector3(scanRange, scanY));

        foreach (var point in points)
            point.SetScanY(scanY);

        var targetPoints = points.Where(point => point.position.y < scanY).ToList();

        for (int i = 0; i < targetPoints.Count(); i++)
        {
            for (int j = i + 1; j < targetPoints.Count(); j++)
            {
                var thisPoint = targetPoints[i];
                var thatPoint = targetPoints[j];

                thisPoint.SolvePoint(thatPoint);
            }
        }
    }
}
