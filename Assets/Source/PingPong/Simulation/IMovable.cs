using UnityEngine;

namespace Source.PingPong
{
    public interface IMovable
    {
        public Vector3 Direction { get; set; }
        public float Speed { get; set; }
        public Vector3 Velocity { get; set; }
        public Vector3 Position { get; set; }
    }
}