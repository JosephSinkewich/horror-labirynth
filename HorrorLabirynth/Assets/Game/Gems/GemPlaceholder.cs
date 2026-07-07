using UnityEngine;
using VContainer;

namespace Game.Gems
{
    public class GemPlaceholder : MonoBehaviour
    {
        private GemPlaceholdersSystem _gemPlaceholdersSystem;
        private GameObject _spawnedGem;

        public bool IsOccupied => _spawnedGem != null;

        [Inject]
        public void Construct(GemPlaceholdersSystem gemPlaceholdersSystem)
        {
            _gemPlaceholdersSystem = gemPlaceholdersSystem;
            _gemPlaceholdersSystem.Register(this);
        }

        public bool TryOccupy(GameObject gem)
        {
            if (gem == null || _spawnedGem != null)
                return false;

            _spawnedGem = gem;
            return true;
        }

        public void Release()
        {
            _spawnedGem = null;
        }

        public bool HasGem(GameObject gem)
        {
            return _spawnedGem == gem;
        }

        private void OnDestroy()
        {
            if (_gemPlaceholdersSystem != null)
                _gemPlaceholdersSystem.Unregister(this);
        }
    }
}
