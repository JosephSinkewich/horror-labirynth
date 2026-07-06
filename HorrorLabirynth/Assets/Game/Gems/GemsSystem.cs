using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Gems
{
    public class GemsSystem
    {
        private readonly GemsBalance _balance;
        private readonly List<GameObject> _spawnedGems = new();

        private int _collectedCount;

        public GemsSystem(GemsBalance balance)
        {
            _balance = balance;
        }

        public IReadOnlyList<GameObject> SpawnedGems => _spawnedGems;
        public int CollectedCount => _collectedCount;
        public int MaxGemsOnLevel => _balance.MaxGemsOnLevel;
        public int GemsRequiredToExit => _balance.GemsRequiredToExit;
        public float MinRespawnDistance => _balance.MinRespawnDistance;
        public bool HasCollectedRequiredGems => _collectedCount >= _balance.GemsRequiredToExit;

        public event Action<GameObject> OnGemCollected;
        public event Action OnAllRequiredGemsCollected;

        public void RegisterSpawnedGem(GameObject gem)
        {
            if (gem == null || _spawnedGems.Contains(gem))
                return;

            _spawnedGems.Add(gem);
        }

        public bool TryCollectGem(GameObject gem)
        {
            if (gem == null || !_spawnedGems.Remove(gem))
                return false;

            _collectedCount++;
            OnGemCollected?.Invoke(gem);
            UnityEngine.Object.Destroy(gem);

            if (HasCollectedRequiredGems)
                OnAllRequiredGemsCollected?.Invoke();

            return true;
        }
    }
}
