using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace Light2D.Examples
{
    public class InputManager : MonoBehaviour
    {
        public Spacecraft controlledSpacecraft;
        public GameObject touchControls;
        public ButtonHelper upButton, downButton, leftButton, rightButton;

        private IEnumerator Start()
        {
            touchControls.SetActive(Input.touchSupported);

            controlledSpacecraft.mainRigidbody.isKinematic = true;
            yield return new WaitForSeconds(1);
            controlledSpacecraft.mainRigidbody.isKinematic = false;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F1))
                Time.timeScale = Time.timeScale > 0.5 ? 0 : 1;

            if (Input.GetKeyDown(KeyCode.R))
                Application.LoadLevel(0);

            if(Input.GetKeyDown(KeyCode.C))
                controlledSpacecraft.DropFlares();

            controlledSpacecraft.bottomLeftEngine.forcePercent = 0;
            controlledSpacecraft.bottomRightEngine.forcePercent = 0;
            controlledSpacecraft.sideRightEngine.forcePercent = 0;
            controlledSpacecraft.sideLeftEngine.forcePercent = 0;

            Vector2 moveDir = Vector2.zero;
            if (Input.GetKey(KeyCode.UpArrow) || upButton.IsPressed) moveDir += new Vector2(0, 1);
            if (Input.GetKey(KeyCode.DownArrow) || downButton.IsPressed) moveDir += new Vector2(0, -1);
            if (Input.GetKey(KeyCode.RightArrow) || rightButton.IsPressed) moveDir += new Vector2(1, 0);
            if (Input.GetKey(KeyCode.LeftArrow) || leftButton.IsPressed) moveDir += new Vector2(-1, 0);

            controlledSpacecraft.bottomLeftEngine.forcePercent = moveDir.y*2f + moveDir.x;
            controlledSpacecraft.bottomRightEngine.forcePercent = moveDir.y*2f - moveDir.x;
            controlledSpacecraft.sideLeftEngine.forcePercent = moveDir.x;
            controlledSpacecraft.sideRightEngine.forcePercent = -moveDir.x;
            controlledSpacecraft.reverseLeftEngine.forcePercent = -moveDir.y - moveDir.x*2f;
            controlledSpacecraft.reverseRightEngine.forcePercent = -moveDir.y + moveDir.x*2f;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                controlledSpacecraft.releaseLandingGear ^= true;
            }
        }

        public void LegsClick()
        {
            controlledSpacecraft.releaseLandingGear ^= true;
        }

        public void FlareClick()
        {
            controlledSpacecraft.DropFlares();
        }

        public void Restart()
        {
            Application.LoadLevel(0);
        }
    }
}