using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Game
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class ConstructDamageEventTarget : ISystem
    {
        private Filter _filter;
        private Stash<GameObjectRef> _gameObjectRef;
        private Stash<Target> _target;

        public void Dispose()
        { }

        public void OnAwake()
        {
            _filter = World.Filter.With<GameObjectRef>().With<DamageEvent>();

            _gameObjectRef = World.GetStash<GameObjectRef>();
            _target = World.GetStash<Target>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var gameObjectRef = ref _gameObjectRef.Get(entity);

                if (!gameObjectRef.Value.TryGetComponent(out EntityReference entityReference))
                {
                    continue;
                }

                if (entityReference.Entity.IsNullOrDisposed())
                {
                    continue;
                }

                ref var target = ref _target.Add(entity);
                target.Value = entityReference.Entity;
            }
        }
    }
}