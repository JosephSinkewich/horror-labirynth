using System;
using System.Collections.Generic;
using Game;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Game.Gems
{
    public class GemsSpawner : IDisposable
    {
        private readonly GemsSystem _gemsSystem;
        private readonly IObjectResolver _objectResolver;
        private readonly Transform _gemsRoot;
        private readonly GameObject _gemPrefab;
        private readonly GemPlaceholdersSystem _gemPlaceholdersSystem;

        public GemsSpawner(
            GemsSystem gemsSystem,
            IObjectResolver objectResolver,
            [Key(TransformContainerId.GemsRoot)] Transform gemsRoot,
            GemResources gemResources,
            GemPlaceholdersSystem gemPlaceholdersSystem)
        {
            _gemsSystem = gemsSystem;
            _objectResolver = objectResolver;
            _gemsRoot = gemsRoot;
            _gemPrefab = gemResources.GemPrefab;
            _gemPlaceholdersSystem = gemPlaceholdersSystem;
            _gemsSystem.OnGemDestroyed += OnGemDestroyed;
        }

        public void Initialize()
        {
            SpawnInitialGems();
        }

        public void Dispose()
        {
            _gemsSystem.OnGemDestroyed -= OnGemDestroyed;
        }

        private void SpawnInitialGems()
        {
            List<GemPlaceholder> availablePlaceholders = GetFreePlaceholders();
            Shuffle(availablePlaceholders);

            int gemsToSpawn = Mathf.Min(_gemsSystem.MaxGemsOnLevel, availablePlaceholders.Count);
            for (int i = 0; i < gemsToSpawn; i++)
                SpawnAt(availablePlaceholders[i]);
        }

        private void OnGemDestroyed(GameObject gem)
        {
            GemPlaceholder placeholder = FindPlaceholderWithGem(gem);
            if (placeholder == null)
                return;

            Vector3 collectedPosition = placeholder.transform.position;
            placeholder.Release();
            TryRespawnGem(collectedPosition);
        }

        private void TryRespawnGem(Vector3 collectedPosition)
        {
            if (_gemsSystem.SpawnedGems.Count >= _gemsSystem.MaxGemsOnLevel)
                return;

            List<GemPlaceholder> candidates = GetFreePlaceholdersAtLeastDistance(collectedPosition, _gemsSystem.MinRespawnDistance);
            if (candidates.Count == 0)
                candidates = GetFreePlaceholders();

            if (candidates.Count == 0)
                return;

            SpawnAt(candidates[UnityEngine.Random.Range(0, candidates.Count)]);
        }

        private void SpawnAt(GemPlaceholder placeholder)
        {
            if (placeholder == null || _gemsRoot == null || placeholder.IsOccupied)
                return;

            Transform placeholderTransform = placeholder.transform;
            GameObject gem = _objectResolver.Instantiate(
                _gemPrefab,
                placeholderTransform.position,
                placeholderTransform.rotation,
                _gemsRoot);
            _gemsSystem.RegisterSpawnedGem(gem);
            placeholder.TryOccupy(gem);
        }

        private GemPlaceholder FindPlaceholderWithGem(GameObject gem)
        {
            IReadOnlyList<GemPlaceholder> placeholders = _gemPlaceholdersSystem.Placeholders;

            for (int i = 0; i < placeholders.Count; i++)
            {
                GemPlaceholder placeholder = placeholders[i];
                if (placeholder != null && placeholder.HasGem(gem))
                    return placeholder;
            }

            return null;
        }

        private List<GemPlaceholder> GetFreePlaceholders()
        {
            var result = new List<GemPlaceholder>();
            IReadOnlyList<GemPlaceholder> placeholders = _gemPlaceholdersSystem.Placeholders;

            for (int i = 0; i < placeholders.Count; i++)
            {
                GemPlaceholder placeholder = placeholders[i];
                if (placeholder != null && !placeholder.IsOccupied)
                    result.Add(placeholder);
            }

            return result;
        }

        private List<GemPlaceholder> GetFreePlaceholdersAtLeastDistance(Vector3 fromPosition, float minDistance)
        {
            var result = new List<GemPlaceholder>();
            float minDistanceSqr = minDistance * minDistance;
            IReadOnlyList<GemPlaceholder> placeholders = _gemPlaceholdersSystem.Placeholders;

            for (int i = 0; i < placeholders.Count; i++)
            {
                GemPlaceholder placeholder = placeholders[i];
                if (placeholder == null || placeholder.IsOccupied)
                    continue;

                if ((placeholder.transform.position - fromPosition).sqrMagnitude >= minDistanceSqr)
                    result.Add(placeholder);
            }

            return result;
        }

        private static void Shuffle(List<GemPlaceholder> items)
        {
            for (int i = items.Count - 1; i > 0; i--)
            {
                int swapIndex = UnityEngine.Random.Range(0, i + 1);
                (items[i], items[swapIndex]) = (items[swapIndex], items[i]);
            }
        }
    }
}
