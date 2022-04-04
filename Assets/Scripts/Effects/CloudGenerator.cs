using System.Collections.Generic;
using System.Linq;
using Entities;
using GameFlow;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Effects
{
    public class CloudGenerator : MonoBehaviour
    {
        #region PUBLIC_VARS

        public FlowCamera flowCamera;
        public GameObject[] cloudPrefabs;

        public AnimationCurve amountCurve;
        public float minY;
        public float maxY;
        public float distanceToCamera = 10f;
        public float minDistance = .2f;
        public float maxDistance = 1.5f;
        public float minParallaxSpeed = 0.1f;
        public float maxParallaxSpeed = 0.4f;
        
        #endregion

        #region PRIVATE_VARS
        
        private readonly List<Transform> _clouds = new();
        
        #endregion
        
        #region UNITY_METHODS

        private void Update()
        {
            HandleCloudCreation();
        }

        #endregion
        
        #region PRIVATE_METHODS

        private void HandleCloudCreation()
        {
            if (ShouldSpawn())
            {
                var amountPercentage = ((flowCamera.transform.position.y - minY) / (maxY - minY));
                var amount = Mathf.FloorToInt(amountCurve.Evaluate(amountPercentage) * 100);
                
                var toCreate = amount - _clouds.Count;
                for (var i = 0; i < toCreate; i++)
                {
                    var distance = Random.Range(minDistance, maxDistance);

                    SpawnCloud(distanceToCamera + distance);
                }
            }

            var toRemove = new List<Transform>();
            foreach (var cloud in _clouds.Where(cloud => cloud.localPosition.y < -(transform.localPosition.y + 9f)))
            {
                toRemove.Add(cloud);
                Destroy(cloud.gameObject);
            }

            toRemove.ForEach(y => _clouds.Remove(y));
        }
        
        private void SpawnCloud(float y)
        {
            var cloud = Instantiate(GetCloudPrefab(), transform);

            cloud.transform.localPosition = new Vector3(
                Random.Range(-6f, 6f),
                y,
                0
            );
            var scale = Random.Range(.1f, .3f);
            cloud.transform.localScale = Vector3.one * scale;

            cloud.GetComponent<Cloud>().flowCamera = flowCamera;
            cloud.GetComponent<Cloud>().parallaxSpeed = Random.Range(minParallaxSpeed, maxParallaxSpeed);
            
            _clouds.Add(cloud.transform);
        }

        private bool ShouldSpawn()
        {
            var camY = flowCamera.transform.position.y;

            return camY > minY && camY < maxY;
        }
        
        private GameObject GetCloudPrefab()
        {
            return cloudPrefabs[Random.Range(0, cloudPrefabs.Length)];
        }

        #endregion
    }
}