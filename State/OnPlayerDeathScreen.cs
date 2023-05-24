using Scellecs.Morpeh;

namespace Game
{
    public class OnPlayerDeathScreen : IState
    {
        private readonly ILevelLoader _levelLoader;
        private readonly SceneListSO _sceneListSo;
        private readonly ISceneLoader _sceneLoader;
        private readonly StateMachine _stateMachine;

        public OnPlayerDeathScreen(StateMachine stateMachine,
                                 ISceneLoader sceneLoader,
                                 SceneListSO sceneListSo,
                                 ILevelLoader levelLoader)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _sceneListSo = sceneListSo;
            _levelLoader = levelLoader;
        }

        public void Enter()
        {
            _sceneLoader.LoadAdditiveAsyncAll(_sceneListSo.PlayerDeathScenes);

            UIEvents.PlayerDeath.RestartPressed += OnRestartPressed;
            UIEvents.PlayerDeath.MenuPressed += OnMenuPressed;
        }

        public void Exit()
        {
            World.Default?.Dispose();
            _levelLoader.Unload(0);
            _sceneLoader.UnloadAsyncAll(_sceneListSo.PlayerDeathScenes);

            UIEvents.PlayerDeath.RestartPressed -= OnRestartPressed;
            UIEvents.PlayerDeath.MenuPressed -= OnMenuPressed;
        }

        private void OnMenuPressed()
        {
            _stateMachine.ChangeState<InMainMenu>();
        }

        private void OnRestartPressed()
        {
            _levelLoader.Unload(0, () => { _stateMachine.ChangeState<LoadingLevel>(); });
        }
    }
}