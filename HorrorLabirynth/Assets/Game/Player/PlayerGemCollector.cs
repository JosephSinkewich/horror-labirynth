using System;
using Game;
using UnityEngine;

namespace Game.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerGemCollector : MonoBehaviour
    {
        [TagSelector]
        [SerializeField] string _gemTag = "Gem";

        public event Action GemCollected;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag(_gemTag))
                return;

            GemCollected?.Invoke();
            Destroy(other.gameObject);
        }
    }
}
