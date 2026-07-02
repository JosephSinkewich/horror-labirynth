using UnityEngine;

namespace Game.Mutant
{
    static class MutantNavUtility
    {
        public static float HorizontalDistance(Vector3 a, Vector3 b)
        {
            a.y = 0f;
            b.y = 0f;
            return Vector3.Distance(a, b);
        }
    }
}
