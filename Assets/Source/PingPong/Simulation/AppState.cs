using System;

namespace Source.PingPong.Simulation
{
    public class AppState
    {
        public enum State
        {
            MainMenu,
            Paused,
            Play,
            Restart,
            GameOver   
        }

        private State _state;

        public State CurrentState
        {
            get => _state;
            set
            {
                if(_state == value)
                    return;
                
                var prevState = _state;
                _state = value;
                OnStateChange?.Invoke(prevState, _state);
            }
        }

        public event Action<State, State> OnStateChange;
    }
}