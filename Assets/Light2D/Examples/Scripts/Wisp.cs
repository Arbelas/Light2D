using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Light2D.Examples
{
    public class Wisp : MonoBehaviour
    {
        public float force;
        public GameObject lineLight;

        void Update()
        {
            Vector2 moveVec = Vector2.zero;

            if (Input.GetKey(KeyCode.UpArrow)) moveVec += new Vector2(0, 1);
            if (Input.GetKey(KeyCode.DownArrow)) moveVec += new Vector2(0, -1);
            if (Input.GetKey(KeyCode.RightArrow)) moveVec += new Vector2(1, 0);
            if (Input.GetKey(KeyCode.LeftArrow)) moveVec += new Vector2(-1, 0);

            if (moveVec.sqrMagnitude > 0.01f*0.01f)
            {
                Vector2 force = moveVec.normalized*this.force;
                GetComponent<Rigidbody2D>().AddForce(force, ForceMode2D.Force);
            }

            float lightAngle = (Util.GetMousePosInUnits() - (Vector2) transform.position).AngleZ();
            lineLight.transform.rotation = Quaternion.Euler(0, 0, lightAngle);
        }
    }
}
