using System.Collections.Generic;
using Scellecs.Morpeh;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(menuName = "SO/Ability Config", fileName = "AbilityConfig")]
    public class AbilityConfig : SerializedScriptableObject
    {
        [ListDrawerSettings(Expanded = true, DraggableItems = false, ShowIndexLabels = false, ShowPaging = false)]
       [SerializeField] private List<IComponent> _components;
        // public string Id;
        // public float ShootInterval = 0.1f;
        // public GameObject Prefab;
    }
}