using BallRunner.Balls;
using UnityEngine;

namespace BallRunner.Obstacles
{
    public abstract class ObstacleBase : MonoBehaviour
    {
        [SerializeField] protected int damage = 1;

        public void ConfigureDamage(int nextDamage)
        {
            damage = Mathf.Max(0, nextDamage);
        }

        protected void ApplyDamage(Collider other)
        {
            var group = other.GetComponentInParent<BallGroupController>();
            if (group == null)
            {
                return;
            }

            group.TryConsume(Mathf.Max(0, damage));
        }
    }
}
