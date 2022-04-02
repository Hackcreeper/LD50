using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Entities
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Player : MonoBehaviour
    {
        public float jumpForce = 10f;
        public float moveSpeed = 5f;
        public Transform groundCheckStartPosition;
        public float groundCheckDistance = 0.1f;
        public LayerMask groundLayer;

        private float _forceCooldown = .5f;
        private Rigidbody2D _rigidbody2D;
        private float _xVelocity;
        private bool _started;
        private bool _grounded;

        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            if (_forceCooldown > 0f || !_grounded)
            {
                return;
            }

            Jump();
        }

        private void Jump()
        {
            _rigidbody2D.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            _forceCooldown = 0.8f;
        }

        private void Update()
        {
            _forceCooldown -= Time.deltaTime;
            _rigidbody2D.velocity = new Vector2(_xVelocity * moveSpeed, _rigidbody2D.velocity.y);

            _grounded = Physics2D.Raycast(
                groundCheckStartPosition.position,
                -transform.up,
                groundCheckDistance,
                groundLayer
            );
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            _xVelocity = context.ReadValue<Vector2>().x;
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (!context.started || _started)
            {
                return;
            }

            Jump();
            _started = true;
        }

        private void OnDrawGizmos()
        {
            if (!groundCheckStartPosition)
            {
                return;
            }

            Gizmos.color = Color.red;
            Gizmos.DrawRay(groundCheckStartPosition.position, -transform.up * groundCheckDistance);
        }
    }
}