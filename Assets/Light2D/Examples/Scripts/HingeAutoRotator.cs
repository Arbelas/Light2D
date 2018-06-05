using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


namespace Light2D.Examples
{
    public class HingeAutoRotator : MonoBehaviour
    {
        public float targetAngle;
        public bool worldAngle = true;
        public float speed = 1;
        public float maxSpeed = 360;
        public HingeJoint2D joint;
        private Rigidbody2D _jointRigidbody;

        private void Awake()
        {
            if (joint == null)
                joint = GetComponent<HingeJoint2D>();
            _jointRigidbody = joint.GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            float targetAngle = worldAngle ? this.targetAngle : _jointRigidbody.rotation - this.targetAngle;
            float rotTarget = Mathf.DeltaAngle(targetAngle, joint.connectedBody.rotation);
            JointMotor2D motor = joint.motor;
            motor.motorSpeed = Mathf.Clamp(-rotTarget*speed, -maxSpeed, maxSpeed);
            joint.motor = motor;
        }
    }
}