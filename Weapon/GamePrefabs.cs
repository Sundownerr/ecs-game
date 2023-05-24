using System;
using Sirenix.OdinInspector;

namespace Game
{
    [Serializable]
    public class GamePrefabs
    {
        [ListDrawerSettings(ShowIndexLabels = false, DraggableItems = false)]
        public Prefabs[] Enemies;
        [ListDrawerSettings(ShowIndexLabels = false, DraggableItems = false)]
        public Prefabs[] Players;
        [ListDrawerSettings(ShowIndexLabels = false, DraggableItems = false)]
        public Prefabs[] Weapons;
    }
}