using System;
using UnityEngine;
using UnityEngine.UI;

namespace Source.PingPong.Presentation
{
    public class ColorPickerButton : MonoBehaviour, IColorPicker
    {
        [SerializeField] private Color _color;
        [SerializeField] private Button _button;
        [SerializeField] private Image _buttonImage;
        
        public event Action<Color> OnSelectColor;

        private void OnValidate()
        {
            if(_buttonImage == null)
                return;
            
            _buttonImage.color = _color;
        }

        private void Start()
        {
            _button.onClick.AddListener(() => OnSelectColor?.Invoke(_color));
        }
    }
}