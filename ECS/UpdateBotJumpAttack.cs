using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

namespace Game
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class UpdateBotJumpAttack : ISystem
    {
        private Filter _filter;

        private Stash<Gravity> _gravityStash;
        private Stash<InitialPosition> _initialPosition;
        private Stash<MoveDirection> _moveDirection;

        private Stash<NavMeshAgentRef> _navMeshAgentRef;
        private Stash<ParentEntity> _parentEntity;

        private Stash<TransformRef> _transformRef;

        public void Dispose()
        { }

        public void OnAwake()
        {
            _filter = World.Filter.With<BotJumpAttackAnimation>().With<TransformRef>().With<NavMeshAgentRef>()
                .With<MoveDirection>().With<Gravity>().With<InitialPosition>().With<ParentEntity>();

            _transformRef = World.GetStash<TransformRef>();
            _navMeshAgentRef = World.GetStash<NavMeshAgentRef>();
            _moveDirection = World.GetStash<MoveDirection>();
            _gravityStash = World.GetStash<Gravity>();
            _initialPosition = World.GetStash<InitialPosition>();
            _parentEntity = World.GetStash<ParentEntity>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var transformRef = ref _transformRef.Get(entity);
                ref var moveDirection = ref _moveDirection.Get(entity);
                ref var gravity = ref _gravityStash.Get(entity);
                ref var initialPosition = ref _initialPosition.Get(entity);

                moveDirection.Value += Vector3.up * gravity.Value * deltaTime;
                transformRef.Value.position += moveDirection.Value * deltaTime;

                if (!Physics.Raycast(transformRef.Value.position, Vector3.down, out var hit, 0.5f, 1 << 6))
                {
                    continue;
                }

                if (Vector3.Distance(hit.point, initialPosition.Value) < 2f)
                {
                    continue;
                }

                entity.AddComponent<OneFrameMarker>();
                ref var navMeshAgent = ref _navMeshAgentRef.Get(entity);

                var navMeshSearchRadius = 1f;
                NavMeshHit navSample;

                while (!NavMesh.SamplePosition(hit.point, out navSample, navMeshSearchRadius, NavMesh.AllAreas))
                {
                    navMeshSearchRadius += 1f;
                }

                transformRef.Value.position = navSample.position;
                navMeshAgent.Value.enabled = true;

                if (_parentEntity.Get(entity).Value.Has<CantMoveOnNavMesh>())
                {
                    _parentEntity.Get(entity).Value.RemoveComponent<CantMoveOnNavMesh>();
                }

                var player = World.Filter.With<PlayerMarker>().With<PlayerMoveSpeedSettings>().First();
                ref var playerPosition = ref player.GetComponent<WorldPosition>();
                ref var damageRadius = ref entity.GetComponent<Radius>();

                if (Vector3.Distance(playerPosition.Value, transformRef.Value.position) > damageRadius.Value)
                {
                    continue;
                }

                var damageEvent = World.CreateDamageEvent(entity.GetComponent<Damage>().Value);
                
                ref var target = ref damageEvent.AddComponent<Target>();
                target.Value = player;
            }
        }
    }
}