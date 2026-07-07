using Game;
using Game.Gems;
using UnityEngine;
using VContainer;

namespace Game.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerGemCollector : MonoBehaviour
    {
        [TagSelector]
        [SerializeField] private string _gemTag = "Gem";

        private GemsSystem _gemsSystem;
        private GemsCollectionProgress _gemsCollectionProgress;

        [Inject]
        public void Construct(GemsSystem gemsSystem, GemsCollectionProgress gemsCollectionProgress)
        {
            _gemsSystem = gemsSystem;
            _gemsCollectionProgress = gemsCollectionProgress;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag(_gemTag))
                return;

            if (!_gemsSystem.TryDestroyGem(other.gameObject))
                return;

            _gemsCollectionProgress.CollectGem();
        }
    }
}
