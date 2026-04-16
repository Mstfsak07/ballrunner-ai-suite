using UnityEngine;

namespace BallRunner.Balls
{
    public sealed class BallUnit : MonoBehaviour
    {
        [SerializeField] private Renderer cachedRenderer;

        public void SetVisual(Color color)
        {
            if (cachedRenderer == null)
            {
                return;
            }

            cachedRenderer.material.color = color;
        }
    }
}
