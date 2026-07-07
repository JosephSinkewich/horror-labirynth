using Game;
using UnityEngine;
using UnityEngine.Serialization;
using VContainer;

namespace Game.Mutant
{
    [RequireComponent(typeof(MutantNavMovement))]
    [RequireComponent(typeof(MutantPatrol))]
    [RequireComponent(typeof(MutantPerception))]
    public class MutantAi : MonoBehaviour
    {
        private enum State
        {
            Patrol,
            Chase,
            ReturnToPatrol
        }

        [FormerlySerializedAs("player")]
        [SerializeField] private Transform _player;
        [FormerlySerializedAs("patrolPoints")]
        [SerializeField] private Transform[] _patrolPoints;
        [FormerlySerializedAs("detectionRadius")]
        [SerializeField] private float _detectionRadius = 8f;
        [FormerlySerializedAs("loseRadius")]
        [SerializeField] private float _loseRadius = 12f;
        [FormerlySerializedAs("catchRadius")]
        [SerializeField] private float _catchRadius = 1.2f;
        [FormerlySerializedAs("waypointReachDistance")]
        [SerializeField] private float _waypointReachDistance = 0.6f;
        [FormerlySerializedAs("patrolSpeed")]
        [SerializeField] private float _patrolSpeed = 2.5f;
        [FormerlySerializedAs("chaseSpeed")]
        [SerializeField] private float _chaseSpeed = 4f;
        [FormerlySerializedAs("destinationRefreshDistance")]
        [SerializeField] private float _destinationRefreshDistance = 0.5f;

        private MutantNavMovement _movement;
        private MutantPatrol _patrol;
        private MutantPerception _perception;

        private State _state = State.Patrol;
        private Transform _returnTarget;
        private bool _hasCaughtPlayer;
        private GameSession _gameSession;

        [Inject]
        public void Construct(GameSession gameSession)
        {
            _gameSession = gameSession;
        }

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
            _gameSession.OnMutantCaughtPlayer();
        }
    }
}
