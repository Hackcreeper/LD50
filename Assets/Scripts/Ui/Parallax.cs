using GameFlow;
using UnityEngine;

namespace Ui
{
    [RequireComponent(typeof(RectTransform))]
    public class Parallax : MonoBehaviour
    {
        #region PUBLIC_VARS

        // How fast should the images move, for each unit the player moves
        public float parallaxSpeed = 3f;
        public FlowCamera flowCamera;

        #endregion
        
        #region PRIVATE_VARS

        private RectTransform _rectTransform;
        private float _cameraStartY;
        private float _lastFrame;
        
        #endregion

        #region UNITY_METHODS

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        private void Start()
        {
            _cameraStartY = flowCamera.transform.position.y;
            _lastFrame = 0;
        }

        private void Update()
        {
            var valueToMove = CalculatePositionY();

            foreach (var child in GetComponentsInChildren<RectTransform>())
            {
                child.anchoredPosition = new Vector2(
                    child.anchoredPosition.x,
                    child.anchoredPosition.y - valueToMove
                );
            }
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