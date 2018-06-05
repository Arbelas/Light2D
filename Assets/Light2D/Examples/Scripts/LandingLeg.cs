using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


namespace Light2D.Examples
{
    public class LandingLeg : MonoBehaviour
    {
        public bool release = false;
        public float releasedAngle;
        public float hiddenAngle;
        public HingeAutoRotator autoRotator;

        public void Start()
        {
            if (autoRotator == null)
                autoRotator = GetComponent<HingeAutoRotator>();
        }

        private void Update()
        {
            autoRotator.targetAngle = release ? releasedAngle : hiddenAngle;
        }
    }
}
