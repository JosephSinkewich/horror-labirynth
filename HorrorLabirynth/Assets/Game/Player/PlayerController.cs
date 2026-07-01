using UnityEngine;

namespace Game.Player
{
    [RequireComponent(typeof(PlayerMover))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] PlayerInputSettings settings;
        [SerializeField] PlayerCamera playerCamera;

        PlayerMover _mover;
        bool _isEnabled = true;

        void Awake()
        {
            _mover = GetComponent<PlayerMover>();
        }

        void Update()
        {
            if (!_isEnabled)
                return;

            Vector2 input = settings.MoveAction.ReadValue<Vector2>();
            if (input.sqrMagnitude < 0.01f)
                return;

            Vector3 forward = transform.forward;
            Vector3 right = transform.right;
            forward.y = 0f;
            right.y = 0f;
            forward.Normalize();
            right.Normalize();

            Vector3 direction = right * input.x + forward * input.y;
            _mover.Move(direction, settings.SprintAction.IsPressed());
        }

        public void SetControlEnabled(bool enabled)
        {
            _isEnabled = enabled;
            playerCamera?.SetEnabled(enabled);
        }
    }
}
