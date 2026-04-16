using BallRunner.Balls;
using BallRunner.Economy;
using UnityEngine;

namespace BallRunner.Level
{
    public sealed class FinishZone : MonoBehaviour
    {
        [SerializeField] private RunResultService runResultService;
        private bool consumed;

        private void OnTriggerEnter(Collider other)
        {
            if (consumed)
            {
                return;
            }

            var group = other.GetComponentInParent<BallGroupController>();
            if (group == null)
            {
                return;
            }

            consumed = true;
            if (runResultService != null)
            {
                runResultService.ApplyFinish(group.CurrentCount);
            }
        }
    }
}
