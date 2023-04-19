using System;
using Source.PingPong.Presentation;
using Source.SaveLoad;
using UnityEngine;
using Zenject;

namespace Source.PingPong.Simulation
{
    public class LevelInstaller : MonoInstaller
    {
        [SerializeField] private LevelController _levelController;

        [Header("Presentation")] 
        [SerializeField] private GameObject _sizableViewPrefab;
        [SerializeField] private GameObject _racketViewPrefab;
        [SerializeField] private GameObject _ballViewPrefab;
        
        public override void InstallBindings()
        {
            if(!Validate())
                return;
            
            InstallSimulation();
            InstallPresentation();
        }

        private bool Validate()
        {
            void LogNullFieldException(in string fieldName)
                => Debug.LogWarning(new NullReferenceException($"{nameof(LevelInstaller)}: {fieldName} is NULL"), gameObject);

            if(_sizableViewPrefab == null)
                LogNullFieldException(nameof(_sizableViewPrefab));
            if(_racketViewPrefab == null)
                LogNullFieldException(nameof(_racketViewPrefab));
            if(_ballViewPrefab == null)
                LogNullFieldException(nameof(_ballViewPrefab));
            
            return _sizableViewPrefab != null
                && _racketViewPrefab != null
                && _ballViewPrefab != null;
        }

        private void InstallSimulation()
        {
            Container.Bind<LevelController>().FromInstance(_levelController).AsSingle();

            Container.Bind<IScore>().To<Score>().AsSingle();
        }

        private void InstallPresentation()
        {
            Container.Bind<LevelPresenter>().AsSingle();
                
            Container.Bind<SimpleMeshView>().ToSelf().FromComponentOn(_sizableViewPrefab).AsCached();
            Container.Bind<SimpleMeshView>().WithId("RacketView").ToSelf().FromComponentOn(_racketViewPrefab).AsCached();
            Container.Bind<BallView>().ToSelf().FromComponentOn(_ballViewPrefab).AsCached();
            Container.Bind<BallViewSettings>().ToSelf().AsSingle();
            Container.Bind<ISaveLoadManager>().To<BallViewSaveLoadManager>().AsSingle();
        }
    }
}