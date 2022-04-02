using Entities;
using UnityEngine;

namespace GameFlow
{
    public class FlowCamera : MonoBehaviour
    {
        public Player player;

        private float _offsetY = 0.64f;
        private float _minY;
        
        private void Awake()
        {
            _minY = transform.position.y;
        }

        private void Update()
        {
            var yPos = Mathf.Max(_minY, player.transform.position.y - _offsetY);
            
            transform.position = new Vector3(
                transform.position.x,
                yPos,
                transform.position.z
            );

            _minY = yPos;
        }
    }
}