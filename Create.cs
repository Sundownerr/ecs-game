using Scellecs.Morpeh;

namespace Game
{
    public static class Create
    {
        public static Entity Entity(World inWorld)
        {
            return inWorld.CreateEntity();
        }
        public static Entity CreateDamageEvent(this World inWorld, float damageValue)
        {
            var entity = inWorld.CreateEntity();
            entity.AddComponent<DamageEvent>();
            entity.AddComponent<OneFrameMarker>();
            
            ref var damage = ref entity.AddComponent<Damage>();
            damage.Value = damageValue;
            
            return  entity;
        }
    }
}