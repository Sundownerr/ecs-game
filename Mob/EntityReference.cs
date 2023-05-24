using System;
using Scellecs.Morpeh;
using UnityEngine;

namespace Game
{
    public class EntityReference : MonoBehaviour
    {
        [NonSerialized]
        public Entity Entity;
    }
}