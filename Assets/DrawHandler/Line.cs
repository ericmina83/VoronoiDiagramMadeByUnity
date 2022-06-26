using UnityEngine;
using System.Collections.Generic;

namespace DrawHandler
{
    [RequireComponent(typeof(LineRenderer))]
    public class Line : MonoBehaviour
    {
        private LineRenderer lr;
        public string parent;
        public Vector3 position;
        public float width = 0.1f;

        public void SetColor(Color color)
        {
            lr.material.color = color;
        }

        void Awake()
        {
            lr = gameObject.GetComponent<LineRenderer>();
            // lr.material = lineMaterial;

            lr.startWidth = width;
            lr.endWidth = width;
        }

        #region Drawer

        public Line DrawPoints(List<Vector3> points)
        {
            return DrawPoints(points.ToArray());
        }

        public Line DrawPoints(Vector3[] points)
        {
            gameObject.SetActive(true);
            lr.positionCount = points.Length;

            lr.SetPositions(points);

            return this;
        }


        public Line DrawCircle(Vector3 center, float radius)
        {
            // int segementCount = 40;

            // lr.positionCount = segementCount + 1;

            // for (int i = 0; i <= segementCount; i++)
            // {
            //     float angleRad = i * 2.0f * Mathf.PI / segementCount;
            //     Vector3 circlePosition = center + new Vector3(radius * Mathf.Cos(angleRad), 0.0f, radius * Mathf.Sin(angleRad));
            //     lr.SetPosition(i, circlePosition);
            // }

            DrawAngle(center, radius, 0, 360);

            return this;
        }

        public Line DrawLine(Vector3 pt1, Vector3 pt2)
        {
            gameObject.SetActive(true);
            lr.positionCount = 2;

            lr.SetPosition(0, pt1);
            lr.SetPosition(1, pt2);

            return this;
        }

        public Line DrawAngle(Vector3 center, Vector3 fromPoint, Vector3 toPoint, float radius)
        {
            gameObject.SetActive(true);
            int segementCount = 40;

            lr.positionCount = segementCount + 1;

            var fromVec = (fromPoint - center).normalized * radius;
            var toVec = toPoint - center;

            var normal = Vector3.Cross(fromVec, toVec);

            float angleStep = Vector3.SignedAngle(fromVec, toVec, normal) / segementCount;

            for (int i = 0; i <= segementCount; i++)
            {
                lr.SetPosition(i, center + Quaternion.AngleAxis(angleStep * i, normal) * fromVec);
            }

            return this;
        }

        public Line DrawAngle(Vector3 center, float radius, float startAngle, float endAngle)
        {
            gameObject.SetActive(true);
            int segementCount = 40;

            lr.positionCount = segementCount + 1;

            float interval = (endAngle - startAngle) / segementCount;

            List<Vector3> points = new List<Vector3>();

            for (int i = 0; i <= segementCount; i++)
            {
                float angleRad = (startAngle + interval * i) * Mathf.Deg2Rad;
                Vector3 circlePosition = center + new Vector3(radius * Mathf.Cos(angleRad), 0.0f, radius * Mathf.Sin(angleRad));
                lr.SetPosition(i, circlePosition);
            }

            return this;
        }

        public void DeleteSelf()
        {
            Destroy(gameObject);
        }

        #endregion
    }
}
