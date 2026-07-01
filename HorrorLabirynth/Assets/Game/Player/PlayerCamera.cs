using UnityEngine;

namespace Game.Player
{
    public class PlayerCamera : MonoBehaviour
    {
        [SerializeField] PlayerInputSettings settings;
        [SerializeField] Transform cameraPivot;
        [SerializeField] float lookSensitivity = 0.1f;
        [SerializeField] float minPitch = -89f;
        [SerializeField] float maxPitch = 89f;

        float _pitch;
        bool _isEnabled = true;

        void Start()
        {
            SetCursorLocked(true);
        }

        void Update()
        {
            if (!_isEnabled)
                return;

            Vector2 look = settings.LookAction.ReadValue<Vector2>();
            if (look.sqrMagnitude < 0.001f)
                return;

            transform.Rotate(Vector3.up, look.x * lookSensitivity);

            _pitch -= look.y * lookSensitivity;
            _pitch = Mathf.Clamp(_pitch, minPitch, maxPitch);
            cameraPivot.localRotation = Quaternion.Euler(_pitch, 0f, 0f);
        }

        public void SetEnabled(bool enabled)
        {
            _isEnabled = enabled;
            SetCursorLocked(enabled);
        }

        void SetCursorLocked(bool locked)
        {
            Cursor.lockState = locked ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = !locked;
        }
    }
}
