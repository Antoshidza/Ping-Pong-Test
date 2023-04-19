using System;
using Source.PingPong.Input;
using UnityEngine;

public class RacketInput : MonoBehaviour, IRacketInput
{
    [SerializeField] private float _sensitivity;

    private RacketControls _racketControls;

    public event Action<Vector2> OnMove;
    
    private void Start()
    {
        _racketControls = new();
        _racketControls.Enable();

        _racketControls.Racket.Move.performed += context => OnMove?.Invoke(context.ReadValue<Vector2>() * _sensitivity * Time.deltaTime);
    }
}
