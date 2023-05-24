using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class EnemySpawnPoints
    {
        [ListDrawerSettings(Expanded = true, DraggableItems = false, ShowIndexLabels = false)] [SerializeField]
        public Transform[] All;
    }
}