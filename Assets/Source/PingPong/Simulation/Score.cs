using System;
using UnityEngine;

namespace Source.PingPong.Simulation
{
    public class Score : IScore
    {
        private int _value;
        private int _bestValue;

        public int ScoreValue
        {
            get => _value;
            set
            {
                if(_value == value)
                    return;

                _value = value;
                OnScoreChange?.Invoke(_value);
            }
        }
        
        public int BestScoreValue
        {
            get => _bestValue;
            set
            {
                if(_bestValue == value)
                    return;

                _bestValue = value;
                OnBestScoreChange?.Invoke(_bestValue);
            }
        }
        
        public event Action<int> OnScoreChange;
        public event Action<int> OnBestScoreChange;
    }
}