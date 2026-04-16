using System.Collections.Generic;
using BallRunner.Data;
using UnityEngine;

namespace BallRunner.Level
{
    public sealed class LevelCatalog : MonoBehaviour
    {
        [SerializeField] private List<LevelData> levels = new();

        public int LevelCount => levels.Count;

        public LevelData GetLevel(int index)
        {
            if (levels.Count == 0)
            {
                return null;
            }

            var safeIndex = Mathf.Clamp(index, 0, levels.Count - 1);
            return levels[safeIndex];
        }
    }
}
