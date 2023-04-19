using System;
using System.Collections.Generic;
using Source.PingPong.Presentation;
using Source.SaveLoad;
using UnityEngine;
using Zenject;

namespace Source.PingPong.Simulation
{
    public class AppController : MonoBehaviour
    {
        private IEnumerable<ISaveLoadManager> _saveLoadManagers;
        private LevelController _levelController;
        private AppState _appState;
        
        [Inject]
        private void Initialize(
            AppState appState,
            IEnumerable<IAppStateChanger> appStateChangers,
            IEnumerable<ISaveLoadManager> saveLoadManagers,
            LevelController levelController,
            MainMenuPresenter mainMenuPresenter,
            PauseMenuPresenter pauseMenuPresenter
            )
        {
            _appState = appState;
            _levelController = levelController;
            _saveLoadManagers = saveLoadManagers;
            
            foreach (var loadManager in _saveLoadManagers)
                loadManager.Load();

            foreach (var stateChanger in appStateChangers)
                stateChanger.OnStateChangeRequest += state => appState.CurrentState = state;

            appState.OnStateChange += (prevState, state) =>
            {
                void HandleState()
                {
                    bool CheckStateTransition(AppState.State from, AppState.State to)
                    => prevState == from && state == to;

                    void StartNewGame()
                    {
                        levelController.SetGameVisible(true);
                        levelController.SetPause(false);   
                        levelController.StartGame();
                    }

                    if (CheckStateTransition(AppState.State.MainMenu, AppState.State.Play))
                    {
                        if(!levelController.IsInitialized)
                            levelController.Initialize();
                        StartNewGame();
                        
                        return;
                    }

                    if (CheckStateTransition(AppState.State.Paused, AppState.State.Restart))
                    {
                        Save();
                        appState.CurrentState = AppState.State.Play;

                        return;
                    }
                    
                    if (CheckStateTransition(AppState.State.Restart, AppState.State.Play))
                    {
                        StartNewGame();
                        
                        return;
                    }

                    if (CheckStateTransition(AppState.State.Play, AppState.State.Paused))
                    {
                        levelController.SetPause(true);
                        
                        return;
                    }

                    if (CheckStateTransition(AppState.State.Paused, AppState.State.Play))
                    {
                        levelController.SetPause(false);
                        
                        return;
                    }
                    
                    if (CheckStateTransition(AppState.State.Paused, AppState.State.MainMenu))
                    {
                        Save();
                        levelController.SetGameVisible(false);
                        
                        return;
                    }

                    Debug.LogError(new Exception($"{nameof(AppController)}: unexpected state transition {prevState} -> {state}"));
                }
                
                HandleState();
                
                mainMenuPresenter.SetOpen(state == AppState.State.MainMenu);
                pauseMenuPresenter.SetOpen(state == AppState.State.Paused);
            };

            appState.CurrentState = AppState.State.MainMenu;
            mainMenuPresenter.SetOpen(true);
            _levelController.SetGameVisible(false);
        }

        private void Save()
        {
            foreach (var saveLoadManager in _saveLoadManagers)
                saveLoadManager.Save();
            _levelController.Save();
        }

        private void SaveIfGameInProgress()
        {
            if (_appState.CurrentState != AppState.State.MainMenu)
                Save();
        }

        private void OnApplicationPause(bool paused)
        {
            if(paused)
                SaveIfGameInProgress();
        }

        private void OnApplicationQuit()
            => SaveIfGameInProgress();
    }
}