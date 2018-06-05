using UnityEngine;
using System.Collections;
using UnityEditor;

namespace Light2D.Examples
{
    [CustomEditor(typeof (LandingLeg))]
    [CanEditMultipleObjects]
    public class LandingLegEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            if (targets.Length == 1)
            {
                LandingLeg myScript = (LandingLeg) target;
                HingeJoint2D joint = myScript.autoRotator == null ? null : myScript.autoRotator.joint;
                Rigidbody2D connBody = joint == null ? null : joint.connectedBody;

                GUI.enabled = false;
                EditorGUILayout.ObjectField("Connected body", connBody, typeof (Rigidbody2D), true);
                GUI.enabled = true;
            }

            DrawDefaultInspector();
        }
    }
}