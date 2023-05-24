using Scellecs.Morpeh;
using UnityEngine;

namespace Game
{
    public class WeaponFireModule : MonoBehaviour
    {
        protected Entity _weaponEntity;
        protected World _world;
        
        public void Construct(Entity weaponEntity, World world)
        {
            _world = world;
            _weaponEntity = weaponEntity;
        }
    }
}