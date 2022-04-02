using System;
using Entities;
using Ui;
using UnityEngine;

namespace GameFlow
{
    public class FlowCamera : MonoBehaviour
    {
        #region PUBLIC_VARS

        public Player player;
        public float introAnimationTime = 1f;
        public Intro intro;

        #endregion

        #region PRIVATE_VARS

        private float _offsetY = 0.64f;

        private float _originalY = 4.86f;
        private float _originalZ = -18.44f;

        private float _minY;

        #endregion

        #region UNITY_FUNCTIONS

        private void Awake()
        {
            _minY = _originalY;
        }

        private void Update()
        {
            if (!player.HasStarted())
            {
                return;
            }

            var yPos = Mathf.Max(_minY, player.transform.position.y - _offsetY);
            var trans = transform;
            var position = trans.position;

            trans.position = new Vector3(
                position.x,
                yPos,
                position.z
            );

            _minY = yPos;
        }

        #endregion

        #region PUBLIC_METHODS

        public void OnStart(Action callback)
        {
            LeanTween.cancel(intro.gameNameLabel.rectTransform);
            LeanTween.cancel(intro.creditsLabel.rectTransform);

            LeanTween.value(intro.gameNameLabel.gameObject, value =>
            {
                intro.gameNameLabel.color = value;
            }, Color.white, new Color(1, 1, 1, 0), introAnimationTime);
            
            LeanTween.value(intro.creditsLabel.gameObject, value =>
            {
                intro.creditsLabel.color = value;
            }, Color.white, new Color(1, 1, 1, 0), introAnimationTime);
            
            LeanTween.move(
                    gameObject,
                    new Vector3(
                        transform.position.x,
                        _originalY,
                        _originalZ
                    ),
                    introAnimationTime
                ).setEaseLinear()
                .setOnComplete(callback);
        }

        #endregion
    }
}