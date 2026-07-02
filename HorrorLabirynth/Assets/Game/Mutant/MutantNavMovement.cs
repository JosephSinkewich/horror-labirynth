using UnityEngine;
using UnityEngine.AI;

namespace Game.Mutant
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class MutantNavMovement : MonoBehaviour
    {
        NavMeshAgent _agent;

        public bool IsOnNavMesh => _agent.isOnNavMesh;

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
        }

        public bool EnsureOnNavMesh()
        {
            if (_agent.isOnNavMesh)
                return true;

            if (!NavMesh.SamplePosition(transform.position, out NavMeshHit hit, 2f, NavMesh.AllAreas))
                return false;

            _agent.Warp(hit.position);
            return _agent.isOnNavMesh;
        }

        public void SetSpeed(float speed)
        {
            _agent.speed = speed;
        }

        public bool SetDestination(Vector3 target)
        {
            if (!_agent.enabled || !_agent.isOnNavMesh)
                return false;

            if (_agent.isStopped)
                _agent.isStopped = false;

            return _agent.SetDestination(target);
        }

        public void RefreshDestination(Vector3 target, float speed, float refreshDistance)
        {
            SetSpeed(speed);

            if (MutantNavUtility.HorizontalDistance(_agent.destination, target) > refreshDistance)
                SetDestination(target);
        }

        public bool HasReachedDestination(Vector3 destination, float reachDistance)
        {
            if (_agent.pathPending)
                return false;

            if (IsAtPosition(destination, reachDistance))
                return true;

            if (!_agent.hasPath)
                return false;

            return _agent.remainingDistance <= reachDistance;
        }

        public bool IsAtPosition(Vector3 destination, float reachDistance)
        {
            return MutantNavUtility.HorizontalDistance(transform.position, destination) <= reachDistance;
        }

        public void Stop()
        {
            if (!_agent.isOnNavMesh)
                return;

            _agent.isStopped = true;
            _agent.ResetPath();
        }

        public void ResetPath()
        {
            if (!_agent.isOnNavMesh)
                return;

            _agent.ResetPath();
        }
    }
}
