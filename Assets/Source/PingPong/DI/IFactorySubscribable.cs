using System;
using Zenject;

namespace Source.PingPong.Simulation
{
    public interface IFactorySubscribable<out T> : IFactory<T>
    {
        public event Action<T> OnCreate;
    }
}