using System;

namespace Game.Gems
{
    public class GemsCollectionProgress
    {
        private readonly GemsBalance _balance;

        private int _collectedGemsCount;

        public GemsCollectionProgress(GemsBalance balance)
        {
            _balance = balance;
        }

        public int CollectedGemsCount => _collectedGemsCount;
        public int GemsRequiredToExit => _balance.GemsRequiredToExit;
        public bool HasCollectedRequiredGems => _collectedGemsCount >= GemsRequiredToExit;

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
