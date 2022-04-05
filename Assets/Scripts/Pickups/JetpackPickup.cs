using Entities;

namespace Pickups
{
    public class JetpackPickup : Pickup
    {
        protected override void OnCollect(Player player)
        {
            if (!player.IsJetpackEnabled())
            {
                player.EnableJetpack();
            }
            
            Destroy(gameObject);
        }
    }
}