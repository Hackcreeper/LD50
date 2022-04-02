using System;
using UnityEngine;

namespace Platforms
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class MovingPlatform : Platform
    {
        #region PUBLIC_VARS

        public float speed = 3f;
        
        #endregion
        
        #region PRIVATE_VARS

        private bool _movingRight = true;
        private BoxCollider2D _collider;
        
        #endregion
        
        #region UNITY_FUNCTIONS

        private void Awake()
        {
            _collider = GetComponent<BoxCollider2D>();
        }

        private void Update()
        {
            if (transform.position.x >= 4 - _collider.size.x / 2f)
            {
                _movingRight = false;
            } else if (transform.position.x <= -4 + _collider.size.x / 2f)
            {
                _movingRight = true;
            }
            
            transform.Translate((_movingRight ? speed : -speed) * Time.deltaTime, 0, 0);
        }
        
        #endregion
    }
}