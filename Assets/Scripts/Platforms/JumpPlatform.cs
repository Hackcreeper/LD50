using Entities;

namespace Platforms
{
    public class JumpPlatform : Platform
    {
        public float jumpForce = 20f;
        
        public override void OnPlayerEnter(Player player)
        {
            player.Jump(jumpForce);
        }
    }
}