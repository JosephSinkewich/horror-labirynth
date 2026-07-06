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

        [Inject]
        public void Construct(GemsSystem gemsSystem)
        {
            _gemsSystem = gemsSystem;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag(_gemTag))
                return;

            _gemsSystem.TryCollectGem(other.gameObject);
        }
    }
}
