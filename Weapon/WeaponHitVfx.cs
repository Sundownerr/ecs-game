using Scellecs.Morpeh;
using Unity.Collections;
using Unity.IL2CPP.CompilerServices;
using Unity.Mathematics;
using UnityEngine;

namespace Game
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class WeaponHitVfx : ISystem
    {
        private readonly ParticleSystemEmitter _hitVfx;
        private Filter _filter;
        private Stash<WorldPosition> _worldPositionStash;

        public WeaponHitVfx(ParticleSystemEmitter hitVfx)
        {
            _hitVfx = hitVfx;
        }

        public void Dispose()
        { }

        public void OnAwake()
        {
            _filter = World.Filter.With<DamageEvent>().With<WorldPosition>();
            _worldPositionStash = World.GetStash<WorldPosition>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            var positions =new NativeArray<float3>(_filter.GetLengthSlow(), Allocator.Temp);
            var index = 0;
            
            foreach (var entity in _filter)
            {
                ref var hitPosition = ref _worldPositionStash.Get(entity);
                positions[index] = hitPosition.Value;
                index++;
            }
            
            _hitVfx.EmitAt(ref positions);

            positions.Dispose();
        }
    }
}