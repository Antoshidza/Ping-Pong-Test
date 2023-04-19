using Source.PingPong.Simulation;
using Zenject;

namespace Source.PingPong.Presentation
{
    public class MainMenuPresenter
    {
        [Inject] private MainMenu _mainMenu;
        [Inject] private IScore _score;
        
        public void SetOpen(bool value)
        {
            _mainMenu.gameObject.SetActive(value);

            if (value)
                _mainMenu.BestScore = _score.BestScoreValue;
        }
    }
}