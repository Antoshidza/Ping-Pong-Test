using System;

namespace Source.PingPong.Simulation
{
    public interface IAppStateChanger
    {
        public event Action<AppState.State> OnStateChangeRequest;
    }
}