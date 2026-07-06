using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Player
{
    public class PlayerCamera : MonoBehaviour
    {
        [FormerlySerializedAs("settings")]
        [SerializeField] private PlayerInputSettings _settings;
        [FormerlySerializedAs("cameraPivot")]
        [SerializeField] private Transform _cameraPivot;
        [FormerlySerializedAs("lookSensitivity")]
        [SerializeField] private float _lookSensitivity = 0.1f;
        [FormerlySerializedAs("minPitch")]
        [SerializeField] private float _minPitch = -89f;
        [FormerlySerializedAs("maxPitch")]
        [SerializeField] private float _maxPitch = 89f;

        private float _pitch;
        private bool _isEnabled = true;

        private void Start()
        {
            SetCursorLocked(true);
        }

        private void Update()
        {
            if (!_isEnabled)
                return;

            Vector2 look = _settings.LookAction.ReadValue<Vector2>();
            if (look.sqrMagnitude < 0.001f)
                return;

            transform.Rotate(Vector3.up, look.x * _lookSensitivity);

            _pitch -= look.y * _lookSensitivity;
            _pitch = Mathf.Clamp(_pitch, _minPitch, _maxPitch);
            _cameraPivot.localRotation = Quaternion.Euler(_pitch, 0f, 0f);
        }

        public void SetEnabled(bool enabled)
        {
            _isEnabled = enabled;
            SetCursorLocked(enabled);
        }

        private void SetCursorLocked(bool locked)
        {
            Cursor.lockState = locked ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = !locked;
        }
    }
}
