using System;
using UnityEngine;
using UnityEngine.AI;

namespace Game
{
    public class BotPrefab : MonoBehaviour

    {
        public Transform Model;
        public Vector3 DeathVfxPosition;
        public EntityReference EntityReference;

        [NonSerialized] public int ID;
        [NonSerialized] public int Index;

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position + DeathVfxPosition, 0.1f);
        }

        public Vector3 DeathVfxPositionWorld()
        {
            return transform.position + DeathVfxPosition;
        }
    }
}