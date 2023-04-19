using System;
using UnityEngine;

namespace Source.PingPong.Simulation
{
    public class Bounds : MonoBehaviour, IBounds
    {
        public event Action<IMovable> OnOutOfBounds;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.TryGetMovables(out var movables))
                return;

            foreach (var movable in movables)
                OnOutOfBounds?.Invoke(movable);
        }
    }
}