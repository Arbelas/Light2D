using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Light2D.Examples {
    public class SimpleExamplesController : MonoBehaviour {
        public LightSprite[] coloredLights = new LightSprite[0];
        public GameObject[] examples = new GameObject[0];
        public Color[] lightColors = { Color.white };
        private int _currExampleIndex;
        private int _currColorIndex;

        void Start() {
            UpdateExample();
            UpdateColors();
        }

        void Update() {
            if (Input.GetMouseButtonUp(0)) {
                _currExampleIndex++;
                if (_currExampleIndex >= examples.Length)
                    _currExampleIndex = 0;

                UpdateExample();
            }

            if (Input.GetMouseButtonUp(1)) {
                _currColorIndex++;
                if (_currColorIndex >= lightColors.Length)
                    _currColorIndex = 0;

                UpdateColors();
            }
        }

        void UpdateColors() {
            Color color = lightColors.Length == 0 ? Color.white : lightColors[_currColorIndex];
            foreach (LightSprite t in coloredLights) t.color = color.WithAlpha(t.color.a);
        }

        void UpdateExample() {
            for (int i = 0; i < examples.Length; i++)
                examples[i].SetActive(i == _currExampleIndex);
            LightingSystem.Instance.LoopAmbientLight(20);
        }
    }
}
