using UnityEngine;

namespace Game
{
    [CreateAssetMenu(menuName = "SO/Mob Config", fileName = "MobConfig")]
    public class BotConfig : ScriptableObject
    {
        public int MaxAmount;
        public int SpawnBatch;
        public float SpawnInterval;
        public ID MobID;
    }
}