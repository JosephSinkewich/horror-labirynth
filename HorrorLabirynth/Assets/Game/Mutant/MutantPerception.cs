using UnityEngine;

namespace Game.Mutant
{
    public class MutantPerception : MonoBehaviour
    {
        Transform _player;
        float _detectionRadius;
        float _loseRadius;
        float _catchRadius;

        public Transform Player => _player;

        public void Configure(Transform player, float detectionRadius, float loseRadius, float catchRadius)
        {
            _player = player;
            _detectionRadius = detectionRadius;
            _loseRadius = loseRadius;
            _catchRadius = catchRadius;
        }

        public float GetDistanceToPlayer()
        {
            return MutantNavUtility.HorizontalDistance(transform.position, _player.position);
        }

        public bool CanDetectPlayer()
        {
            return GetDistanceToPlayer() <= _detectionRadius;
        }

        public bool ShouldLoseChase()
        {
            return GetDistanceToPlayer() > _loseRadius;
        }

        public bool CanCatchPlayer()
        {
            return GetDistanceToPlayer() <= _catchRadius;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, _detectionRadius);

            Gizmos.color = Color.gray;
            Gizmos.DrawWireSphere(transform.position, _loseRadius);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _catchRadius);
        }
    }
}
