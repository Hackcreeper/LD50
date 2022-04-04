using GameFlow;
using UnityEngine;

namespace Entities
{
    public class Cloud : MonoBehaviour
    {
        #region PUBLIC_VARS
        
        public float parallaxSpeed = 2f;
        public FlowCamera flowCamera;
        
        #endregion
        
        #region PRIVATE_VARS
        
        private float _cameraStartY;
        private float _lastFrame;
        
        #endregion
        
        #region UNITY_METHODS

        private void Start()
        {
            _cameraStartY = flowCamera.transform.position.y;
            _lastFrame = 0;
        }

        private void Update()
        {
            var valueToMove = CalculatePositionY();

            transform.localPosition = new Vector3(
                transform.localPosition.x,
                transform.localPosition.y - valueToMove
            );
        }

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