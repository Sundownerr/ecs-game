using Scellecs.Morpeh;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game
{
   
    public class ComponentWrapperGroup : ComponentWrapper
    {
        [ListDrawerSettings(ShowIndexLabels = false, Expanded = true)]
        public ComponentWrapper[] Components;

        public override void AddTo(Entity entity, GameObject gameObject)
        {
            foreach (var componentSo in Components)
            {
                componentSo.AddTo(entity, gameObject);
            }
        }
        
    }
}