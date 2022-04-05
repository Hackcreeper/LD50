using Entities;
using GameJolt.API;
using GameJolt.UI;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Ui
{
    [RequireComponent(typeof(AudioSource))]
    public class GameOver : MonoBehaviour
    {
        #region PUBLIC_VARS

        public TextMeshProUGUI scoreLabel;
        public RectTransform gameOverScreen;
        public float gameOverFadeTime = .8f;
        public float scoreScaleTime = .5f;
        public TextMeshProUGUI gameOverLabel;
        public TextMeshProUGUI gameOverScoreLabel;
        public Player player;
        public TMP_InputField nameInput;
        public Button submitScoreButton;
        public Button playAgainButton;
        public AudioClip[] gameOverSounds;

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

        public void OnPlayerDeath()
        {
            gameOverScoreLabel.text = $"Score: {player.GetScore()}";

            LeanTween.cancel(scoreLabel.gameObject);

            LeanTween.scale(scoreLabel.gameObject, Vector3.zero, scoreScaleTime)
                .setEaseSpring()
                .setIgnoreTimeScale(true)
                .setOnComplete(ShowGameOver);
        }

        public void SubmitGuestScore()
        {
            var guestName = nameInput.text;
            var score = player.GetScore();

            Scores.Add(score, score.ToString(), guestName, 0, "muh", ShowLeaderboard);
        }

        public void Restart()
        {
            GameJoltUI.Instance.DismissLeaderboard();
            SceneManager.LoadScene(0);
        }

        #endregion

        #region PRIVATE_METHODS

        private void ShowGameOver()
        {
            gameObject.SetActive(true);
            
            GetComponent<AudioSource>().clip = gameOverSounds[Random.Range(0, gameOverSounds.Length)];
            GetComponent<AudioSource>().Play();
            
            LeanTween.value(gameObject, value =>
            {
                _image.color = new Color(
                    _image.color.r,
                    _image.color.g,
                    _image.color.b,
                    value
                );
            }, 0f, 1f / 255f * 160f, gameOverFadeTime).setIgnoreTimeScale(true);

            LeanTween.moveY(gameOverLabel.rectTransform, -100, gameOverFadeTime)
                .setIgnoreTimeScale(true)
                .setEaseInOutQuint();

            LeanTween.scale(gameOverScoreLabel.rectTransform, Vector3.one, gameOverFadeTime / 1.5f)
                .setIgnoreTimeScale(true)
                .setDelay(gameOverFadeTime / 2f)
                .setOvershoot(1.3f)
                .setEaseSpring();

            LeanTween.scaleX(playAgainButton.gameObject, 1f, gameOverFadeTime / 1.5f)
                .setIgnoreTimeScale(true)
                .setDelay(gameOverFadeTime / 2f)
                .setOvershoot(1.3f)
                .setEaseSpring();

            var isSignedIn = GameJoltAPI.Instance.HasSignedInUser;

            if (isSignedIn)
            {
                SubmitUserScore();
                return;
            }

            LeanTween
                .scaleX(nameInput.gameObject, 1f, gameOverFadeTime / 1.5f)
                .setIgnoreTimeScale(true)
                .setDelay(gameOverFadeTime / 2f)
                .setOvershoot(1.3f)
                .setEaseSpring();

            LeanTween.scaleX(submitScoreButton.gameObject, 1f, gameOverFadeTime / 1.5f)
                .setIgnoreTimeScale(true)
                .setDelay(gameOverFadeTime / 2f)
                .setOvershoot(1.3f)
                .setEaseSpring();
        }

        private void SubmitUserScore()
        {
            var score = player.GetScore();

            Scores.Add(score, score.ToString(), 0, "muh", ShowLeaderboard);
        }

        private void ShowLeaderboard(bool success)
        {
            LeanTween
                .scaleX(nameInput.gameObject, 0f, gameOverFadeTime / 1.5f)
                .setIgnoreTimeScale(true)
                .setEaseSpring();

            LeanTween.scaleX(submitScoreButton.gameObject, 0f, gameOverFadeTime / 1.5f)
                .setIgnoreTimeScale(true)
                .setEaseSpring()
                .setOnComplete(() => { GameJoltUI.Instance.ShowLeaderboards(); });
        }

        #endregion
    }
}