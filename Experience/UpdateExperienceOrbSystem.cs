using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Game
{
    // [Il2CppSetOption(Option.NullChecks, false)]
    // [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    // [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    // public sealed class UpdateExperienceOrb : ISystem
    // {
    //     private readonly ParticleSystem.Particle[] _particles = new ParticleSystem.Particle[1500];
    //     private readonly int _pickupRadius;
    //     private readonly int _vacuumForce;
    //     private readonly int _vacuumRadius;
    //     private Filter _filter;
    //
    //     public UpdateExperienceOrbSystem(ExperienceOrbConfig config)
    //     {
    //         _pickupRadius = config.PickupRadius;
    //         _vacuumRadius = config.VacuumRadius;
    //         _vacuumForce = config.VacuumForce;
    //     }
    //
    //     public void Dispose()
    //     { }
    //
    //     public void OnAwake()
    //     {
    //         _filter = World.Filter.With<ExperienceOrbParticleSystem>();
    //     }
    //
    //     public World World { get; set; }
    //
    //     public void OnUpdate(float deltaTime)
    //     {
    //         var playerPosition = World.Filter.With<PlayerMarker>().With<WorldPosition>().First()
    //             .GetComponent<WorldPosition>().Value;
    //
    //         foreach (var entity in _filter)
    //         {
    //             var particleSystem = entity.GetComponent<ExperienceOrbParticleSystem>().Value;
    //             var particleCount = particleSystem.GetParticles(_particles);
    //
    //             var particleLifetime = particleSystem.main.startLifetime.constant;
    //
    //             for (var i = 0; i < particleCount; i++)
    //             {
    //                 if (_particles[i].remainingLifetime <= 0.1f)
    //                 {
    //                     _particles[i].remainingLifetime = particleLifetime;
    //                 }
    //
    //                 var particlePosition = _particles[i].position;
    //                 var x = playerPosition.x - particlePosition.x;
    //                 var y = playerPosition.y - particlePosition.y;
    //                 var z = playerPosition.z - particlePosition.z;
    //
    //                 var distance = x * x + y * y + z * z;
    //
    //                 if (distance <= _pickupRadius * _pickupRadius)
    //                 {
    //                     _particles[i].remainingLifetime = 0f;
    //                     continue;
    //                 }
    //
    //                 if (distance < _vacuumRadius * _vacuumRadius)
    //                 {
    //                     _particles[i].velocity = new Vector3(x, y, z).normalized * _vacuumForce;
    //                 }
    //             }
    //
    //             particleSystem.SetParticles(_particles, particleCount);
    //         }
    //     }
    // }
}