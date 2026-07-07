using System;
using Game.Gems;

namespace Game.Player
{
    public class PlayerResources
    {
        private readonly GemsBalance _balance;

        private int _collectedGemsCount;

        public PlayerResources(GemsBalance balance)
        {
            _balance = balance;
        }

        public int CollectedGemsCount => _collectedGemsCount;
        public bool HasCollectedRequiredGems => _collectedGemsCount >= _balance.GemsRequiredToExit;

        public event Action OnGemCollected;
        public event Action OnAllCollected;

        public void CollectGem()
        {
            _collectedGemsCount++;
            OnGemCollected?.Invoke();

            if (HasCollectedRequiredGems)
                OnAllCollected?.Invoke();
        }
    }
}
