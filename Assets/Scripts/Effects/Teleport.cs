using Entities;
using UnityEngine;

namespace Effects
{
    public class Teleport : MonoBehaviour
    {
        public static float Cooldown;
        public Transform target;

        private void OnTriggerEnter2D(Collider2D col)
        {
            var player = col.GetComponent<Player>();
            if (!player)
            {
                return;
            }

            if (player.IsGrounded() || Cooldown > 0f)
            {
                return;
            }

            player.transform.position = new Vector3(
                target.position.x,
                player.transform.position.y,
                player.transform.position.z
            );

            Cooldown = 1f;
        }

        private void Update()
        {
            Cooldown -= Time.deltaTime;
        }
    }
}