using UnityEngine;

namespace Game.Gems
{
    [CreateAssetMenu(fileName = "GemsBalance", menuName = "Game/Gems Balance")]
    public class GemsBalance : ScriptableObject
    {
        [SerializeField] [Min(1)] private int _maxGemsOnLevel = 5;
        [SerializeField] [Min(1)] private int _gemsRequiredToExit = 10;
        [SerializeField] [Min(0f)] private float _minRespawnDistance = 5f;

        public int MaxGemsOnLevel => _maxGemsOnLevel;
        public int GemsRequiredToExit => _gemsRequiredToExit;
        public float MinRespawnDistance => _minRespawnDistance;
    }
}
