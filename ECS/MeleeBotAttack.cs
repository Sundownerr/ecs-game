using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using Unity.Mathematics;
using Random = UnityEngine.Random;

namespace Game
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class MeleeBotAttack : ISystem
    {
        private Stash<BotJumpAttackAbility> _botJumpAttackAbility;
        private Stash<ChildEntities> _childEntities;

        private Stash<DistanceToTarget> _distanceToTarget;
        private Filter _filter;
        private Stash<GameObjectRef> _gameObjectRef;
        private Stash<Health> _healthStash;
        private Stash<IsUsable> _isUsable;
        private Stash<NavMeshAgentRef> _navMeshAgentRef;
        private Stash<ParentEntity> _parentEntity;
        private Stash<Target> _target;
        private Stash<TransformRef> _transformRef;
        private Stash<WorldPosition> _worldPositionStash;

        public void Dispose()
        { }

        public void OnAwake()
        {
            _filter = World.Filter.With<ParentEntity>().With<BotJumpAttackAbility>();

            _distanceToTarget = World.GetStash<DistanceToTarget>();
            _target = World.GetStash<Target>();
            _healthStash = World.GetStash<Health>();
            _isUsable = World.GetStash<IsUsable>();
            _botJumpAttackAbility = World.GetStash<BotJumpAttackAbility>();
            _gameObjectRef = World.GetStash<GameObjectRef>();
            _parentEntity = World.GetStash<ParentEntity>();
            _transformRef = World.GetStash<TransformRef>();
            _navMeshAgentRef = World.GetStash<NavMeshAgentRef>();
            _worldPositionStash = World.GetStash<WorldPosition>();
            _childEntities = World.GetStash<ChildEntities>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            // if (_filter.GetLengthSlow() > 0)
            // {
            //     Debug.Log(" ");
            // }

            foreach (var entity in _filter)
            {
                ref var parentBot = ref _parentEntity.Get(entity);

                if (parentBot.Value.Has<CantMoveOnNavMesh>())
                {
                    continue;
                }

                ref var isUsable = ref _isUsable.Get(entity);

                if (!isUsable.Value)
                {
                    continue;
                }

                ref var distanceToTarget = ref _distanceToTarget.Get(parentBot.Value);
                ref var jumpAttack = ref _botJumpAttackAbility.Get(entity);

                if (distanceToTarget.Value > jumpAttack.UseRange)
                {
                    continue;
                }

                ref var target = ref _target.Get(parentBot.Value);

                if (!_healthStash.Has(target.Value))
                {
                    continue;
                }

                isUsable.Value = false;

                var navMeshAgentRef = _navMeshAgentRef.Get(parentBot.Value);
                navMeshAgentRef.Value.Stop();
                navMeshAgentRef.Value.enabled = false;

                if (!parentBot.Value.Has<CantMoveOnNavMesh>())
                {
                    parentBot.Value.AddComponent<CantMoveOnNavMesh>();
                }

                var abilityInstance = jumpAttack.Template.Create(World, _gameObjectRef.Get(parentBot.Value).Value);

                ref var childEntities = ref _childEntities.Get(parentBot.Value);
                childEntities.Value.Add(abilityInstance);

                var botParentTransform = _transformRef.Get(parentBot.Value);

                ref var abilityTargetTransform = ref abilityInstance.AddComponent<TransformRef>();
                abilityTargetTransform = _transformRef.Get(parentBot.Value);

                ref var parentEntity = ref abilityInstance.AddComponent<ParentEntity>();
                parentEntity = parentBot;

                ref var abilityTargetNavMeshAgent = ref abilityInstance.AddComponent<NavMeshAgentRef>();
                abilityTargetNavMeshAgent.Value = navMeshAgentRef.Value;

                ref var abilityTarget = ref abilityInstance.AddComponent<Target>();
                abilityTarget.Value = target.Value;

                ref var targetPosition = ref abilityInstance.AddComponent<LastTargetPosition>();

                var offset = _transformRef.Get(target.Value).Value.position - botParentTransform.Value.position;
                var randomOffset = Random.insideUnitSphere * 2f;
                randomOffset.y = 0;
                offset += randomOffset;

                targetPosition.Value = _transformRef.Get(target.Value).Value.position +
                                       offset.normalized * 2f +
                                       randomOffset;

                ref var initialPosition = ref abilityInstance.AddComponent<InitialPosition>();
                initialPosition.Value = botParentTransform.Value.position;

                ref var config = ref abilityInstance.GetComponent<BotJumpAttackAnimation>();

                ref var moveDirection = ref abilityInstance.AddComponent<MoveDirection>();
                var direction = targetPosition.Value - _worldPositionStash.Get(parentBot.Value).Value;
                moveDirection.Value = direction * config.JumpForce + math.up() * config.JumpHeight;
            }

            // if (_filter.GetLengthSlow() > 0)
            // {
            //     Debug.Log("");
            // }
        }
    }
}