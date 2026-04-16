using System.Collections.Generic;
using UnityEngine;

namespace BallRunner.Balls
{
    public sealed class BallPool : MonoBehaviour
    {
        [SerializeField] private BallUnit prefab;
        [SerializeField] private Transform container;
        [SerializeField] private int prewarmCount = 40;

        private readonly Queue<BallUnit> inactive = new();

        private void Awake()
        {
            for (var i = 0; i < prewarmCount; i++)
            {
                var item = Create();
                item.gameObject.SetActive(false);
                inactive.Enqueue(item);
            }
        }

        public BallUnit Get(Transform parent)
        {
            var item = inactive.Count > 0 ? inactive.Dequeue() : Create();
            item.transform.SetParent(parent, false);
            item.gameObject.SetActive(true);
            return item;
        }

        public void Release(BallUnit item)
        {
            if (item == null)
            {
                return;
            }

            item.gameObject.SetActive(false);
            item.transform.SetParent(container == null ? transform : container, false);
            inactive.Enqueue(item);
        }

        private BallUnit Create()
        {
            return Instantiate(prefab, container == null ? transform : container);
        }
    }
}
