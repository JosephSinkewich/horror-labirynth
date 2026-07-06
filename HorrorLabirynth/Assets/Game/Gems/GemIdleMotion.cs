using UnityEngine;

namespace Game.Gem
{
    public class GemIdleMotion : MonoBehaviour
    {
        [SerializeField] private float _rotationSpeed = 90f;
        [SerializeField] private float _floatAmplitude = 0.15f;
        [SerializeField] private float _floatFrequency = 1f;
        [SerializeField] private float _phaseOffset;

        private Vector3 _initialLocalPosition;

        private void Awake()
        {
            _initialLocalPosition = transform.localPosition;
        }

        private void Update()
        {
            transform.Rotate(Vector3.up, _rotationSpeed * Time.deltaTime, Space.Self);

            float yOffset = Mathf.Sin((Time.time + _phaseOffset) * _floatFrequency) * _floatAmplitude;
            transform.localPosition = _initialLocalPosition + Vector3.up * yOffset;
        }
    }
}
