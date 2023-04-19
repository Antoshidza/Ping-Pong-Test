using System;
using UnityEngine;
using UnityEngine.UI;

namespace Source.PingPong.Simulation
{
    public class ButtonAppStateChanger : MonoBehaviour, IAppStateChanger
    {
        [SerializeField] private AppState.State _changeToState;
        [SerializeField] private Button _button;

        public event Action<AppState.State> OnStateChangeRequest;
        
        private void Start()
        {
            _button.onClick.AddListener(() => OnStateChangeRequest?.Invoke(_changeToState));
        }
    }
}