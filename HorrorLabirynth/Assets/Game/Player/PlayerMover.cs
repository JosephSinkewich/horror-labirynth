using UnityEngine;

namespace Game.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMover : MonoBehaviour
    {
        [SerializeField] float walkSpeed = 3f;
        [SerializeField] float runSpeed = 6f;

        CharacterController _controller;

        void Awake()
        {
            _controller = GetComponent<CharacterController>();
        }

        public void Move(Vector3 worldDirection, bool isRunning)
        {
            Vector3 direction = new Vector3(worldDirection.x, 0f, worldDirection.z).normalized;

            float speed = isRunning ? runSpeed : walkSpeed;
            Vector3 motion = direction * (speed * Time.deltaTime);

            _controller.Move(motion);
        }
    }
}
