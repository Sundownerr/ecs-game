using Scellecs.Morpeh;
using UnityEngine;

namespace Game
{
    public class ExperienceOrbFactory : MonoBehaviour
    {
        [SerializeField] private ParticleSystemEmitter _expEmitter;
        [SerializeField] private Vector3 _offset;

        public void Construct()
        {
            // ref var expOrbParticleSystem = ref World.Default.CreateEntity().AddComponent<Expere>();
            // expOrbParticleSystem.Value = _expEmitter.Entries[0].ParticleSystem;
        }

        public void CreateAt(Vector3 position)
        {
            _expEmitter.EmitAt(position + _offset);
        }
    }
}