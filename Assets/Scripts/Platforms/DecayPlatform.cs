using Entities;
using UnityEngine;

namespace Platforms
{
    public class DecayPlatform : Platform
    {
        public Rigidbody2D[] rigidbodies;
        
        protected override void OnLeave(Player player)
        {
            LeanTween.value(gameObject, val =>
            {
                rigidbodies[Mathf.FloorToInt(val)].bodyType = RigidbodyType2D.Dynamic;
            }, 0, rigidbodies.Length - 1, 0.8f);

            GetComponent<BoxCollider2D>().enabled = false;
        }
    }
}