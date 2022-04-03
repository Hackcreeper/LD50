using System;
using Entities;
using Pickups;
using UnityEngine;

namespace Platforms
{
    public class Platform : MonoBehaviour
    {
        #region PUBLIC_VARS

        public Pickup pickup;

        #endregion

        #region UNITY_METHODS

        protected virtual void Update()
        {
            if (!pickup)
            {
                return;
            }

            pickup.transform.position = transform.position + new Vector3(0, .8f, 0);
        }

        private void OnDestroy()
        {
            if (!pickup)
            {
                return;
            }
            
            Destroy(pickup.gameObject);
        }

        #endregion

        #region PUBLIC_METHODS

        public void OnPlayerEnter(Player player)
        {
            CollectPickup(player);
            OnEnter(player);
        }

        #endregion

        #region PROTECTED_METHODS

        protected virtual void OnEnter(Player player)
        {
            player.Jump(player.jumpForce);
        }

        #endregion

        #region PRIVATE_METHODS

        private void CollectPickup(Player player)
        {
            if (!pickup)
            {
                return;
            }
            
            pickup.OnPlayerCollect(player);
            pickup = null;
        }

        #endregion
    }
}