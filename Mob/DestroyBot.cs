using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Game
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class DestroyBot : ISystem
    {
        private readonly BotSpawner _botSpawner;
        // private Stash<BotDeathEvent> _botDeathEvent;
        private Stash<BotPrefabRef> _botPrefabRef;
        private Filter _filter;
        // private Filter _filterGroup;

        public DestroyBot(BotSpawner botSpawner)
        {
            _botSpawner = botSpawner;
        }

        public void Dispose()
        { }

        public void OnAwake()
        {
            _filter = World.Filter.With<BotMarker>().With<BotPrefabRef>().With<OneFrameMarker>();
            // _filterGroup = World.Filter.With<BotGroupDeathEvent>();
            
            // _botDeathEvent = World.GetStash<BotDeathEvent>();
            _botPrefabRef = World.GetStash<BotPrefabRef>();
            // World.GetStash<ChildEntities>().AsDisposable();
            // World.GetStash<MobDeathVfxSettings>().AsDisposable();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var botInstance = ref _botPrefabRef.Get(entity);
                _botSpawner.Push(botInstance.Value);

                // if (!entity.Has<OneFrameMarker>())
                // {
                //     entity.AddComponent<OneFrameMarker>();
                // }
            }

            // foreach (var entity in _filterGroup)
            // {
            //     ref var group = ref entity.GetComponent<BotGroupDeathEvent>();
            //
            //     for (var i = 0; i < group.Bots.Length; i++)
            //     {
            //         RemoveBot(group.Bots[i]);
            //     }
            // }
            //
            // void RemoveBot(Entity bot)
            // {
            //     if (bot.IsNullOrDisposed())
            //     {
            //         return;
            //     }
            //
            //    
            // }
        }
    }
}