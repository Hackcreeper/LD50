using Entities;

namespace Pickups
{
    public class BonusScorePickup : Pickup
    {
        public int bonusScore = 100;
        
        protected override void OnCollect(Player player)
        {
            player.AddBonusScore(bonusScore);
            Destroy(gameObject);
        }
    }
}