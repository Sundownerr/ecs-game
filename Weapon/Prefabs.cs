using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(menuName = "SO/Prefabs", fileName = "Prefabs", order = 0)]
    public class Prefabs : ScriptableObject
    {
        [ListDrawerSettings(ShowIndexLabels = false, DraggableItems = false)]
        public Entry[] All;

        public GameObject WithID(ID id)
        {
            foreach (var entry in All)
            {
                if (entry.ID == id)
                {
                    return entry.Prefab;
                }
            }

            return null;
        }

        [Serializable]
        public class Entry
        {
            [HorizontalGroup("1")]
            [HideLabel]
            public ID ID;
            [HideLabel]
            [HorizontalGroup("1")]
            public GameObject Prefab;
        }
    }
}