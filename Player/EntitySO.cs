using System.Collections.Generic;
using Scellecs.Morpeh;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.ECS
{
    [CreateAssetMenu(menuName = "SO/Entity", fileName = "EntitySO")]
    public class EntitySO : SerializedScriptableObject
    {
        [HideLabel]
        public ID ID;

        [PropertySpace(20)] [ListDrawerSettings(ShowIndexLabels = false, ShowPaging = false)]
        public ComponentWrapper[] Components;

        
        [PropertySpace(20)] [InlineEditor(Expanded = false)] [ListDrawerSettings(ShowIndexLabels = false)]
        public EntitySO[] Childs;

        public Entity Create(World inWorld, GameObject gameObject)
        {
            var toEntity = inWorld.CreateEntity();

            Add(Components, toEntity, gameObject);
            Add(Childs, toEntity, inWorld, gameObject);

            return toEntity;
        }

        public Entity CopyTo(Entity entity, World inWorld, GameObject gameObject)
        {
            Add(Components, entity, gameObject);
            Add(Childs, entity, inWorld, gameObject);

            return entity;
        }

        private static void Add(IEnumerable<ComponentWrapper> components, Entity parentEntity, GameObject gameObject)
        {
            foreach (var component in components)
            {
                component.AddTo(parentEntity, gameObject);
            }
        }

        private static void Add(EntitySO[] childs,
                                Entity parentEntity,
                                World inWorld,
                                GameObject gameObject)
        {
            if (childs.Length == 0)
            {
                return;
            }

            ref var childEntities = ref parentEntity.AddComponent<ChildEntities>();
            childEntities.Value = new List<Entity>(childs.Length);
            childEntities.World = inWorld;

            foreach (var child in childs)
            {
                var childEntity = child.Create(inWorld, gameObject);
                ref var parent = ref childEntity.AddComponent<ParentEntity>();
                parent.Value = parentEntity;

                childEntities.Value.Add(childEntity);
            }
        }
    }
}