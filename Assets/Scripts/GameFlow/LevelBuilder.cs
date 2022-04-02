using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameFlow
{
    public class LevelBuilder : MonoBehaviour
    {
        public GameObject platformPrefab;
        public AnimationCurve distanceCurve;
        public float difficultyMultiplier = 0.0001f;
        
        private readonly Dictionary<float, Transform> _platforms = new();
        private Camera _camera;
        private float _lastY = -2;
        private int _steps = 0;

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
                Debug.Log("Last distance = " + distance);
                
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

        private void SpawnPlatform(float y)
        {
            var platform = Instantiate(platformPrefab, transform);
            platform.transform.position = new Vector3(Random.Range(-4f, 4f), y, 0);

            _platforms.Add(y, platform.transform);
        }
    }
}