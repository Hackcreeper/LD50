using GameFlow;
using Platforms;
using TMPro;
using Ui;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Entities
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Player : MonoBehaviour
    {
        #region PUBLIC_VARS
        
        public float jumpForce = 10f;
        public float moveSpeed = 5f;
        public Transform groundCheckStartPosition;
        public float groundCheckDistance = 0.1f;
        public LayerMask groundLayer;
        public TextMeshProUGUI scoreLabel;
        public GameOver gameOver;
        public FlowCamera flowCamera;
        
        #endregion

        #region PRIVATE_VARS
        
        private Rigidbody2D _rigidbody2D;
        
        private float _forceCooldown = .5f;
        private float _xVelocity;
        private bool _started;
        private bool _grounded;
        private int _score;
        private bool _dead;
        private bool _introStarted;
        
        #endregion

        #region UNITY_FUNCTIONS
        
        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            if (_forceCooldown > 0f || !_grounded || !_started || _dead || _rigidbody2D.velocity.y > 0.1f)
            {
                return;
            }

            _score++;
            
            var platform = collision.gameObject.GetComponent<Platform>();
            if (!platform)
            {
                Jump(jumpForce);
                return;
            }
            
            platform.OnPlayerEnter(this);
        }

        private void Update()
        {
            if (_dead)
            {
                return;
            }
            
            _forceCooldown -= Time.deltaTime;
            _rigidbody2D.velocity = new Vector2(_xVelocity * moveSpeed, _rigidbody2D.velocity.y);

            CheckForGrounded();
            CheckForDead();
        }
        
        #endregion
        
        #region PUBLIC_METHODS

        public int GetScore() => _score;

        public bool HasStarted() => _started;
        
        public void Jump(float force)
        {
            _rigidbody2D.AddForce(new Vector2(0, force), ForceMode2D.Impulse);
            _forceCooldown = 0.8f;

            if (_score > 0)
            {
                scoreLabel.text = _score.ToString();
            }
        }

        #endregion
        
        #region PRIVATE_METHODS
        
        private void CheckForDead()
        {
            var viewPort = flowCamera.GetComponent<Camera>().WorldToViewportPoint(transform.position);
            if (viewPort.y > 0)
            {
                return;
            }

            _dead = true;
            gameOver.OnPlayerDeath();
        }

        private void CheckForGrounded()
        {
            _grounded = Physics2D.Raycast(
                groundCheckStartPosition.position,
                -transform.up,
                groundCheckDistance,
                groundLayer
            );
        }

        #endregion
        
        #region INPUT_MOVEMENTS
        
        public void OnMove(InputAction.CallbackContext context)
        {
            _xVelocity = context.ReadValue<Vector2>().x;
        }

        public void OnStart(InputAction.CallbackContext context)
        {
            if (!context.started || _started || _introStarted)
            {
                return;
            }

            _introStarted = true;

            flowCamera.OnStart(() =>
            {
                Jump(jumpForce);
                _started = true;
            });
        }

        #endregion
    }
}