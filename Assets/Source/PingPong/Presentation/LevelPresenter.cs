using System.Collections.Generic;
using System.Linq;
using Source.PingPong.Simulation;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace Source.PingPong.Presentation
{
    public class LevelPresenter
    {
        [Inject] private IEnumerable<IRacket> _rackets;
        [Inject(Id = "RacketView")] private SimpleMeshView _racketView;
        
        [Inject] private IEnumerable<IBall> _balls;
        [Inject] private BallView _ballView;
        [Inject(Id = "BallColorPicker")] private IEnumerable<IColorPicker> _ballColorPickers;
        [Inject] private BallViewSettings _ballViewSettings;

        [Inject] private IScore _score;
        [Inject] private IScoreView _scoreView;

        [Inject] private List<IVisible> _visibles;

        public void Present()
        {
            PresentRacket();
            PresentBall();
            PresentScore();
        }

        private void PresentRacket()
        {
            foreach (var racket in _rackets)
            {
                var view = Object.Instantiate(_racketView);
                _visibles.Add(view);
                racket.AddChild(view.transform);
                view.transform.localPosition = Vector3.zero;

                void CopySize() => view.Size = new Vector3(racket.Size, 1f, 1f);

                CopySize();
                racket.OnSizeChange += _ => CopySize();
            }
        }

        private void PresentBall()
        {
            var ballViews = new List<SimpleMeshView>(_balls.Count());
            
            foreach (var ball in _balls)
            {
                var view = Object.Instantiate(_ballView);
                _visibles.Add(view);
                ballViews.Add(view);
                ball.AddChild(view.transform);
                view.transform.localPosition = Vector3.zero;
            }

            foreach (var colorPicker in _ballColorPickers)
                colorPicker.OnSelectColor += color => _ballViewSettings.Color = color;

            void ApplyColorToBalls()
            {
                foreach (var ballView in ballViews)
                    ballView.Color = _ballViewSettings.Color;
            }

            _ballViewSettings.OnColorChange += _ => ApplyColorToBalls();
            ApplyColorToBalls();
        }

        private void PresentScore()
        {
            _scoreView.Score = _score.ScoreValue;
            _scoreView.BestScore = _score.BestScoreValue;
            _score.OnScoreChange += score => _scoreView.Score = score;
            _score.OnBestScoreChange += score => _scoreView.BestScore = score;
        }

        public void SetVisible(bool value)
        {
            foreach (var visible in _visibles)
                visible.SetVisible(value);
        }
    }
}