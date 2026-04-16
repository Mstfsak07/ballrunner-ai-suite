using UnityEngine;
using System;

namespace BallRunner.Economy
{
    public sealed class CurrencyManager : MonoBehaviour
    {
        [SerializeField] private SaveManager saveManager;

        public int Coin { get; private set; }
        public event Action<int> OnCoinChanged;

        public void Initialize(PlayerProgressData progress)
        {
            Coin = Mathf.Max(0, progress.coin);
            OnCoinChanged?.Invoke(Coin);
        }

        public void Add(int amount)
        {
            Coin = Mathf.Max(0, Coin + amount);
            OnCoinChanged?.Invoke(Coin);
        }

        public bool TrySpend(int amount)
        {
            if (amount <= 0)
            {
                return true;
            }

            if (Coin < amount)
            {
                return false;
            }

            Coin -= amount;
            OnCoinChanged?.Invoke(Coin);
            return true;
        }

        public void Persist(PlayerProgressData progress)
        {
            progress.coin = Coin;
            saveManager.Save(progress);
        }
    }
}
