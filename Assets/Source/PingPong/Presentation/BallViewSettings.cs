using System;
using UnityEngine;

namespace Source.PingPong.Presentation
{
    public class BallViewSettings
    {
        private Color _color;
        
        public Color Color
        { 
            get => _color;
            set
            {
                if(_color == value)
                    return;

                _color = value;
                OnColorChange?.Invoke(_color);
            }
        }

        public event Action<Color> OnColorChange;
    }
}