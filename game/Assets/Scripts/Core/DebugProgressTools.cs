using UnityEngine;

namespace BallRunner.Core
{
    public sealed class DebugProgressTools : MonoBehaviour
    {
        private const string LevelIndexKey = "ball_runner_level_index";
        private const string TutorialSeenKey = "ball_runner_tutorial_seen";
        private const string EconomySaveKey = "ball_runner_progress_v1";

        [ContextMenu("Reset Level Progress")]
        public void ResetLevelProgress()
        {
            PlayerPrefs.DeleteKey(LevelIndexKey);
            PlayerPrefs.Save();
            Debug.Log("[DebugProgressTools] Level progress reset.");
        }

        [ContextMenu("Reset Tutorial State")]
        public void ResetTutorialState()
        {
            PlayerPrefs.DeleteKey(TutorialSeenKey);
            PlayerPrefs.Save();
            Debug.Log("[DebugProgressTools] Tutorial state reset.");
        }

        [ContextMenu("Reset Economy Save")]
        public void ResetEconomySave()
        {
            PlayerPrefs.DeleteKey(EconomySaveKey);
            PlayerPrefs.Save();
            Debug.Log("[DebugProgressTools] Economy save reset.");
        }

        [ContextMenu("Reset All Gameplay Progress")]
        public void ResetAllGameplayProgress()
        {
            ResetLevelProgress();
            ResetTutorialState();
            ResetEconomySave();
            Debug.Log("[DebugProgressTools] All gameplay progress reset.");
        }
    }
}
