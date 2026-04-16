using UnityEngine;
using UnityEngine.UI;

namespace BallRunner.UI
{
    public sealed class HUDController : MonoBehaviour
    {
        [SerializeField] private Text ballCountText;
        [SerializeField] private Slider progressSlider;
        [SerializeField] private Text coinText;

        public void SetBallCount(int count)
        {
            if (ballCountText != null)
            {
                ballCountText.text = $"Balls: {count}";
            }
        }

        public void SetProgress(float normalized)
        {
            if (progressSlider != null)
            {
                progressSlider.value = Mathf.Clamp01(normalized);
            }
        }

        public void SetCoin(int amount)
        {
            if (coinText != null)
            {
                coinText.text = $"Coin: {amount}";
            }
        }
    }
}
