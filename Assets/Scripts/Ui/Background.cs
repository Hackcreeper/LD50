using GameFlow;
using UnityEngine;
using UnityEngine.UI;

namespace Ui
{
    public class Background : MonoBehaviour
    {
        #region PUBLIC_VARS
        
        public Image image;
        public FlowCamera flowCamera;
        public float parallaxSpeed = 2; // how many pixels to move for each unit moved in the camera?
        
        #endregion
        
        #region PRIVATE_VARS

        private float _startY;
        
        #endregion

        #region PUBLIC_METHODS
        
        public void OnStart()
        {
            LeanTween.scale(
                GetComponent<RectTransform>(),
                Vector3.one,
                flowCamera.introAnimationTime
            );
        }
        
        #endregion
    }
}