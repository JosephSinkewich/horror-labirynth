using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Gems
{
    public class GemsSystem
    {
        private readonly GemsBalance _balance;
        private readonly List<GameObject> _spawnedGems = new();

        public GemsSystem(GemsBalance balance)
        {
            _balance = balance;
        }

        public IReadOnlyList<GameObject> SpawnedGems => _spawnedGems;
        public int MaxGemsOnLevel => _balance.MaxGemsOnLevel;
        public float MinRespawnDistance => _balance.MinRespawnDistance;

        public event Action<GameObject> OnGemDestroyed;

        public void RegisterSpawnedGem(GameObject gem)
        {
            if (gem == null || _spawnedGems.Contains(gem))
                return;

            _spawnedGems.Add(gem);
        }

        public bool TryDestroyGem(GameObject gem)
        {
            if (gem == null || !_spawnedGems.Remove(gem))
                return false;

            OnGemDestroyed?.Invoke(gem);
            UnityEngine.Object.Destroy(gem);
            return true;
        }
    }
}
