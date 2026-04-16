using System.Collections.Generic;
using BallRunner.Core;
using UnityEngine;

namespace BallRunner.Balls
{
    public sealed class BallGroupController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private BallUnit ballPrefab;
        [SerializeField] private BallPool pool;
        [SerializeField] private Transform ballParent;

        [Header("Layout")]
        [SerializeField] private float spacing = 0.45f;
        [SerializeField] private int rowWidth = 8;

        [Header("Defaults")]
        [SerializeField] private int initialCount = 5;
        [SerializeField] private int fallbackMaxCount = 80;

        private readonly List<BallUnit> activeBalls = new();

        public int CurrentCount => activeBalls.Count;
        public int MaxCount { get; private set; }

        private void Awake()
        {
            MaxCount = RuntimeConfigInstaller.GameConfig != null
                ? RuntimeConfigInstaller.GameConfig.MaxVisibleBalls
                : fallbackMaxCount;

            Initialize(initialCount);
        }

        public void Initialize(int count)
        {
            ClearAll();
            var clamped = Mathf.Clamp(count, 0, MaxCount);
            Spawn(clamped);
            RebuildFormation();
        }

        public int ApplyDelta(int delta)
        {
            if (delta == 0)
            {
                return CurrentCount;
            }

            if (delta > 0)
            {
                Spawn(delta);
            }
            else
            {
                Despawn(-delta);
            }

            RebuildFormation();
            return CurrentCount;
        }

        public int ApplyMultiplier(float multiplier)
        {
            var target = Mathf.RoundToInt(CurrentCount * multiplier);
            var clamped = Mathf.Clamp(target, 0, MaxCount);
            ApplyDelta(clamped - CurrentCount);
            return CurrentCount;
        }

        public bool TryConsume(int amount)
        {
            if (amount <= 0)
            {
                return true;
            }

            if (CurrentCount < amount)
            {
                return false;
            }

            Despawn(amount);
            RebuildFormation();
            return true;
        }

        private void Spawn(int amount)
        {
            var allowed = Mathf.Min(amount, MaxCount - CurrentCount);
            for (var i = 0; i < allowed; i++)
            {
                var unit = pool != null
                    ? pool.Get(ballParent == null ? transform : ballParent)
                    : Instantiate(ballPrefab, ballParent == null ? transform : ballParent);
                activeBalls.Add(unit);
            }
        }

        private void Despawn(int amount)
        {
            var removeCount = Mathf.Min(amount, CurrentCount);
            for (var i = 0; i < removeCount; i++)
            {
                var index = activeBalls.Count - 1;
                var unit = activeBalls[index];
                activeBalls.RemoveAt(index);
                if (pool != null)
                {
                    pool.Release(unit);
                }
                else if (unit != null)
                {
                    Destroy(unit.gameObject);
                }
            }
        }

        private void RebuildFormation()
        {
            var count = activeBalls.Count;
            for (var i = 0; i < count; i++)
            {
                var col = i % rowWidth;
                var row = i / rowWidth;

                var x = (col - (rowWidth - 1) * 0.5f) * spacing;
                var z = -row * spacing;
                activeBalls[i].transform.localPosition = new Vector3(x, 0f, z);
            }
        }

        private void ClearAll()
        {
            for (var i = activeBalls.Count - 1; i >= 0; i--)
            {
                var unit = activeBalls[i];
                if (pool != null)
                {
                    pool.Release(unit);
                }
                else if (unit != null)
                {
                    Destroy(unit.gameObject);
                }
            }

            activeBalls.Clear();
        }
    }
}
