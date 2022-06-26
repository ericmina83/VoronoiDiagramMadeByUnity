using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace DrawHandler
{
    public class LineHandler : MonoBehaviour
    {
        public Line linePrefab;

        private readonly Dictionary<string, GameObject> parents = new Dictionary<string, GameObject>();

        #region Clear Line

        public void ClearLines(string parent)
        {
            if (!parents.ContainsKey(parent))
                return;

            Destroy(parents[parent]);

            parents.Remove(parent);
        }

        public void ClearAllLines()
        {
            foreach (var pair in parents)
            {
                //line.gameObject.SetActive(false);

                Destroy(pair.Value);
            }

            parents.Clear();
        }

        #endregion

        #region Draw Line

        public Line DrawLine(Vector3 pt1, Vector3 pt2, string parent, Color color)
        {
            return AddLine(GetParent(parent), color, Vector3.zero).DrawLine(pt1, pt2);
        }

        #endregion

        #region Draw Circle

        public Line DrawCircle(Vector3 center, float radius, string parent, Color color)
        {
            return AddLine(GetParent(parent), color, center).DrawCircle(center, radius);
        }

        #endregion

        #region Draw Points

        public Line DrawPoints(List<Vector3> points, string parent, Color color)
        {
            return AddLine(GetParent(parent), color, Vector3.zero).DrawPoints(points);
        }


        public Line DrawPoints(Vector3[] points, string parent, Color color)
        {
            return AddLine(GetParent(parent), color, Vector3.zero).DrawPoints(points);
        }


        #endregion

        #region Draw Angle

        public Line DrawAngle(Vector3 center, Vector3 fromPoint, Vector3 toPoint, float radius, string parent, Color color)
        {
            return AddLine(GetParent(parent), color, center).DrawAngle(center, fromPoint, toPoint, radius);
        }

        public Line DrawAngle(Vector3 center, float radius, float startAngle, float endAngle, string parent, Color color)
        {
            return AddLine(GetParent(parent), color, center).DrawAngle(center, radius, startAngle, endAngle);
        }

        #endregion

        #region Private

        Line AddLine(GameObject parent, Color color, Vector3 position)
        {
            Line lineObj = Instantiate(linePrefab, parent.transform);

            lineObj.parent = parent.name;
            lineObj.SetColor(color);
            lineObj.position = position;

            return lineObj;
        }

        GameObject GetParent(string parent)
        {
            if (!parents.ContainsKey(parent))
            {
                var parentObj = new GameObject(parent);
                parentObj.transform.parent = transform;
                parents.Add(parent, parentObj);
                return parentObj;
            }


            return parents[parent];
        }

        #endregion
    }
}