using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Random = UnityEngine.Random;


namespace Light2D.Examples
{
    public class RocketEngine : MonoBehaviour
    {
        public float forcePercent;
        public float maxForce;
        public ParticleSystem particles;
        public Rigidbody2D rigidbody;
        public Vector2 localForceDirection = Vector3.up;
        public bool isEnabled = true;
        private ParticleSystem[] _allParticles;
        private Transform _myTransform;
        private Transform _rigidbodyTransform;

        private void Awake()
        {
            _myTransform = transform;
            _rigidbodyTransform = rigidbody.transform;
            _allParticles = particles.GetComponentsInChildren<ParticleSystem>();
            localForceDirection = localForceDirection.normalized;
        }

        private void Update()
        {
            foreach (ParticleSystem particle in _allParticles)
                particle.enableEmission = isEnabled && forcePercent >= Random.value;
        }

        private void FixedUpdate()
        {
            if (!isEnabled) return;
            Vector3 pos = transform.position;
            Vector3 force = _myTransform.TransformDirection(localForceDirection)*maxForce*Mathf.Clamp01(forcePercent);
            rigidbody.AddForceAtPosition(force, pos);
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawRay(transform.position, transform.TransformDirection(-localForceDirection));
        }
    }
}