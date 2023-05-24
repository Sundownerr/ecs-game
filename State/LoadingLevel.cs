using UnityEngine.SceneManagement;

namespace Game
{
    public class LoadingLevel : IState
    {
        private readonly ILevelLoader _levelLoader;
        private readonly ISceneLoader _sceneLoader;
        private readonly SceneListSO _scenes;
        private readonly StateMachine _stateMachine;

        public LoadingLevel(StateMachine stateMachine,
                         ILevelLoader levelLoader,
                         ISceneLoader sceneLoader,
                         SceneListSO scenes)
        {
            _stateMachine = stateMachine;
            _levelLoader = levelLoader;
            _sceneLoader = sceneLoader;
            _scenes = scenes;
        }

        public void Enter()
        {
            _levelLoader.Load(0, level =>
            {
                _stateMachine.ChangeState<PlayingGame>();
                SceneManager.SetActiveScene(level);
            });
        }

        public void Exit()
        { }
    }
}