using UnityEngine;

namespace BallRunner.Player
{
    public sealed class FollowTargetCamera : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private Vector3 offset = new Vector3(0f, 7f, -9f);
        [SerializeField] private float followLerp = 10f;

        private void LateUpdate()
        {
            if (target == null)
            {
                return;
            }

            var desired = target.position + offset;
            transform.position = Vector3.Lerp(transform.position, desired, followLerp * Time.deltaTime);
        }
    }
}
