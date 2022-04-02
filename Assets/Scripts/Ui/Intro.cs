using TMPro;
using UnityEngine;

namespace Ui
{
    public class Intro : MonoBehaviour
    {
        #region PUBLIC_VARS
        
        public TextMeshProUGUI gameNameLabel;
        public TextMeshProUGUI creditsLabel;

        #endregion
        
        #region UNITY_FUNCTIONS

        private void Start()
        {
            LeanTween.scale(gameNameLabel.rectTransform, Vector3.one * 1.05f, 1.5f)
                .setLoopType(LeanTweenType.pingPong);
        }
        
        #endregion
    }
}