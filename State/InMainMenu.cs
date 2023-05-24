using UnityEngine;

namespace Game
{
    public class InMainMenu : IState
    {
        private readonly SceneListSO _sceneListSo;
        private readonly ISceneLoader _sceneLoader;
        private readonly StateMachine _stateMachine;

        public InMainMenu(StateMachine stateMachine, ISceneLoader sceneLoader, SceneListSO sceneListSo)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _sceneListSo = sceneListSo;
        }

        public void Enter()
        {
            UIEvents.MainMenu.PlayPressed += OnPlayPressed;
            UIEvents.MainMenu.QuitPressed += OnQuitPressed;
            _sceneLoader.LoadAdditiveAsyncAll(_sceneListSo.StartScenes);
        }

        public void Exit()
        {
            _sceneLoader.UnloadAsyncAll(_sceneListSo.StartScenes);
            UIEvents.MainMenu.PlayPressed -= OnPlayPressed;
            UIEvents.MainMenu.QuitPressed -= OnQuitPressed;
        }

        private void OnQuitPressed()
        {
            Application.Quit();
        }

        private void OnPlayPressed()
        {
            _stateMachine.ChangeState<LoadingLevel>();
        }
    }
}