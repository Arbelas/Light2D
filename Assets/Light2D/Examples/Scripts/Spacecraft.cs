using System.Linq;
using UnityEngine;
using System.Collections;

namespace Light2D.Examples
{
    public class Spacecraft : MonoBehaviour
    {
        public bool releaseLandingGear = false;
        public RocketEngine bottomLeftEngine;
        public RocketEngine bottomRightEngine;
        public RocketEngine sideLeftEngine;
        public RocketEngine sideRightEngine;
        public RocketEngine reverseLeftEngine;
        public RocketEngine reverseRightEngine;
        public Rigidbody2D mainRigidbody;
        public GameObject flaresPrefab;
        public Vector2 rightFlareSpawnPos = new Vector3(1.87f, -0.28f, 0);
        public Vector2 rightFlareVelocity;
        public float flareAngularVelocity;
        private LandingLeg[] _landingLegs;

        private void Awake()
        {
            _landingLegs = GetComponentsInChildren<LandingLeg>(true);
        }

        private void Start()
        {
            BalanceCenterOfMass();
            FixCollision();
        }

        private void FixCollision()
        {
            Collider2D[] colliders = GetComponentsInChildren<Collider2D>();
            foreach (Collider2D coll1 in colliders)
            {
                foreach (Collider2D coll2 in colliders)
                {
                    if (coll1 != coll2)
                        Physics2D.IgnoreCollision(coll1, coll2);
                }
            }
        }

        private void BalanceCenterOfMass()
        {
            Rigidbody2D[] rigidbodies = GetComponentsInChildren<Rigidbody2D>();
            IGrouping<string, Rigidbody2D>[] groups = rigidbodies
                .GroupBy(rb => rb.name.Replace("Left", "").Replace("Right", ""))
                .ToArray();
            foreach (IGrouping<string, Rigidbody2D> group in groups)
            {
                Vector3 mainCenterOfMass = transform.InverseTransformPoint(group.First().worldCenterOfMass);
                foreach (Rigidbody2D rb in group)
                {
                    Vector3 cm = transform.InverseTransformPoint(rb.worldCenterOfMass);
                    if (Mathf.Abs(mainCenterOfMass.x + cm.x) < 0.02f && Mathf.Abs(cm.y - mainCenterOfMass.y) < 0.02f)
                    {
                        cm.x = -mainCenterOfMass.x;
                        cm.y = mainCenterOfMass.y;
                    }
                    rb.centerOfMass = rb.transform.InverseTransformPoint(transform.TransformPoint(cm));
                }
            }
        }

        private void Update()
        {
            SetLandingGear(releaseLandingGear);
        }

        private void SetLandingGear(bool release)
        {
            foreach (LandingLeg landingLeg in _landingLegs)
                landingLeg.release = release;
        }

        public void DropFlares()
        {
            SpawnFlare(rightFlareSpawnPos, rightFlareVelocity);
            SpawnFlare(new Vector3(-rightFlareSpawnPos.x, rightFlareSpawnPos.y),
                new Vector2(-rightFlareVelocity.x, rightFlareVelocity.y));
        }

        void SpawnFlare(Vector2 localPos, Vector2 localVelocity)
        {
            Vector3 worldPos = mainRigidbody.transform.TransformPoint(localPos);
            Vector2 worldVel = (Vector2)mainRigidbody.transform.TransformDirection(localVelocity) + mainRigidbody.velocity;
            Quaternion worldRot = Quaternion.Euler(0, 0,
                flaresPrefab.transform.rotation.eulerAngles.z*Mathf.Sign(localVelocity.x) +
                mainRigidbody.rotation);
            GameObject flareObj = Instantiate(flaresPrefab, worldPos, worldRot);
            Rigidbody2D flareRigidbody = flareObj.GetComponent<Rigidbody2D>();
            flareRigidbody.velocity = worldVel;
            flareRigidbody.angularVelocity = flareAngularVelocity*Mathf.Sign(localVelocity.x);
        }
    }
}