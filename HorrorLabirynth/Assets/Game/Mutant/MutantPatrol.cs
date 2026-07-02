using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine.Rendering;
#endif

namespace Game.Mutant
{
    [RequireComponent(typeof(MutantNavMovement))]
    public class MutantPatrol : MonoBehaviour
    {
        const float _patrolPointRadius = 0.45f;
        const float _patrolGizmoHeightOffset = 0.15f;

        Transform[] _patrolPoints;
        float _patrolSpeed;
        float _waypointReachDistance;

        MutantNavMovement _movement;
        int _patrolIndex;
        bool _patrolStarted;

        public bool HasRoute => _patrolPoints != null && _patrolPoints.Length > 0;
        public float PatrolSpeed => _patrolSpeed;
        public float WaypointReachDistance => _waypointReachDistance;
        public int CurrentIndex => _patrolIndex;

        private void Awake()
        {
            _movement = GetComponent<MutantNavMovement>();
        }

        public void Configure(Transform[] patrolPoints, float patrolSpeed, float waypointReachDistance)
        {
            _patrolPoints = patrolPoints;
            _patrolSpeed = patrolSpeed;
            _waypointReachDistance = waypointReachDistance;
        }

        public void SetCurrentIndex(int index)
        {
            _patrolIndex = index;
        }

        public Transform GetCurrentPoint()
        {
            if (!HasRoute || _patrolPoints[_patrolIndex] == null)
                return null;

            return _patrolPoints[_patrolIndex];
        }

        public bool TryStart()
        {
            if (_patrolStarted || !HasRoute)
                return false;

            if (!_movement.EnsureOnNavMesh())
                return false;

            Transform currentPoint = GetCurrentPoint();
            if (currentPoint == null)
                return false;

            if (_movement.IsAtPosition(currentPoint.position, _waypointReachDistance))
            {
                _patrolStarted = true;
                MoveToNext();
                return true;
            }

            _patrolStarted = MoveTo(currentPoint.position);
            return _patrolStarted;
        }

        public bool HasReachedCurrentPoint()
        {
            Transform currentPoint = GetCurrentPoint();
            if (currentPoint == null)
                return false;

            return _movement.HasReachedDestination(currentPoint.position, _waypointReachDistance);
        }

        public void MoveToNext()
        {
            if (!HasRoute)
                return;

            _patrolIndex = (_patrolIndex + 1) % _patrolPoints.Length;
            MoveTo(_patrolPoints[_patrolIndex].position);
        }

        public Transform GetNearestPoint()
        {
            Transform nearest = _patrolPoints[0];
            float nearestDistance = MutantNavUtility.HorizontalDistance(transform.position, nearest.position);

            for (int i = 1; i < _patrolPoints.Length; i++)
            {
                float distance = MutantNavUtility.HorizontalDistance(transform.position, _patrolPoints[i].position);
                if (distance >= nearestDistance)
                    continue;

                nearestDistance = distance;
                nearest = _patrolPoints[i];
            }

            return nearest;
        }

        public int IndexOf(Transform point)
        {
            for (int i = 0; i < _patrolPoints.Length; i++)
            {
                if (_patrolPoints[i] == point)
                    return i;
            }

            return 0;
        }

        public bool MoveTo(Vector3 target)
        {
            _movement.SetSpeed(_patrolSpeed);
            return _movement.SetDestination(target);
        }

        public bool HasReachedPoint(Vector3 target)
        {
            return _movement.HasReachedDestination(target, _waypointReachDistance);
        }

        private void OnDrawGizmosSelected()
        {
            if (_patrolPoints == null || _patrolPoints.Length == 0)
                return;

            Gizmos.color = Color.blue;

            for (int i = 0; i < _patrolPoints.Length; i++)
            {
                if (_patrolPoints[i] == null)
                    continue;

                Vector3 point = GetPatrolGizmoPosition(_patrolPoints[i].position);
                Gizmos.DrawSphere(point, _patrolPointRadius);
                Gizmos.DrawWireSphere(point, _patrolPointRadius * 1.15f);

                Transform next = _patrolPoints[(i + 1) % _patrolPoints.Length];
                if (next != null)
                {
#if UNITY_EDITOR
                    MutantAiGizmos.DrawLineThroughWalls(point, GetPatrolGizmoPosition(next.position), Color.blue);
#endif
                }
            }
        }

        private static Vector3 GetPatrolGizmoPosition(Vector3 worldPosition)
        {
            worldPosition.y += _patrolGizmoHeightOffset;
            return worldPosition;
        }
    }

#if UNITY_EDITOR
    static class MutantAiGizmos
    {
        public static void DrawLineThroughWalls(Vector3 from, Vector3 to, Color color)
        {
            Color previousColor = Handles.color;
            CompareFunction previousZTest = Handles.zTest;

            Handles.color = color;
            Handles.zTest = CompareFunction.Always;
            Handles.DrawLine(from, to);

            Handles.color = previousColor;
            Handles.zTest = previousZTest;
        }
    }
#endif
}
