using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Game.Player
{
    [CreateAssetMenu(fileName = "PlayerInputSettings", menuName = "Game/Player Input Settings")]
    public class PlayerInputSettings : ScriptableObject
    {
        [FormerlySerializedAs("moveAction")]
        [SerializeField] private InputActionReference _moveAction;
        [FormerlySerializedAs("lookAction")]
        [SerializeField] private InputActionReference _lookAction;
        [FormerlySerializedAs("sprintAction")]
        [SerializeField] private InputActionReference _sprintAction;

        public InputAction MoveAction => _moveAction.action;
        public InputAction LookAction => _lookAction.action;
        public InputAction SprintAction => _sprintAction.action;
    }
}
