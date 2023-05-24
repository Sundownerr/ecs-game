using System.Collections.Generic;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Game
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class CreateWeapon : ISystem
    {
        private Filter _filter;

        public void Dispose()
        { }

        public void OnAwake()
        {
            _filter = World.Filter.With<Instantiate_OneFrame>().With<InstantiateWeaponConfig>().With<ParentEntity>();
        }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _filter)
            {
                ref var config = ref entity.GetComponent<InstantiateWeaponConfig>();
                ref var parentEntity = ref entity.GetComponent<ParentEntity>();
                ref var parentTransformParts = ref parentEntity.Value.GetComponent<TransformPartsRef>();
                
                var weaponParent = parentTransformParts.Value.WithID(config.SlotID);
                var weaponInstance = Object.Instantiate(config.Prefab, weaponParent);
                var weaponEntity = config.EntityTemplate.Create(World, weaponInstance);

                if (!parentEntity.Value.Has<ChildEntities>())
                {
                    ref var childs = ref parentEntity.Value.AddComponent<ChildEntities>();
                    childs.Value = new List<Entity>();
                    childs.World = World;
                }
                {
                    ref var childs = ref parentEntity.Value.GetComponent<ChildEntities>();
                    childs.Value.Add(weaponEntity);
                }
                
            }
        }
    }
}