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
        private PlayerResources _playerResources;

        [Inject]
        public void Construct(GemsSystem gemsSystem, PlayerResources playerResources)
        {
            _gemsSystem = gemsSystem;
            _playerResources = playerResources;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag(_gemTag))
                return;

            if (!_gemsSystem.TryDestroyGem(other.gameObject))
                return;

            _playerResources.CollectGem();
        }
    }
}
