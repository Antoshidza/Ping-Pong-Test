using Source.SaveLoad;
using UnityEngine;
using Zenject;

namespace Source.PingPong.Simulation
{
    public class BestScoreSaveLoadManager : ISaveLoadManager
    {
        private IScore _score;

        private const string ScorePrefsName = "PingPongScore";
        
        [Inject]
        public void Initialize(IScore score)
        {
            _score = score;
        }

        public void Save()
        {
            PlayerPrefs.SetInt(ScorePrefsName, Mathf.Max(_score.ScoreValue, _score.BestScoreValue));
        }

        public void Load()
            => _score.BestScoreValue = GetSavedScore();

        private int GetSavedScore()
            => PlayerPrefs.GetInt(ScorePrefsName);
    }
}