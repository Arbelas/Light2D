﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Light2D {
    /// <summary>
    /// Sprite with dual color support. Grabs sprite from GameSpriteRenderer field.
    /// </summary>
    [ExecuteInEditMode]
    public class LightObstacleSprite : CustomSprite {
        /// <summary>
        /// Renderer from which sprite will be used.
        /// </summary>
        public Renderer gameSpriteRenderer;

        /// <summary>
        /// Color is packed in mesh UV1.
        /// </summary>
        public Color additiveColor;
        private Color _oldSecondaryColor;
        private Renderer _oldGameSpriteRenderer;
        private SpriteRenderer _oldUnitySprite;
        private CustomSprite _oldCustomSprite;

        protected override void OnEnable() {
#if UNITY_EDITOR
            if (material == null) {
                material = (Material)AssetDatabase.LoadAssetAtPath("Assets/Light2D/Materials/DualColor.mat", typeof(Material));
            }
#endif

            base.OnEnable();

            if (gameSpriteRenderer == null && transform.parent != null)
                gameSpriteRenderer = transform.parent.gameObject.GetComponent<Renderer>();

            gameObject.layer = LightingSystem.Instance.lightObstaclesLayer;

            UpdateMeshData(true);
        }

        private void UpdateSecondaryColor() {
            Vector2 uv1 = new Vector2(
                Util.DecodeFloatRgba((Vector4)additiveColor),
                Util.DecodeFloatRgba(new Vector4(additiveColor.a, 0, 0)));
            for (int i = 0; i < this.uv1.Length; i++) {
                this.uv1[i] = uv1;
            }
        }

        protected override void UpdateMeshData(bool forceUpdate = false) {
            if (meshRenderer == null || meshFilter == null || IsPartOfStaticBatch)
                return;

            if (gameSpriteRenderer != null && (gameSpriteRenderer != _oldGameSpriteRenderer || forceUpdate ||
                (_oldUnitySprite != null && _oldUnitySprite.sprite != null && _oldUnitySprite.sprite != sprite) ||
                (_oldCustomSprite != null && _oldCustomSprite.sprite != null && _oldCustomSprite.sprite != sprite))) {
                _oldGameSpriteRenderer = gameSpriteRenderer;

                _oldCustomSprite = gameSpriteRenderer.GetComponent<CustomSprite>();
                if (_oldCustomSprite != null) {
                    sprite = _oldCustomSprite.sprite;
                } else {
                    _oldUnitySprite = gameSpriteRenderer.GetComponent<SpriteRenderer>();
                    if (_oldUnitySprite != null)
                        sprite = _oldUnitySprite.sprite;
                }

                material.EnableKeyword("NORMAL_TEXCOORD");
            }

            if (_oldSecondaryColor != additiveColor || forceUpdate) {
                UpdateSecondaryColor();
                isMeshDirty = true;
                _oldSecondaryColor = additiveColor;
            }

            base.UpdateMeshData(forceUpdate);
        }
    }
}

