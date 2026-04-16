using System;
using System.Collections.Generic;
using BallRunner.Data;
using BallRunner.Gates;
using BallRunner.Obstacles;
using UnityEngine;

namespace BallRunner.Level
{
    [Serializable]
    public struct GatePrefabEntry
    {
        public GateType gateType;
        public GateBase prefab;
    }

    [Serializable]
    public struct ObstaclePrefabEntry
    {
        public ObstacleType obstacleType;
        public ObstacleBase prefab;
    }

    public sealed class LevelRuntimeBuilder : MonoBehaviour
    {
        [Header("Sources")]
        [SerializeField] private LevelCatalog levelCatalog;
        [SerializeField] private int levelIndex;
        [SerializeField] private bool buildOnStart = true;

        [Header("Prefab Registries")]
        [SerializeField] private GatePrefabEntry[] gatePrefabs = Array.Empty<GatePrefabEntry>();
        [SerializeField] private ObstaclePrefabEntry[] obstaclePrefabs = Array.Empty<ObstaclePrefabEntry>();

        [Header("Parents")]
        [SerializeField] private Transform gatesRoot;
        [SerializeField] private Transform obstaclesRoot;

        private readonly List<GameObject> spawned = new();

        private void Start()
        {
            if (buildOnStart)
            {
                BuildCurrentLevel();
            }
        }

        public void BuildCurrentLevel()
        {
            if (levelCatalog == null)
            {
                Debug.LogError("[LevelRuntimeBuilder] LevelCatalog is not assigned.");
                return;
            }

            Build(levelCatalog.GetLevel(levelIndex));
        }

        public void Build(LevelData data)
        {
            ClearSpawned();
            if (data == null)
            {
                Debug.LogWarning("[LevelRuntimeBuilder] LevelData is null.");
                return;
            }

            BuildGates(data.Gates);
            BuildObstacles(data.Obstacles);
        }

        public void SetLevelIndex(int index)
        {
            levelIndex = Mathf.Max(0, index);
        }

        private void BuildGates(GateSpawnData[] gates)
        {
            foreach (var gateData in gates)
            {
                var prefab = FindGatePrefab(gateData.gateType);
                if (prefab == null)
                {
                    continue;
                }

                var instance = Instantiate(prefab, gateData.position, Quaternion.identity, gatesRoot);
                instance.Configure(gateData.value);

                if (instance is MultiplyGate multiplyGate)
                {
                    multiplyGate.ConfigureMultiplier(Mathf.Max(1f, gateData.value));
                }

                spawned.Add(instance.gameObject);
            }
        }

        private void BuildObstacles(ObstacleSpawnData[] obstacles)
        {
            foreach (var obstacleData in obstacles)
            {
                var prefab = FindObstaclePrefab(obstacleData.obstacleType);
                if (prefab == null)
                {
                    continue;
                }

                var instance = Instantiate(prefab, obstacleData.position, Quaternion.identity, obstaclesRoot);
                instance.ConfigureDamage(obstacleData.damage);
                spawned.Add(instance.gameObject);
            }
        }

        private GateBase FindGatePrefab(GateType gateType)
        {
            foreach (var entry in gatePrefabs)
            {
                if (entry.gateType == gateType)
                {
                    return entry.prefab;
                }
            }

            Debug.LogWarning($"[LevelRuntimeBuilder] Missing gate prefab for {gateType}");
            return null;
        }

        private ObstacleBase FindObstaclePrefab(ObstacleType obstacleType)
        {
            foreach (var entry in obstaclePrefabs)
            {
                if (entry.obstacleType == obstacleType)
                {
                    return entry.prefab;
                }
            }

            Debug.LogWarning($"[LevelRuntimeBuilder] Missing obstacle prefab for {obstacleType}");
            return null;
        }

        private void ClearSpawned()
        {
            for (var i = spawned.Count - 1; i >= 0; i--)
            {
                var item = spawned[i];
                if (item != null)
                {
                    Destroy(item);
                }
            }

            spawned.Clear();
        }
    }
}
