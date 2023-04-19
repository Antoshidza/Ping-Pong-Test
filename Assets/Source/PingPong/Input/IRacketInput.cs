using System;
using UnityEngine;

namespace Source.PingPong.Input
{
    public interface IRacketInput
    {
        public event Action<Vector2> OnMove;
    }
}