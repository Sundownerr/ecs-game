using Scellecs.Morpeh;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game
{
    public class ParticleWeapon : MonoBehaviour, IWeapon
    {
        [SerializeField] private ParticleSystem _particleSystem;
        [SerializeField] private WeaponFireModule _weaponFireModule;
        [SerializeField] private int _emitCount = 1;
        [ListDrawerSettings(Expanded = true, ShowIndexLabels = false, DraggableItems = true)]
        [SerializeReference] private ComponentWrapper[] _weaponHitComponents;

        // private void Awake()
        // {
        //     _weaponFireModule.Construct(_weaponHitComponents);
        // }

        public void Construct(Entity weaponEntity, World world)
        {
            _weaponFireModule.Construct(weaponEntity, world);
        }

        public void Use()
        {
            _particleSystem.Emit(_emitCount);
        }
    }
}