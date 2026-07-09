using Game.Gems;
using UnityEngine;
using VContainer;

namespace Game.Labirynth.Gate
{
    public class Gate : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private Collider _blockingCollider;

        private GemsCollectionProgress _gemsCollectionProgress;
        private bool _hasOpened;

        [Inject]
        public void Construct(GemsCollectionProgress gemsCollectionProgress)
        {
            _gemsCollectionProgress = gemsCollectionProgress;
            _gemsCollectionProgress.OnAllCollected += OnAllCollected;
        }

        private void OnDestroy()
        {
            if (_gemsCollectionProgress != null)
                _gemsCollectionProgress.OnAllCollected -= OnAllCollected;
        }

        private void OnAllCollected()
        {
            Open();
        }

        private void Open()
        {
            if (_hasOpened)
                return;

            _hasOpened = true;
            _blockingCollider.enabled = false;
            _animator.SetTrigger(GateAnimatorParams.OpenHash);
        }
    }
}
