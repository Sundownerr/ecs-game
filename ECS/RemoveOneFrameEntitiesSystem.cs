using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Game
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class RemoveOneFrameEntities : ISystem
    {
        public void Dispose()
        { }

        public void OnAwake()
        { }

        public World World { get; set; }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in World.Filter.With<OneFrameMarker>())
            {
                World.RemoveEntity(entity);
            }

            foreach (var entity in World.Filter.With<Construct_OneFrame>())
            {
                entity.RemoveComponent<Construct_OneFrame>();
            }

            foreach (var entity in World.Filter.With<Instantiate_OneFrame>())
            {
                entity.RemoveComponent<Instantiate_OneFrame>();
            }
        }
    }
}