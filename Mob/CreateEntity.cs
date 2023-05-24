using Game.ECS;
using Scellecs.Morpeh;
using UnityEngine;

namespace Game
{
    public class CreateEntity : MonoBehaviour
    {
        public EntitySO EntityTemplate;

        private void Start()
        {
            EntityTemplate.Create(World.Default, gameObject);
        }
    }
}