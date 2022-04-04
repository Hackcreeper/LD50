using Entities;
using UnityEngine;

namespace Platforms
{
    public class JumpPlatform : Platform
    {
        public float jumpForce = 20f;
        public Animator animator;
        
        private static readonly int JumpingAction = Animator.StringToHash("jumping");

        protected override void OnEnter(Player player)
        {
            player.ForceJump(jumpForce);
            animator.SetBool(JumpingAction, true);
        }
    }
}