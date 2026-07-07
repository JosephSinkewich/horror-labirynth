using UnityEngine;

namespace Game.Gems
{
    [CreateAssetMenu(fileName = "GemResources", menuName = "Game/Gem Resources")]
    public class GemResources : ScriptableObject
    {
        [SerializeField] private GameObject _gemPrefab;

        public GameObject GemPrefab => _gemPrefab;
    }
}
