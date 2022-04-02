using Entities;
using UnityEngine;

namespace Platforms
{
    public class Platform : MonoBehaviour
    {
        public virtual void OnPlayerEnter(Player player)
        {
            player.Jump(player.jumpForce);
        }
    }
}