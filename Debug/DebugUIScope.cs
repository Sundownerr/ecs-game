using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Game
{
    public class DebugUIScope : LifetimeScope
    {
        [SerializeField] private DebugUI _debugUI;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(_debugUI);
            builder.RegisterEntryPoint<DebugUIPresenter>();
        }
    }
}