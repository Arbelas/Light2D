using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


namespace Light2D.Examples
{
    public class SpiderLeg : MonoBehaviour
    {
        public Rigidbody2D body;
        public Transform connectedTransform;
        public Vector2 connectedAnchor;
        public float maxForce = 5000;
        public float maxMoveSpeed = 10;
        public float targetLength = 10;
        public float spring = 100;
        public float damper = 10;
        private Transform _transform;

        private void Awake()
        {
            _transform = transform;
        }

        private void LateUpdate()
        {
            if (connectedTransform == null || body == null)
                return;

            Vector2 worldAnchor = connectedTransform.TransformPoint(connectedAnchor);
            Vector2 worldOrigin = body.position;
            float length = (worldAnchor - worldOrigin).magnitude;

            _transform.position = worldOrigin;
            _transform.localScale = transform.localScale.WithY(length);
            _transform.rotation = Quaternion.Euler(0, 0, (worldOrigin - worldAnchor).AngleZ());
        }

        private void FixedUpdate()
        {
            if (connectedTransform == null || body == null)
                return;

            Vector2 worldAnchor = connectedTransform.TransformPoint(connectedAnchor);
            Vector2 worldOrigin = body.position;

            float length = (worldAnchor - worldOrigin).magnitude;
            float force = (targetLength - length)*spring;
            force -= body.velocity.magnitude*damper*Mathf.Sign(force);
            force = Mathf.Clamp(force, -maxForce, maxForce);
            Vector2 forceVec = (body.position - worldAnchor)/length*force;

            body.AddForce(forceVec, ForceMode2D.Force);
        }

        private void OnDrawGizmos()
        {
            if (connectedTransform == null || body == null)
                return;

            Vector2 worldAnchor = connectedTransform.TransformPoint(connectedAnchor);
            Vector2 worldOrigin = body.position;

            Gizmos.DrawLine(worldAnchor, worldOrigin);
        }
    }
}