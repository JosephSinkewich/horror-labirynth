using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Player
{
    [CreateAssetMenu(fileName = "PlayerInputSettings", menuName = "Game/Player Input Settings")]
    public class PlayerInputSettings : ScriptableObject
    {
        [SerializeField] InputActionReference moveAction;
        [SerializeField] InputActionReference lookAction;
        [SerializeField] InputActionReference sprintAction;

        public InputAction MoveAction => moveAction.action;
        public InputAction LookAction => lookAction.action;
        public InputAction SprintAction => sprintAction.action;
    }
}
