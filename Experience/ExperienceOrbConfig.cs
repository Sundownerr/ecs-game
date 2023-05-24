using UnityEngine;

namespace Game
{
    [CreateAssetMenu(menuName = "SO/Experience Orb Config", fileName = "ExperienceOrbConfig")]
    public class ExperienceOrbConfig : ScriptableObject
    {
        public int PickupRadius;
        public int VacuumRadius;
        public int VacuumForce;
    }
}