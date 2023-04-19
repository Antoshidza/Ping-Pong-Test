using UnityEngine;

namespace Source.PingPong.Simulation
{
    public interface ITransformParent
    {
        public void AddChild(Transform child);
    }
}