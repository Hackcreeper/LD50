using GameFlow;
using UnityEngine;

namespace Entities
{
    [RequireComponent((typeof(SpriteRenderer)))]
    public class Cloud : MonoBehaviour
    {
        #region PUBLIC_VARS
        
        public float parallaxSpeed = 2f;
        public FlowCamera flowCamera;
        public CloudColor[] colors;
        
        #endregion
        
        #region PRIVATE_VARS
        
        private float _cameraStartY;
        private float _lastFrame;
        private SpriteRenderer _sprite;
        
        #endregion
        
        #region UNITY_METHODS

        private void Awake()
        {
            _sprite = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            _cameraStartY = flowCamera.transform.position.y;
            _lastFrame = 0;

            foreach (var color in colors)
            {
                if (color.minSpeed > parallaxSpeed)
                {
                    break;
                }

                _sprite.color = color.color;
            }
        }

        private void Update()
        {
            var valueToMove = CalculatePositionY();

            var trans = transform;
            var localPosition = trans.localPosition;
            trans.localPosition = new Vector3(
                localPosition.x,
                localPosition.y - valueToMove,
                localPosition.z
            );
        }

        #endregion
        
        #region PUBLIC_METHODS

        public SpriteRenderer GetSprite() => _sprite;
        
        #endregion
        
        #region PRIVATE_METHODS
        
        private float CalculatePositionY()
        {
            // Get the current player position
            var playerPositionY = flowCamera.transform.position.y;

            // And the player start position (maybe set in start()?)
            // Done √
            
            // Get the difference, and multiply it with the parallax Speed variable
            var targetY = (playerPositionY - _cameraStartY) * parallaxSpeed;
            
            // Now take the difference between lastFrame (initial = playerY * multiply) and the new value
            var difference = targetY - _lastFrame;

            _lastFrame = targetY;

            return difference;
        }

        #endregion
    }
}