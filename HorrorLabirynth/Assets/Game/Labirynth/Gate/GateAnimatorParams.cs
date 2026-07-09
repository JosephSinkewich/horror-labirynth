using UnityEngine;

namespace Game.Labirynth.Gate
{
    public static class GateAnimatorParams
    {
        public const string Open = nameof(Open);
        public static readonly int OpenHash = Animator.StringToHash(Open);
    }
}
