using TMPro;
using UnityEngine;

namespace Source.PingPong.Presentation
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private TMP_Text _scoreLabel;

        public int BestScore
        {
            set => _scoreLabel.text = $"best score: {value}";
        }
    }
}