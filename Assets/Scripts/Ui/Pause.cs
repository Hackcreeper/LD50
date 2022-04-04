using Entities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Ui
{
    public class Pause : MonoBehaviour
    {
        #region PUBLIC_VARS

        public static Pause Instance { get; private set; }
        
        public TextMeshProUGUI scoreLabel;
        public float pauseFadeTime = .8f;
        public float scoreScaleTime = .5f;
        public TextMeshProUGUI pauseLabel;
        public TextMeshProUGUI pauseScoreLabel;
        public Player player;
        public Button continueGameButton;

        #endregion
        
        #region PRIVATE_VARS
        
        private Image _image;
        private bool _paused;
        
        #endregion
        
        #region UNITY_FUNCTIONS

        private void Awake()
        {
            Instance = this;
            _image = GetComponent<Image>();
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            if (!hasFocus)
            {
                PauseGame();
            }
        }

        #endregion
        
        #region PUBLIC_METHODS

        public void PauseGame()
        {
            Time.timeScale = 0f;
            _paused = true;
            
            pauseScoreLabel.text = $"Score: {player.GetScore()}";

            LeanTween.cancel(scoreLabel.gameObject);

            LeanTween.scale(scoreLabel.gameObject, Vector3.zero, scoreScaleTime)
                .setIgnoreTimeScale(true)
                .setEaseSpring();
            
            ShowPause();
        }
        
        public void ContinueGame()
        {
            HidePause();
        }

        public bool IsPaused() => _paused;
        
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
            }, 0f, 1f / 255f * 160f, pauseFadeTime).setIgnoreTimeScale(true);

            LeanTween.moveY(pauseLabel.rectTransform, -100, pauseFadeTime)
                .setIgnoreTimeScale(true)
                .setEaseInOutQuint();

            LeanTween.scale(pauseScoreLabel.rectTransform, Vector3.one, pauseFadeTime / 1.5f)
                .setOvershoot(1.3f)
                .setIgnoreTimeScale(true)
                .setEaseSpring();

            LeanTween.scaleX(continueGameButton.gameObject, 1f, pauseFadeTime / 1.5f)
                .setOvershoot(1.3f)
                .setIgnoreTimeScale(true)
                .setEaseSpring();
        }
        
        private void HidePause()
        {
            LeanTween.value(gameObject, value =>
            {
                _image.color = new Color(
                    _image.color.r,
                    _image.color.g,
                    _image.color.b,
                    value
                );
            }, 1f / 255f * 160f, 0f, pauseFadeTime).setIgnoreTimeScale(true);

            LeanTween.moveY(pauseLabel.rectTransform, 50, pauseFadeTime)
                .setIgnoreTimeScale(true)
                .setEaseInOutQuint()
                .setOnComplete(() =>
                {
                    Time.timeScale = 1f;
                    _paused = false;
                    
                    LeanTween.scale(scoreLabel.gameObject, Vector3.one, scoreScaleTime)
                        .setIgnoreTimeScale(true)
                        .setEaseSpring();
                });

            LeanTween.scale(pauseScoreLabel.rectTransform, Vector3.zero, pauseFadeTime / 1.5f)
                .setIgnoreTimeScale(true)
                .setEaseSpring();

            LeanTween.scaleX(continueGameButton.gameObject, 0f, pauseFadeTime / 1.5f)
                .setIgnoreTimeScale(true)
                .setEaseSpring();
        }

        #endregion
    }
}