using Scellecs.Morpeh;

namespace Game
{
    public interface IWeapon
    {
        void Construct(Entity weaponEntity, World world);
        void Use();
    }
}