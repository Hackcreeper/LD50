using Entities;
using UnityEngine;

namespace Pickups
{
    public abstract class Pickup : MonoBehaviour
    {
        #region PUBLIC_VARS

        public string label;
            
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
            // - Show tooltip / info: "Collected {label}"
            // - Mark the pickup on the player
            // - Maybe have an internal cooldown, if necessary which removes the pickup after some time (co-routine?)
            // - Disable the pickup on the field (myself)

            _collected = true;
            
            player.ShowTooltip($"Collected {label}");
            OnCollect(player);
        }

        protected abstract void OnCollect(Player player);

        #endregion
    }
}