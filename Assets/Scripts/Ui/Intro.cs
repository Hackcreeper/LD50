using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Ui
{
    public class Intro : MonoBehaviour
    {
        #region PUBLIC_VARS
        
        public Image logo;
        public TextMeshProUGUI creditsLabel;

        #endregion
        
        #region UNITY_FUNCTIONS

        private void Start()
        {
            LeanTween.scale(logo.rectTransform, Vector3.one * 1.05f, 1.5f)
                .setLoopType(LeanTweenType.pingPong);
        }
        
        #endregion
    }
}