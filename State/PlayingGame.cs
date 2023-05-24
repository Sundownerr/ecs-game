namespace Game
{
    public class PlayingGame : IState
    {
        private readonly ILevelLoader _levelLoader;
        private readonly SceneListSO _sceneListSo;
        private readonly ISceneLoader _sceneLoader;
        private readonly StateMachine _stateMachine;

        public PlayingGame(StateMachine stateMachine,
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
            _sceneLoader.LoadAdditiveAsyncAll(_sceneListSo.GameplayScenes);
            
            GameplayEvents.PlayerDied += OnGameFinished;
        }

        public void Exit()
        {
            _sceneLoader.UnloadAsyncAll(_sceneListSo.GameplayScenes);
            
            GameplayEvents.PlayerDied -= OnGameFinished;
        }

        private void OnGameFinished()
        {
            _stateMachine.ChangeState<OnPlayerDeathScreen>();
        }
    }
}