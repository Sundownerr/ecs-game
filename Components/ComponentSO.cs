using System;
using Scellecs.Morpeh;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game
{
    [Serializable]
    public abstract class ComponentWrapper
    {
        public abstract void AddTo(Entity entity, GameObject gameObject);
    }

    [InlineProperty]
    [GUIColor(0.659f, 0.871f, 1)]
    [Serializable]
    public abstract class RuntimeComponentWrapper : ComponentWrapper
    { }

    [GUIColor(0.906f, 1, 0.816f)]
    [Serializable]
    public abstract class ConfigComponentWrapper : ComponentWrapper
    { }

    public abstract class RuntimeComponentWrapper<TComponent> : RuntimeComponentWrapper
        where TComponent : struct, IComponent
    {
        [NonSerialized] public TComponent Component;

        protected ref TComponent AddComponent(Entity entity)
        {
            ref var component = ref entity.AddComponent<TComponent>();
            component = Component;
            return ref component;
        }
    }

    [Serializable]
    public abstract class ComponentWrapper<TComponent> : ComponentWrapper where TComponent : struct, IComponent
    {
        [InlineProperty] [HideLabel] public TComponent Component;

        protected ref TComponent AddComponent(Entity entity)
        {
            ref var component = ref entity.AddComponent<TComponent>();
            component = Component;
            return ref component;
        }
    }

    [Serializable]
    public abstract class ConfigComponentWrapper<TComponent> : ConfigComponentWrapper
        where TComponent : struct, IComponent
    {
        [InlineProperty] [HideLabel] public TComponent Component;

        protected ref TComponent AddComponent(Entity entity)
        {
            ref var component = ref entity.AddComponent<TComponent>();
            component = Component;
            return ref component;
        }
    }

    [InlineProperty]
    [GUIColor(1f, 0.9f, 0.3f)]
    [Serializable]
    public class MarkerComponentWrapper<TComponent> : ComponentWrapper<TComponent> where TComponent : struct, IComponent
    {
        public override void AddTo(Entity entity, GameObject gameObject)
        {
            entity.AddComponent<TComponent>();
        }
    }
}