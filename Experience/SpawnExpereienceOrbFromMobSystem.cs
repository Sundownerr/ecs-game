using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Game
{
    // [Il2CppSetOption(Option.NullChecks, false)]
    // [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    // [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    // public sealed class SpawnExpereienceOrbFromMob : ISystem
    // {
    //     private readonly ExperienceOrbFactory _experienceOrbFactory;
    //     private Filter _filter;
    //
    //     public SpawnExpereienceOrbFromMobSystem(ExperienceOrbFactory experienceOrbFactory)
    //     {
    //         _experienceOrbFactory = experienceOrbFactory;
    //     }
    //
    //     public void Dispose()
    //     { }
    //
    //     public void OnAwake()
    //     {
    //         _filter = World.Filter.With<BotDeathEvent>();
    //     }
    //
    //     public World World { get; set; }
    //
    //     public void OnUpdate(float deltaTime)
    //     {
    //         foreach (var entity in _filter)
    //         {
    //             var mob = entity.GetComponent<BotDeathEvent>().Bot;
    //             var position = mob.GetComponent<WorldPosition>().Value;
    //
    //             _experienceOrbFactory.CreateAt(position);
    //         }
    //     }
    // }
}