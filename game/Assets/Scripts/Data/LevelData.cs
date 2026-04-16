using System;
using UnityEngine;

namespace BallRunner.Data
{
    public enum GateType
    {
        Add = 0,
        Subtract = 1,
        Multiply = 2
    }

    public enum ObstacleType
    {
        SpinBar = 0,
        DamageWall = 1,
        Gap = 2
    }

    [Serializable]
    public struct GateSpawnData
    {
        public GateType gateType;
        public int value;
        public Vector3 position;
    }

    [Serializable]
    public struct ObstacleSpawnData
    {
        public ObstacleType obstacleType;
        public int damage;
        public Vector3 position;
    }

    [CreateAssetMenu(fileName = "LevelData", menuName = "BallRunner/Level/Level Data")]
    public sealed class LevelData : ScriptableObject
    {
        [SerializeField] private int levelId;
        [SerializeField] private int targetDurationSeconds = 30;
        [SerializeField] private int startBallCount = 5;
        [SerializeField] private GateSpawnData[] gates = Array.Empty<GateSpawnData>();
        [SerializeField] private ObstacleSpawnData[] obstacles = Array.Empty<ObstacleSpawnData>();

        public int LevelId => levelId;
        public int TargetDurationSeconds => targetDurationSeconds;
        public int StartBallCount => startBallCount;
        public GateSpawnData[] Gates => gates;
        public ObstacleSpawnData[] Obstacles => obstacles;
    }
}
