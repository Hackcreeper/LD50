using Entities;
using Platforms;
using UnityEngine;

namespace Pickups
{
    public abstract class Pickup : MonoBehaviour
    {
        #region PUBLIC_VARS

        public string label;
        public Platform platform;
            
        #endregion

        #region PRIVATE_VARS

        private bool _collected;

        #endregion
        
        #region PUBLIC_METHODS

        public void OnPlayerCollect(Player player)
        {
            if (_collected)
            {
                return;
            }
            
            // TODO:
            // - Mark the pickup on the player (for some)
            // - Maybe have an internal cooldown, if necessary which removes the pickup after some time (co-routine?)

            _collected = true;
            platform.CollectPickup(player);
            
            player.ShowTooltip($"Collected {label}");
            OnCollect(player);
        }

        protected abstract void OnCollect(Player player);

        #endregion
    }
}