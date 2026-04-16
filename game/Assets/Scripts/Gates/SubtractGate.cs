using BallRunner.Balls;

namespace BallRunner.Gates
{
    public sealed class SubtractGate : GateBase
    {
        protected override string BuildLabel() => $"-{value}";

        protected override void Apply(BallGroupController group)
        {
            group.ApplyDelta(-value);
        }
    }
}
