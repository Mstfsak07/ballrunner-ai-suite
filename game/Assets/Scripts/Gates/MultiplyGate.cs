using BallRunner.Balls;
using UnityEngine;

namespace BallRunner.Gates
{
    public sealed class MultiplyGate : GateBase
    {
        [SerializeField] private float multiplier = 2f;

        protected override string BuildLabel() => $"x{multiplier:0.#}";

        public void ConfigureMultiplier(float nextMultiplier)
        {
            multiplier = Mathf.Max(0f, nextMultiplier);
            RefreshLabel();
        }

        protected override void Apply(BallGroupController group)
        {
            group.ApplyMultiplier(Mathf.Max(0f, multiplier));
        }
    }
}
