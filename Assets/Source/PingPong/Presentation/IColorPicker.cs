using System;
using UnityEngine;

namespace Source.PingPong.Presentation
{
    public interface IColorPicker
    {
        public event Action<Color> OnSelectColor;
    }
}