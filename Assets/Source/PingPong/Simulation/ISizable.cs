using System;

namespace Source.PingPong.Simulation
{
    public interface ISizable
    {
        public float Size { get; set; }

        public event Action<float> OnSizeChange;
    }
}