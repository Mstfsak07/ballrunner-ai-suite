using UnityEngine;

namespace BallRunner.Economy
{
    public sealed class SaveManager : MonoBehaviour
    {
        private const string SaveKey = "ball_runner_progress_v1";

        public PlayerProgressData Load()
        {
            if (!PlayerPrefs.HasKey(SaveKey))
            {
                return new PlayerProgressData();
            }

            var json = PlayerPrefs.GetString(SaveKey);
            return string.IsNullOrWhiteSpace(json)
                ? new PlayerProgressData()
                : JsonUtility.FromJson<PlayerProgressData>(json);
        }

        public void Save(PlayerProgressData data)
        {
            var json = JsonUtility.ToJson(data);
            PlayerPrefs.SetString(SaveKey, json);
            PlayerPrefs.Save();
        }
    }
}
