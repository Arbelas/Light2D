using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Light2D.Examples
{
    public class ColorTweener : MonoBehaviour
    {
        public float tweenInterval = 2.5f;
        public float colorMul = 2;
        private SpriteRenderer _spriteRenderer;
        private float _timer;
        private Color _targetColor;
        private Color _startColor;

        void OnEnable()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        void Update()
        {
            if (_spriteRenderer == null)
                return;

            _timer -= Time.deltaTime;

            if (_timer <= 0)
            {
                _timer = tweenInterval;
                _startColor = _spriteRenderer.color;
                _targetColor = new Vector4(
                    Mathf.Clamp01(Random.value*colorMul), Mathf.Clamp01(Random.value*colorMul),
                    Mathf.Clamp01(Random.value*colorMul), Mathf.Clamp01(Random.value*colorMul));
            }

            _spriteRenderer.color = Color.Lerp(_startColor, _targetColor, 1 - _timer/tweenInterval);
        }
    }
}
