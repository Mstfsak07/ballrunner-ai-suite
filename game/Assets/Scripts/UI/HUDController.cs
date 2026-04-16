using UnityEngine;
using UnityEngine.UI;

namespace BallRunner.UI
{
    public sealed class HUDController : MonoBehaviour
    {
        [SerializeField] private Text ballCountText;
        [SerializeField] private Slider progressSlider;
        [SerializeField] private Text coinText;
        [SerializeField] private Text levelText;

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

        public void SetLevel(int levelIndex, int totalLevelCount)
        {
            if (levelText == null)
            {
                return;
            }

            var safeTotal = Mathf.Max(1, totalLevelCount);
            var displayLevel = Mathf.Clamp(levelIndex + 1, 1, safeTotal);
            levelText.text = $"Level {displayLevel}/{safeTotal}";
        }
    }
}
