using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMover : MonoBehaviour
    {
        [FormerlySerializedAs("walkSpeed")]
        [SerializeField] float _walkSpeed = 3f;
        [FormerlySerializedAs("runSpeed")]
        [SerializeField] float _runSpeed = 6f;

        CharacterController _controller;

        private void Awake()
        {
            _controller = GetComponent<CharacterController>();
        }

        public void Move(Vector3 worldDirection, bool isRunning)
        {
            Vector3 direction = new Vector3(worldDirection.x, 0f, worldDirection.z).normalized;

            float speed = isRunning ? _runSpeed : _walkSpeed;
            Vector3 motion = direction * (speed * Time.deltaTime);

            _controller.Move(motion);
        }
    }
}
