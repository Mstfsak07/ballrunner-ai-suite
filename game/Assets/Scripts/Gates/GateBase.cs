using BallRunner.Balls;
using UnityEngine;

namespace BallRunner.Gates
{
    public abstract class GateBase : MonoBehaviour
    {
        [SerializeField] protected int value = 5;

        private bool isConsumed;

        protected virtual void Start()
        {
            RefreshLabel();
        }

        protected abstract string BuildLabel();
        protected abstract void Apply(BallGroupController group);

        protected virtual void RefreshLabel()
        {
            // Optional: bind gate label to world-space text in a later phase.
        }

        public void Configure(int nextValue)
        {
            value = nextValue;
            isConsumed = false;
            RefreshLabel();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (isConsumed)
            {
                return;
            }

            var group = other.GetComponentInParent<BallGroupController>();
            if (group == null)
            {
                return;
            }

            isConsumed = true;
            Apply(group);
        }
    }
}
