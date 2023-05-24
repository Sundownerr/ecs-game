using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Game
{
    public class BootstrapScope : LifetimeScope
    {
        [SerializeField] private SceneListSO _sceneListSo;
        [SerializeField] private CoroutineRunner _coroutineRunner;

        protected override void Configure(IContainerBuilder builder)
        {
            States(builder);

            builder.RegisterInstance(_coroutineRunner).AsImplementedInterfaces();
            builder.RegisterInstance(_sceneListSo).AsSelf().AsImplementedInterfaces();
            builder.Register<ILevelLoader, LevelLoader>(Lifetime.Singleton);
            builder.Register<ISceneLoader, SceneLoader>(Lifetime.Singleton);
           
            builder.RegisterEntryPoint<BootstrapEntryPoint>();
        }

        private static void States(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<StateMachine>().AsSelf();
            builder.Register<InMainMenu>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<LoadingLevel>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<PlayingGame>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<OnPlayerDeathScreen>(Lifetime.Singleton).AsImplementedInterfaces();
        }
    }
}