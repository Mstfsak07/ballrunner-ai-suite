using UnityEngine;

namespace BallRunner.Core
{
    [CreateAssetMenu(fileName = "EconomyConfig", menuName = "BallRunner/Config/Economy Config")]
    public sealed class EconomyConfig : ScriptableObject
    {
        [Header("Level Rewards")]
        [SerializeField] private int baseLevelCompleteCoin = 20;
        [SerializeField] private int baseFinishScoreCoin = 1;

        [Header("Upgrade Costs")]
        [SerializeField] private int startBallsLevel1Cost = 50;
        [SerializeField] private int startBallsLevel2Cost = 100;
        [SerializeField] private int startBallsLevel3Cost = 200;
        [SerializeField] private int coinBonusLevel1Cost = 50;
        [SerializeField] private int coinBonusLevel2Cost = 100;
        [SerializeField] private int coinBonusLevel3Cost = 200;

        public int BaseLevelCompleteCoin => baseLevelCompleteCoin;
        public int BaseFinishScoreCoin => baseFinishScoreCoin;
        public int StartBallsLevel1Cost => startBallsLevel1Cost;
        public int StartBallsLevel2Cost => startBallsLevel2Cost;
        public int StartBallsLevel3Cost => startBallsLevel3Cost;
        public int CoinBonusLevel1Cost => coinBonusLevel1Cost;
        public int CoinBonusLevel2Cost => coinBonusLevel2Cost;
        public int CoinBonusLevel3Cost => coinBonusLevel3Cost;
    }
}
