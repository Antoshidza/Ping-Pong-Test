using TMPro;
using UnityEngine;

namespace Source.PingPong.Presentation
{
    public class ScoreView : MonoBehaviour, IScoreView
    {
        [SerializeField] private TMP_Text _scoreLabel;
        [SerializeField] private TMP_Text _bestScoreLabel;
        
        public int Score
        {
            set => _scoreLabel.text = value.ToString();
        }

        public int BestScore
        {
            set => _bestScoreLabel.text = $"best: {value}";
        }
    }
}