using System.Collections.Generic;
using UnityEngine;
using VContainer;
using VContainer.Unity;
namespace Game.Gems
{
    public class GemsSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject _gemPrefab;
        [SerializeField] private Transform _gemsRoot;
        [SerializeField] private Transform[] _placeholders = System.Array.Empty<Transform>();

        private GemsSystem _gemsSystem;
        private IObjectResolver _objectResolver;        private readonly Dictionary<Transform, GameObject> _placeholderToGem = new();
        private readonly Dictionary<GameObject, Transform> _gemToPlaceholder = new();

        [Inject]
        public void Construct(GemsSystem gemsSystem, IObjectResolver objectResolver)
        {
            _gemsSystem = gemsSystem;
            _objectResolver = objectResolver;
            _gemsSystem.OnGemDestroyed += OnGemDestroyed;
        }
        private void OnDestroy()
        {
            if (_gemsSystem != null)
                _gemsSystem.OnGemDestroyed -= OnGemDestroyed;
        }

        private void Start()
        {
            SpawnInitialGems();
        }

        private void SpawnInitialGems()
        {
            List<Transform> availablePlaceholders = GetFreePlaceholders();
            Shuffle(availablePlaceholders);

            int gemsToSpawn = Mathf.Min(_gemsSystem.MaxGemsOnLevel, availablePlaceholders.Count);
            for (int i = 0; i < gemsToSpawn; i++)
                SpawnAt(availablePlaceholders[i]);
        }

        private void OnGemDestroyed(GameObject gem)
        {
            if (!_gemToPlaceholder.TryGetValue(gem, out Transform placeholder))
                return;

            _gemToPlaceholder.Remove(gem);
            _placeholderToGem.Remove(placeholder);

            TryRespawnGem(placeholder.position);        }

        private void TryRespawnGem(Vector3 collectedPosition)
        {
            if (_gemsSystem.SpawnedGems.Count >= _gemsSystem.MaxGemsOnLevel)
                return;

            List<Transform> candidates = GetFreePlaceholdersAtLeastDistance(collectedPosition, _gemsSystem.MinRespawnDistance);
            if (candidates.Count == 0)
                candidates = GetFreePlaceholders();

            if (candidates.Count == 0)
                return;

            SpawnAt(candidates[Random.Range(0, candidates.Count)]);
        }

        private void SpawnAt(Transform placeholder)
        {
            if (placeholder == null || _gemsRoot == null || _placeholderToGem.ContainsKey(placeholder))
                return;

            GameObject gem = _objectResolver.Instantiate(_gemPrefab, placeholder.position, placeholder.rotation, _gemsRoot);
            _gemsSystem.RegisterSpawnedGem(gem);
            _placeholderToGem[placeholder] = gem;
            _gemToPlaceholder[gem] = placeholder;
        }

        private List<Transform> GetFreePlaceholders()
        {
            var result = new List<Transform>();

            for (int i = 0; i < _placeholders.Length; i++)
            {
                Transform placeholder = _placeholders[i];
                if (placeholder != null && !_placeholderToGem.ContainsKey(placeholder))
                    result.Add(placeholder);
            }

            return result;
        }

        private List<Transform> GetFreePlaceholdersAtLeastDistance(Vector3 fromPosition, float minDistance)
        {
            var result = new List<Transform>();
            float minDistanceSqr = minDistance * minDistance;

            for (int i = 0; i < _placeholders.Length; i++)
            {
                Transform placeholder = _placeholders[i];
                if (placeholder == null || _placeholderToGem.ContainsKey(placeholder))
                    continue;

                if ((placeholder.position - fromPosition).sqrMagnitude >= minDistanceSqr)
                    result.Add(placeholder);
            }

            return result;
        }

        private static void Shuffle(List<Transform> items)
        {
            for (int i = items.Count - 1; i > 0; i--)
            {
                int swapIndex = Random.Range(0, i + 1);
                (items[i], items[swapIndex]) = (items[swapIndex], items[i]);
            }
        }
    }
}
