using Zenject;

namespace Source.PingPong.Presentation
{
    public class PauseMenuPresenter
    {
        [Inject] private PauseMenu _pauseMenu;
        
        public void SetOpen(bool value)
            => _pauseMenu.gameObject.SetActive(value);
    }
}