using System;
using System.Collections.Generic;
using Source.PingPong.Input;
using Source.PingPong.Presentation;
using Source.SaveLoad;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Source.PingPong.Simulation
{
    public class LevelController : MonoBehaviour
    {
        [SerializeField] private int _scorePerBallReflect;
        [SerializeField] private Vector2 _racketXBounds = new Vector2(-1, 1f);
        [SerializeField] private float _racketInitialSize = 1f;
        
        [Header("Ball settings")]
        [SerializeField] private Vector3 _ballStartPosition;
        [SerializeField] private float _ballStartSpeed;
        [SerializeField] private float _ballRandomStartDirectionXYRelation;
        
        [Inject] private LevelPresenter _levelPresenter;
        [Inject] private IEnumerable<ISaveLoadManager> _saveLoadManagers;

        [Inject] private IEnumerable<IBall> _balls;
        [Inject] private IEnumerable<IRacket> _rackets;
        [Inject] private IRacketInput _racketInput;
        [Inject] private IEnumerable<IBounds> _bounds;
        [Inject] private IScore _score;

        private bool _isInitialized;

        public bool IsInitialized => _isInitialized;

        private void OnValidate()
        {
            if (_racketXBounds.x > _racketXBounds.y)
                _racketXBounds.x = _racketXBounds.y;
        }

        private void ClampPosition(IRacket racket)
        {
            var pos = racket.Position;
            var sizeHalf = racket.Size / 2f;
            pos.x = Mathf.Clamp(pos.x, _racketXBounds.x + sizeHalf, _racketXBounds.y - sizeHalf);

            racket.Position = pos;
        }
        
        private Vector3 GetRandomBallDirection()
        {
            var verticalDirection = Random.value < .5f ? Vector3.up : Vector3.down;
            var horizontalDirection = Random.value < .5f ? Vector3.right : Vector3.left;
            return Vector3.Lerp(horizontalDirection, verticalDirection, Random.Range(_ballRandomStartDirectionXYRelation, 1f));
        }
        
        public void Initialize()
        {
            foreach (var saveLoadManager in _saveLoadManagers)
                saveLoadManager.Load();
                
            _levelPresenter.Present();

            foreach (var racket in _rackets)
            {
                racket.Size = _racketInitialSize;
                _racketInput.OnMove += delta =>
                {
                    racket.Position += new Vector3(delta.x, 0f, 0f);
                    ClampPosition(racket);
                }; 
                racket.OnReflect += () => _score.ScoreValue += _scorePerBallReflect;
            }

            foreach (var bound in _bounds)
                bound.OnOutOfBounds += movable =>
                {
                    movable.Position = _ballStartPosition;
                    movable.Direction = GetRandomBallDirection();
                    
                    ResetScores();
                };

            _isInitialized = true;
        }

        public void StartGame()
        {
            ResetScores();
            
            foreach (var ball in _balls)
            {
                ball.Position = _ballStartPosition;
                ball.Direction = GetRandomBallDirection();
                ball.Speed = _ballStartSpeed;
            }
        }

        private void ResetScores()
        {
            _score.RecordBest();
            _score.ScoreValue = 0;
        }

        public void Save()
        {
            _score.RecordBest();
            
            foreach (var saveLoadManager in _saveLoadManagers)
                saveLoadManager.Save();
        }

        public void SetPause(bool value)
            => Time.timeScale = value ? 0f : 1f;

        public void SetGameVisible(bool value)
            => _levelPresenter.SetVisible(value);

        #region DrawGizmos
        #if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            DrawBounds();
            DrawBallData();
        }

        private void DrawBounds()
        {
            const float xBoundsLineLengthHalf = 5f;
            void DrawVerticalLineAt(float x)
                => Gizmos.DrawLine(new Vector3(x, -xBoundsLineLengthHalf, 0f), new Vector3(x, xBoundsLineLengthHalf, 0f));
            
            Gizmos.color = Color.red;
            DrawVerticalLineAt(_racketXBounds.x);
            DrawVerticalLineAt(_racketXBounds.y);
        }

        private void DrawBallData()
        {
            // draw start position
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(_ballStartPosition, .1f);
            
            // draw start possible directions
            Gizmos.color = Color.green;
            var pos = _ballStartPosition;
            var right = Vector2.right;
            var up = Vector2.up;
            var left = Vector2.left;
            var down = Vector2.down;
            var rightUpDir = (Vector3)Vector2.Lerp(right, up, _ballRandomStartDirectionXYRelation).normalized;
            var leftUpDir = (Vector3)Vector2.Lerp(left, up, _ballRandomStartDirectionXYRelation).normalized;
            var leftDownDir = (Vector3)Vector2.Lerp(left, down, _ballRandomStartDirectionXYRelation).normalized;
            var rightDownDir = (Vector3)Vector2.Lerp(right, down, _ballRandomStartDirectionXYRelation).normalized;
            Gizmos.DrawLine(pos, pos + rightUpDir);
            Gizmos.DrawLine(pos, pos + leftUpDir);
            Gizmos.DrawLine(pos, pos + leftDownDir);
            Gizmos.DrawLine(pos, pos + rightDownDir);

            static void DrawSketchBetween(in Vector3 center, in Vector3 pointA, in Vector3 pointB, int times)
            {
                for (int i = 0; i <= times; i++)
                {
                    var t = i / (float)times;
                    var from = Vector3.Lerp(center, pointA, t);
                    var to = Vector3.Lerp(center, pointB, t);
                    Gizmos.DrawLine(from, to);
                }
            }

            const int sketchDrawTimes = 10;
            DrawSketchBetween(pos, pos + rightUpDir, pos + leftUpDir, sketchDrawTimes);
            DrawSketchBetween(pos, pos + leftDownDir, pos + rightDownDir, sketchDrawTimes);
        }
        #endif
        #endregion
    }
}
