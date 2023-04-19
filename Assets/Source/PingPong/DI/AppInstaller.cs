using System;
using Source.PingPong.Presentation;
using Source.SaveLoad;
using UnityEngine;
using Zenject;

namespace Source.PingPong.Simulation
{
    public class AppInstaller : MonoInstaller
    {
        [SerializeField] private AppController _appController;
        
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

            if (_appController == null)
                LogNullFieldException(nameof(_appController));
            
            return _appController != null;
        }

        private void InstallSimulation()
        {
            Container.Bind<AppState>().AsSingle();
            Container.Bind<AppController>().FromInstance(_appController).AsSingle();
            Container.Bind<ISaveLoadManager>().To<BestScoreSaveLoadManager>().AsSingle().WhenInjectedInto<AppController>();
        }

        private void InstallPresentation()
        {
            Container.Bind<MainMenuPresenter>().AsSingle();
            Container.Bind<PauseMenuPresenter>().AsSingle();
        }
    }
}