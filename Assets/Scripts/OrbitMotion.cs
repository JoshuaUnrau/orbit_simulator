using System;
using System.Collections;
using UnityEngine;

namespace DefaultNamespace
{
    public class OrbitMotion: MonoBehaviour
    {
        public Transform orbitingObject;
        public Ellipse orbitPath;
        public float orbitProgress;
        public float orbitPeriod = 30f;
        public bool orbitActive;
        
        private void Start()
        {
            if (orbitingObject == null)
            {
                orbitActive = false;
                return;
            }
            SetOrbitingObjectPosition();
            StartCoroutine(AnimateOrbit());
        }

        void SetOrbitingObjectPosition()
        {
            Vector2 orbitPos = orbitPath.evaluate(orbitProgress);
            orbitingObject.localPosition = new Vector3(orbitPos.x, 0, orbitPos.y);
        }

        IEnumerator AnimateOrbit()
        {
            if (orbitPeriod < 0.1f)
            {
                orbitPeriod = 0.1f;
            }
            float orbitSpeed = 1f / orbitPeriod;
            while (orbitActive)
            {
                orbitProgress += Time.deltaTime * orbitSpeed;
                orbitProgress %= 1f; //loop time over
                SetOrbitingObjectPosition();
                yield return null;
            }
            
        }
    }
}