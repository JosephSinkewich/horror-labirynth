using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Player
{
    [RequireComponent(typeof(PlayerMover))]
    public class PlayerController : MonoBehaviour
    {
        [FormerlySerializedAs("settings")]
        [SerializeField] private PlayerInputSettings _settings;
        [FormerlySerializedAs("playerCamera")]
        [SerializeField] private PlayerCamera _playerCamera;

        private PlayerMover _mover;
        private bool _isEnabled = true;

        private void Awake()
        {
            _mover = GetComponent<PlayerMover>();
        }

        private void Update()
        {
            if (!_isEnabled)
                return;

            Vector2 input = _settings.MoveAction.ReadValue<Vector2>();
            if (input.sqrMagnitude < 0.01f)
                return;

            Vector3 forward = transform.forward;
            Vector3 right = transform.right;
            forward.y = 0f;
            right.y = 0f;
            forward.Normalize();
            right.Normalize();

            Vector3 direction = right * input.x + forward * input.y;
            _mover.Move(direction, _settings.SprintAction.IsPressed());
        }

        public void SetControlEnabled(bool enabled)
        {
            _isEnabled = enabled;
            _playerCamera?.SetEnabled(enabled);
        }
    }
}
