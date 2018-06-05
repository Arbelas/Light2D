using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Light2D.Examples
{
    public class Flare : MonoBehaviour
    {
        public float lifetime;
        public LightSprite light;
        public float alphaGrowTime = 0.5f;
        private float _lifetimeElapsed = 0;
        private Color _startColor;

        void Start()
        {
            _startColor = light.color;
            light.color = _startColor.WithAlpha(0);
        }

        void Update()
        {
            _lifetimeElapsed += Time.deltaTime;

            

            if (_lifetimeElapsed > lifetime)
            {
                _lifetimeElapsed = lifetime;
                Destroy(gameObject);
            }


            float alpha = Mathf.Lerp(0, _startColor.a, Mathf.Min(_lifetimeElapsed, alphaGrowTime)/alphaGrowTime);
            light.color = Color.Lerp(_startColor.WithAlpha(alpha), _startColor.WithAlpha(0), _lifetimeElapsed/lifetime);
        }
    }
}
