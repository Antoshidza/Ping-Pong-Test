using System;
using UnityEngine;

namespace Source.PingPong.Simulation
{
    public interface IRacket : ISizable, ITransformParent
    {
        public Vector3 Position { get; set; }
        public event Action OnReflect;
    }
}