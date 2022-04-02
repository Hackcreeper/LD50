using GameFlow;
using UnityEngine;
using UnityEngine.UI;

namespace Ui
{
    public class Background : MonoBehaviour
    {
        public Image image;
        public FlowCamera flowCamera;

        public void OnStart()
        {
            LeanTween.scale(
                GetComponent<RectTransform>(),
                Vector3.one,
                flowCamera.introAnimationTime
            );
        }
    }
}