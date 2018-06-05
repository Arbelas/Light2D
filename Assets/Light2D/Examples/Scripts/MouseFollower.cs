using UnityEngine;
using System.Collections;

namespace Light2D.Examples
{
    public class MouseFollower : MonoBehaviour
    {
        public bool rightClickRotation = false;
        private Vector2 _pressPos;

        private void LateUpdate()
        {
            if (Input.GetMouseButtonDown(0))
                _pressPos = Util.GetMousePosInUnits();

            if (Input.GetMouseButton(0) && rightClickRotation)
            {
                Vector2 shift = Util.GetMousePosInUnits() - _pressPos;
                if (shift.sqrMagnitude > 0.1f*0.1f)
                {
                    float angle = shift.AngleZ();
                    transform.rotation = Quaternion.Euler(0, 0, angle);
                }
            }
            else
            {
                Vector3 pos = Util.GetMousePosInUnits();
                pos.z = transform.position.z;
                transform.position = pos;
            }
        }
    }
}
