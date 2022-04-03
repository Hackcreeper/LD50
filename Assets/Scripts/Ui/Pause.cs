using Entities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Ui
{
    public class Pause : MonoBehaviour
    {
        #region PUBLIC_VARS

        public TextMeshProUGUI scoreLabel;
        public RectTransform pauseScreen;
        public float pauseFadeTime = .8f;
        public float scoreScaleTime = .5f;
        public TextMeshProUGUI pauseLabel;
        public TextMeshProUGUI pauseScoreLabel;
        public Player player;
        public Button continueGameButton;

        #endregion
        
        #region PRIVATE_VARS
        
        private Image _image;
        
        #endregion
        
        #region UNITY_FUNCTIONS

        private void Awake()
        {
            _image = GetComponent<Image>();
        }

        #endregion
        
        #region PUBLIC_METHODS

        public void PauseGame()
        {
            pauseScoreLabel.text = $"Score: {player.GetScore()}";

            LeanTween.cancel(scoreLabel.gameObject);

            LeanTween.scale(scoreLabel.gameObject, Vector3.zero, scoreScaleTime)
                .setEaseSpring()
                .setOnComplete(ShowPause);
        }
        
        public void ContinueGame()
        {
            // todo
        }

        #endregion
        
        #region PRIVATE_METHODS

        private void ShowPause()
        {
            LeanTween.value(gameObject, value =>
            {
                _image.color = new Color(
                    _image.color.r,
                    _image.color.g,
                    _image.color.b,
                    value
                );
            }, 0f, 1f / 255f * 160f, pauseFadeTime);

            LeanTween.moveY(pauseLabel.rectTransform, -100, pauseFadeTime)
                .setEaseInOutQuint();

            LeanTween.scale(pauseScoreLabel.rectTransform, Vector3.one, pauseFadeTime / 1.5f)
                .setDelay(pauseFadeTime / 2f)
                .setOvershoot(1.3f)
                .setEaseSpring();

            LeanTween.scaleX(continueGameButton.gameObject, 1f, pauseFadeTime / 1.5f)
                .setDelay(pauseFadeTime / 2f)
                .setOvershoot(1.3f)
                .setEaseSpring();
        }
        
        #endregion
    }
}