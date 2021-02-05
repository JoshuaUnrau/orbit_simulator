using System;
using UnityEngine;

[System.Serializable]
    public class Ellipse
    {
        public float offsetAngle;
        public Vector2 offset;
        public float xAxis;
        public float yAxis;
        public Vector3 offsetPoint;

        public Ellipse(float xAxis, float yAxis)
        {
            this.xAxis = xAxis;
            this.yAxis = yAxis;
        }
        
        public Vector2 evaluate(float t)
        {
            float x;
            float y;
            float angle = Mathf.Deg2Rad * 360f * t;
            x = Mathf.Sin(angle) * xAxis + offset.x;
            y = Mathf.Cos(angle) * yAxis + offset.y;
            float radius = Vector3.Distance(offsetPoint, new Vector3(x, 0, y));
            //x *= Mathf.Cos(offsetAngle);
            //y *= Mathf.Sin(offsetAngle);
            //x += radius * Mathf.Cos(offsetAngle);
            //y += radius * Mathf.Sin(offsetAngle);
            float x_n = Mathf.Cos(-offsetAngle) * (x - offsetPoint.x) - (y - offsetPoint.y)*Mathf.Sin(-offsetAngle) + offsetPoint.x;
            float y_n = Mathf.Sin(-offsetAngle) * (x - offsetPoint.x) + (y - offsetPoint.y)*Mathf.Cos(-offsetAngle) + offsetPoint.y;
            x = x_n;
            y = y_n;
            return new Vector2(x, y);
        }
    }
