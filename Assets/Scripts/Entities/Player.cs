using GameFlow;
using Pickups;
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
        public Transform groundCheckLeftStartPosition;
        public Transform groundCheckRightStartPosition;

        public float groundCheckDistance = 0.1f;
        public LayerMask groundLayer;
        public TextMeshProUGUI scoreLabel;
        public GameOver gameOver;
        public FlowCamera flowCamera;
        public TextMeshPro tooltipLabel;

        public float tooltipFadeInSpeed = 0.4f;
        public float tooltipFadeOutSpeed = 1.5f;
        public float tooltipFadeDelay = 2f;

        public Transform model;
        public Animator animator;

        #endregion

        #region PRIVATE_VARS

        private Rigidbody2D _rigidbody2D;

        private float _forceCooldown = .5f;
        private float _xVelocity;
        private bool _started;
        private bool _grounded;
        private int _bonusScore;
        private bool _dead;
        private bool _introStarted;
        private int _minScore;
        private bool _scoreVisible;
        private float _lastJumpForce;

        private static readonly int JumpingAction = Animator.StringToHash("jumping");
        private static readonly int YVelocityAction = Animator.StringToHash("yVelocity");
        private static readonly int LandingAction = Animator.StringToHash("landing");
        private static readonly int StartingAction = Animator.StringToHash("starting");

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

            var platform = collision.gameObject.GetComponent<Platform>();
            if (!platform)
            {
                Jump(jumpForce);
                return;
            }

            animator.SetBool(LandingAction, true);
            platform.OnPlayerEnter(this);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            var pickup = collision.GetComponent<Pickup>();
            if (!pickup)
            {
                return;
            }

            pickup.OnPlayerCollect(this);
        }

        private void Update()
        {
            if (_dead)
            {
                return;
            }

            _forceCooldown -= Time.deltaTime;
            _rigidbody2D.velocity = new Vector2(_xVelocity * moveSpeed, _rigidbody2D.velocity.y);

            UpdateAnimatorVelocity();
            UpdateScoreLabel();
            UpdateModelScale();
            CheckForGrounded();
            CheckForDead();
        }
        
        #endregion

        #region PUBLIC_METHODS

        public int GetScore()
        {
            var scoreByDistance = Mathf.FloorToInt(transform.position.y / 5f);
            var totalScore = Mathf.Max(_minScore, scoreByDistance + _bonusScore);

            _minScore = totalScore;

            return totalScore;
        }

        public bool HasStarted() => _started;

        public void Jump(float force)
        {
            animator.SetBool(JumpingAction, true);

            _lastJumpForce = force;
            _forceCooldown = 1.4f;
        }

        public void AddBonusScore(int amount)
        {
            _bonusScore += amount;
        }

        public void ShowTooltip(string text)
        {
            tooltipLabel.text = text;
            var tooltipGo = tooltipLabel.gameObject;

            tooltipGo.SetActive(true);

            LeanTween.cancel(tooltipGo);

            LeanTween.value(tooltipGo, color => { tooltipLabel.color = color; },
                new Color(1, 1, 1, 0),
                Color.white,
                tooltipFadeInSpeed
            );

            LeanTween.moveLocalY(tooltipGo, 1.2f, tooltipFadeInSpeed)
                .setOnComplete(() =>
                {
                    LeanTween.moveLocalY(tooltipGo, 1f, tooltipFadeOutSpeed)
                        .setDelay(tooltipFadeDelay);
                });

            LeanTween.value(tooltipGo, color => { tooltipLabel.color = color; },
                    Color.white,
                    new Color(1, 1, 1, 0), tooltipFadeOutSpeed)
                .setDelay(tooltipFadeInSpeed + tooltipFadeDelay)
                .setOnComplete(() =>
                {
                    tooltipGo.SetActive(false);
                    tooltipLabel.transform.localPosition = new Vector3(0, 1f, 0f);
                });
        }
        
        public void StartJump()
        {
            _rigidbody2D.AddForce(new Vector2(0, _lastJumpForce), ForceMode2D.Impulse);
            _lastJumpForce = 0f;
        }

        public void FinishedStandingUp()
        {
            Jump(jumpForce);
            _started = true;
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
            if (FireGroundRaycast(groundCheckStartPosition.position))
            {
                _grounded = true;
                return;
            }

            if (FireGroundRaycast(groundCheckLeftStartPosition.position))
            {
                _grounded = true;
                return;
            }

            if (FireGroundRaycast(groundCheckRightStartPosition.position))
            {
                _grounded = true;
                return;
            }

            _grounded = false;
        }

        private bool FireGroundRaycast(Vector3 startPosition)
        {
            return Physics2D.Raycast(
                startPosition,
                -transform.up,
                groundCheckDistance,
                groundLayer
            );
        }

        private void UpdateModelScale()
        {
            if (_xVelocity == 0)
            {
                return;
            }

            var modelLocalScale = model.localScale;
            var modelLocalPosition = model.localPosition;

            var xScale = Mathf.Abs(modelLocalScale.x);
            var xPos = Mathf.Abs(modelLocalPosition.x);

            model.localScale = new Vector3(
                _xVelocity > 0 ? -xScale : xScale,
                modelLocalScale.y,
                modelLocalScale.z
            );

            model.localPosition = new Vector3(
                _xVelocity > 0 ? xPos : -xPos,
                modelLocalPosition.y,
                modelLocalPosition.z
            );
        }

        private void UpdateScoreLabel()
        {
            if (GetScore() <= 0)
            {
                return;
            }

            scoreLabel.text = GetScore().ToString();

            if (_scoreVisible)
            {
                return;
            }

            LeanTween.moveY(scoreLabel.rectTransform, -48, 0.35f);
            _scoreVisible = true;
        }

        private void UpdateAnimatorVelocity()
        {
            animator.SetFloat(YVelocityAction, _rigidbody2D.velocity.y);
        }

        #endregion

        #region INPUT_MOVEMENTS

        public void OnMove(InputAction.CallbackContext context)
        {
            if (!_started)
            {
                return;
            }

            _xVelocity = context.ReadValue<Vector2>().x;
        }

        public void OnStart(InputAction.CallbackContext context)
        {
            if (!context.started)
            {
                return;
            }

            if (_started || _introStarted)
            {
                if (Application.isEditor)
                {
                    ShowTooltip("You sneaky cheater!");
                    Jump(jumpForce * 5);
                }

                return;
            }

            _introStarted = true;
            animator.SetBool(StartingAction, true);

            flowCamera.OnStart(() => {});
            
            // flowCamera.OnStart(() =>
            // {
            //     Jump(jumpForce);
            //     _started = true;
            // });
        }

        public void OnPause(InputAction.CallbackContext context)
        {
            if (!context.started)
            {
                return;
            }

            if (Pause.Instance.IsPaused())
            {
                Pause.Instance.ContinueGame();
                return;
            }
            
            Pause.Instance.PauseGame();
        }

        public void OnCheat2(InputAction.CallbackContext context)
        {
            if (!context.started || !Application.isEditor)
            {
                return;
            }

            transform.position = new Vector3(
                transform.position.x,
                310,
                transform.position.z
            );
        }
        
        #endregion
    }
}