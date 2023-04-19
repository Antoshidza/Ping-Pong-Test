using System;

namespace Source.PingPong.Simulation
{
    public interface IScore
    {
        public int ScoreValue { get; set; }
        public int BestScoreValue { get; set; }

        public event Action<int> OnScoreChange;
        public event Action<int> OnBestScoreChange;

        public void RecordBest()
        {
            if (ScoreValue > BestScoreValue)
                BestScoreValue = ScoreValue;
        }
    }
}