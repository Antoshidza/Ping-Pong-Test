using System;

namespace Source.PingPong.Simulation
{
    public interface IBounds
    {
        public event Action<IMovable> OnOutOfBounds;
    }
}