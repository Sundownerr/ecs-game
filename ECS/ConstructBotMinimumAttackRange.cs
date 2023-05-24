using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Game
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class ConstructBotMinimumAttackRange : ISystem
    {
        private Filter _filter;

        public void Dispose()
        { }

        public void OnAwake()
        {
            _filter = World.Filter.With<BotMarker>().With<MeleeWeapons>().With<AttackRadius>()
                .With<Construct_OneFrame>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var attackRadius = ref entity.GetComponent<AttackRadius>();
                ref var meleeWeapons = ref entity.GetComponent<MeleeWeapons>();

                foreach (var weapon in meleeWeapons.Value)
                {
                    ref var meleeWeaponSettings = ref weapon.GetComponent<MeleeWeaponSettings>();
                    // Debug.Log(meleeWeaponSettings.Range);
                    if (attackRadius.Value < meleeWeaponSettings.Range)
                    {
                        attackRadius.Value = meleeWeaponSettings.Range;
                    }
                }
                
               
            }
        }
    }
}