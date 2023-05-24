using Game.ECS;
using Scellecs.Morpeh;
using UnityEngine;

namespace Game
{
    public interface IConstructFromGameObject 
    {
        void Construct(GameObject gameObject);
    }

    public interface IWrappedComponent<out T> where T : struct, IComponent
    {
        T Wrapped();
    }
   

    public interface IMigrateToOtherComponent
    {
        void Migrate(EntitySO entityTemplate);
        
        /*
         *
         *  public void Migrate(EntitySO entityTemplate)
        {
            for (var index = 0; index < entityTemplate.ComponentWrappers.Count; index++)
            {
            
                if (entityTemplate.ComponentWrappers[index] is TargetWorldPosition targetWorldPosition)
                {
                    entityTemplate.ComponentWrappers[index] = new WorldPosition { Value = targetWorldPosition.Value };
                }
            }
        }
         * 
         */
    }
}