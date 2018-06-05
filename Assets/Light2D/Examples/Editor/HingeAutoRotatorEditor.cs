using UnityEngine;
using System.Collections;
using UnityEditor;

namespace Light2D.Examples
{
    [CustomEditor(typeof (HingeAutoRotator))]
    [CanEditMultipleObjects]
    public class HingeAutoRotatorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            if (targets.Length == 1)
            {
                HingeAutoRotator myScript = (HingeAutoRotator) target;
                Rigidbody2D connBody = myScript.joint == null ? null : myScript.joint.connectedBody;

                GUI.enabled = false;
                EditorGUILayout.ObjectField("Connected body", connBody, typeof (Rigidbody2D), true);
                GUI.enabled = true;
            }

            DrawDefaultInspector();
        }
    }
}