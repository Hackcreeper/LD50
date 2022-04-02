using System.Collections.Generic;
using System.Linq;
using Platforms;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameFlow
{
    public class LevelBuilder : MonoBehaviour
    {
        #region PUBLIC_VARS
        
        public GameObject basePlatformPrefab;
        public PlatformData[] platforms;
        
        public AnimationCurve distanceCurve;
        public float difficultyMultiplier = 0.0001f;
        
        #endregion
        
        #region PRIVATE_VARS
        
        private readonly Dictionary<float, Platform> _platforms = new();
        private Camera _camera;
        private float _lastY = -2;
        private int _steps;
        
        #endregion

        #region UNITY_FUNCTIONS
        
        private void Awake()
        {
            _camera = Camera.main;
        }

        private void Update()
        {
            var toCreate = 10 - _platforms.Count;
            for (var i = 0; i < toCreate; i++)
            {
                var distance = distanceCurve.Evaluate(_steps * difficultyMultiplier) * 10f;
                
                SpawnPlatform(_lastY + distance);
                _lastY += distance;
                _steps++;
            }

            var toRemove = new List<float>();
            foreach (var (key, value) in from platformPair in _platforms let viewPort = _camera.WorldToViewportPoint(new Vector3(0, platformPair.Key, 0)) where viewPort.y < 0 select platformPair)
            {
                toRemove.Add(key);
                Destroy(value.gameObject);
            }
            
            toRemove.ForEach(y => _platforms.Remove(y));
        }

        #endregion
        
        #region PRIVATE_METHODS
        
        private void SpawnPlatform(float y)
        {
            var platform = Instantiate(GetPlatformPrefab(), transform);
            platform.transform.position = new Vector3(Random.Range(-4f, 4f), y, 0);

            _platforms.Add(y, platform.GetComponent<Platform>());
        }

        private GameObject GetPlatformPrefab()
        {
            var possible = (from platform in platforms where Random.Range(0, 100) <= platform.chance select platform.prefab).ToList();

            return possible.Count ==  0 ? basePlatformPrefab : possible[Random.Range(0, possible.Count)];
        }
        
        #endregion
    }
}