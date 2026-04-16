using System.Collections.Generic;
using UnityEngine;

namespace BallRunner.Obstacles
{
    public sealed class DamageWallObstacle : ObstacleBase
    {
        private readonly HashSet<int> processedGroups = new();

        private void OnTriggerEnter(Collider other)
        {
            var groupRoot = other.transform.root.gameObject;
            var id = groupRoot.GetInstanceID();
            if (!processedGroups.Add(id))
            {
                return;
            }

            ApplyDamage(other);
        }

        private void OnTriggerExit(Collider other)
        {
            var groupRoot = other.transform.root.gameObject;
            processedGroups.Remove(groupRoot.GetInstanceID());
        }
    }
}
