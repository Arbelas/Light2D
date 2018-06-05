using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Light2D.Examples
{
    public class Bat : MonoBehaviour
    {
        public Rect flyRect;
        public float moveSpeed;
        public float rotationLerpFactor;
        private Vector2 _flyTarget;
        private const float TargetSqDist = 1*1;

        void OnEnable()
        {
            FindNewFlyTarget();
        }

        void Update()
        {
            Vector3 pos = transform.position;
            Quaternion rot = transform.rotation;

            while (((Vector2) pos - _flyTarget).sqrMagnitude < TargetSqDist)
                FindNewFlyTarget();

            Vector3 direction = rot*new Vector3(0, 1, 0);
            Quaternion targetRot = Quaternion.Euler(0, 0, (_flyTarget - (Vector2)pos).AngleZ());
            transform.rotation = Quaternion.Lerp(rot, targetRot, rotationLerpFactor*Time.deltaTime);

            transform.position = pos + direction*moveSpeed*Time.deltaTime;
        }

        void FindNewFlyTarget()
        {
            float x = Random.Range(flyRect.xMin, flyRect.xMax);
            float y = Random.Range(flyRect.yMin, flyRect.yMax);
            _flyTarget = new Vector2(x, y);
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireCube(flyRect.center, flyRect.size);
        }
    }
}
