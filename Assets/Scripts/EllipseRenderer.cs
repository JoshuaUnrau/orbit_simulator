using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class EllipseRenderer : MonoBehaviour
    {
        public LineRenderer lr;
        
        [Range(3,100)] public int segments = 3;
        public Ellipse ellipse = new Ellipse(10,100);
        
        private void Start()
        {
            CalculateEllipse();
        }

        public void CalculateEllipse()
        {
            Vector3[] points = new Vector3[segments + 1];
            for (int i = 0; i < segments; i++)
            {
                Vector2 position2D = ellipse.evaluate((float) i / (float) segments);
                points[i] = new Vector3(position2D.x, 0f, position2D.y);
            }

            points[segments] = points[0];
            lr.positionCount = segments + 1;
            lr.SetPositions(points);
        }

        private void OnValidate()
        {
            CalculateEllipse();
        }
    }
}