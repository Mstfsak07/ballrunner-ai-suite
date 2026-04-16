using UnityEngine;

namespace BallRunner.Obstacles
{
    public sealed class SpinBarObstacle : ObstacleBase
    {
        [SerializeField] private Vector3 axis = Vector3.up;
        [SerializeField] private float speed = 120f;

        private void Update()
        {
            transform.Rotate(axis, speed * Time.deltaTime, Space.Self);
        }

        private void OnTriggerEnter(Collider other)
        {
            ApplyDamage(other);
        }
    }
}
