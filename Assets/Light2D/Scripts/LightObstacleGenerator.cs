﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Light2D {
    /// <summary>
    /// That class is generating obstacles for object it attached to.
    /// Obect must have MeshRenderer, SpriteRenderer or CustomSprite script from which texture for obstacle will be grabbed.
    /// For rendering obstacle of SpriteRenderer and CustomSprite LightObstacleSprite with material "Material" (material with dual color shader by default) will be used.
    /// For objects with MeshRenderer "Material" property is ignored. MeshRenderer.sharedMaterial is used instead.
    /// </summary>
    [ExecuteInEditMode]
    public class LightObstacleGenerator : MonoBehaviour {
        /// <summary>
        /// Vertex color.
        /// </summary>
        public Color multiplicativeColor = new Color(0, 0, 0, 1);

        /// <summary>
        /// AdditiveColor that will be used for obstacle when SpriteRenderer or CustomSprite scripts is attached.
        /// Only DualColor shader supports additive color.
        /// </summary>
        public Color additiveColor;

        /// <summary>
        /// Material that will be used for obstacle when SpriteRenderer or CustomSprite scripts is attached.
        /// </summary>
        public Material material;

        public float lightObstacleScale = 1;

        private void Start() {
#if UNITY_EDITOR
            if (material == null) {
                material = (Material)UnityEditor.AssetDatabase.LoadAssetAtPath("Assets/Light2D/Materials/DualColor.mat", typeof(Material));
            }
#endif

            if (!Application.isPlaying)
                return;

            GameObject obstacleObj = new GameObject(gameObject.name + " Light Obstacle");

            obstacleObj.transform.parent = gameObject.transform;
            obstacleObj.transform.localPosition = Vector3.zero;
            obstacleObj.transform.localRotation = Quaternion.identity;
            obstacleObj.transform.localScale = Vector3.one * lightObstacleScale;
            if (LightingSystem.Instance != null)
                obstacleObj.layer = LightingSystem.Instance.lightObstaclesLayer;

            if (GetComponent<SpriteRenderer>() != null || GetComponent<CustomSprite>() != null) {
                LightObstacleSprite obstacleSprite = obstacleObj.AddComponent<LightObstacleSprite>();
                obstacleSprite.color = multiplicativeColor;
                obstacleSprite.additiveColor = additiveColor;
                obstacleSprite.material = material;
            } else {
                LightObstacleMesh obstacleMesh = obstacleObj.AddComponent<LightObstacleMesh>();
                obstacleMesh.multiplicativeColor = multiplicativeColor;
                obstacleMesh.additiveColor = additiveColor;
                obstacleMesh.material = material;
            }

            Destroy(this);
        }
    }
}
