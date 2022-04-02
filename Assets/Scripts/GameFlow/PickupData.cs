using System;
using UnityEngine;

namespace GameFlow
{
    [Serializable]
    public struct PickupData
    {
        public GameObject prefab;
        public int chance;
    }
}