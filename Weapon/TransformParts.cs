using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game
{
    public class TransformParts : MonoBehaviour
    {
        [ListDrawerSettings(Expanded = true, ShowIndexLabels = false, DraggableItems = false)]
       [InlineProperty]
        public Entry[] All;

        public Transform WithID(ID id)
        {
            foreach (var entry in All)
            {
                if (entry.ID == id)
                {
                    return entry.Value;
                }
            }

            return null;
        }

        public Transform WithID(int id)
        {
            foreach (var entry in All)
            {
                if (entry.ID.Value == id)
                {
                    return entry.Value;
                }
            }

            return null;
        }

        [Serializable]
        public class Entry
        {
            [HideLabel] [HorizontalGroup("1")] public Transform Value;
            [HideLabel] [HorizontalGroup("1")] public ID ID;
        }
    }
}