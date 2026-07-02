using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Mutant
{
    [RequireComponent(typeof(MutantNavMovement))]
    [RequireComponent(typeof(MutantPatrol))]
    [RequireComponent(typeof(MutantPerception))]
    public class MutantAi : MonoBehaviour
    {
        enum State
        {
            Patrol,
            Chase,
            ReturnToPatrol
        }

        [FormerlySerializedAs("player")]
        [SerializeField] Transform _player;
        [FormerlySerializedAs("patrolPoints")]
        [SerializeField] Transform[] _patrolPoints;
        [FormerlySerializedAs("detectionRadius")]
        [SerializeField] float _detectionRadius = 8f;
        [FormerlySerializedAs("loseRadius")]
        [SerializeField] float _loseRadius = 12f;
        [FormerlySerializedAs("catchRadius")]
        [SerializeField] float _catchRadius = 1.2f;
        [FormerlySerializedAs("waypointReachDistance")]
        [SerializeField] float _waypointReachDistance = 0.6f;
        [FormerlySerializedAs("patrolSpeed")]
        [SerializeField] float _patrolSpeed = 2.5f;
        [FormerlySerializedAs("chaseSpeed")]
        [SerializeField] float _chaseSpeed = 4f;
        [FormerlySerializedAs("destinationRefreshDistance")]
        [SerializeField] float _destinationRefreshDistance = 0.5f;

        MutantNavMovement _movement;
        MutantPatrol _patrol;
        MutantPerception _perception;

        State _state = State.Patrol;
        Transform _returnTarget;
        bool _hasCaughtPlayer;

        public event Action PlayerCaught;

        private void Awake()
        {
            _movement = GetComponent<MutantNavMovement>();
            _patrol = GetComponent<MutantPatrol>();
            _perception = GetComponent<MutantPerception>();
            ApplyConfiguration();
        }

        private void OnValidate()
        {
            if (_movement == null)
                _movement = GetComponent<MutantNavMovement>();
            if (_patrol == null)
                _patrol = GetComponent<MutantPatrol>();
            if (_perception == null)
                _perception = GetComponent<MutantPerception>();

            ApplyConfiguration();
        }

        private void Start()
        {
            _movement.SetSpeed(_patrolSpeed);
            _patrol.TryStart();
        }

        private void Update()
        {
            if (_hasCaughtPlayer || _perception.Player == null)
                return;

            _patrol.TryStart();

            switch (_state)
            {
                case State.Patrol:
                    UpdatePatrol();
                    break;
                case State.Chase:
                    UpdateChase();
                    break;
                case State.ReturnToPatrol:
                    UpdateReturn();
                    break;
            }
        }

        private void ApplyConfiguration()
        {
            if (_patrol != null)
                _patrol.Configure(_patrolPoints, _patrolSpeed, _waypointReachDistance);

            if (_perception != null)
                _perception.Configure(_player, _detectionRadius, _loseRadius, _catchRadius);
        }

        private void UpdatePatrol()
        {
            if (_perception.CanDetectPlayer())
            {
                BeginChase();
                return;
            }

            if (!_patrol.HasRoute)
                return;

            if (_patrol.HasReachedCurrentPoint())
                _patrol.MoveToNext();
        }

        private void UpdateChase()
        {
            if (_perception.CanCatchPlayer())
            {
                CatchPlayer();
                return;
            }

            if (_perception.ShouldLoseChase())
            {
                BeginReturnToPatrol();
                return;
            }

            _movement.RefreshDestination(_perception.Player.position, _chaseSpeed, _destinationRefreshDistance);
        }

        private void UpdateReturn()
        {
            if (_perception.CanDetectPlayer())
            {
                BeginChase();
                return;
            }

            if (_returnTarget == null)
            {
                _state = State.Patrol;
                return;
            }

            if (!_patrol.HasReachedPoint(_returnTarget.position))
                return;

            _patrol.SetCurrentIndex(_patrol.IndexOf(_returnTarget));
            _state = State.Patrol;
            _returnTarget = null;
            _patrol.MoveToNext();
        }

        private void BeginChase()
        {
            _state = State.Chase;
            _returnTarget = null;
            _movement.RefreshDestination(_perception.Player.position, _chaseSpeed, _destinationRefreshDistance);
        }

        private void BeginReturnToPatrol()
        {
            _state = State.ReturnToPatrol;

            if (!_patrol.HasRoute)
            {
                _state = State.Patrol;
                _movement.ResetPath();
                return;
            }

            _returnTarget = _patrol.GetNearestPoint();
            _patrol.SetCurrentIndex(_patrol.IndexOf(_returnTarget));
            _patrol.MoveTo(_returnTarget.position);
        }

        private void CatchPlayer()
        {
            _hasCaughtPlayer = true;
            _movement.Stop();
            PlayerCaught?.Invoke();
        }
    }
}
