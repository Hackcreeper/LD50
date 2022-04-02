using Entities;

namespace Platforms
{
    public class JumpPlatform : Platform
    {
        public float jumpForce = 20f;
        
        protected override void OnEnter(Player player)
        {
            player.Jump(jumpForce);
        }
    }
}