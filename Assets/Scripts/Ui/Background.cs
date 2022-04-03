using GameFlow;
using UnityEngine;

namespace Ui
{
    public class Background : MonoBehaviour
    {
        #region PUBLIC_VARS
        
        public FlowCamera flowCamera;
        
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