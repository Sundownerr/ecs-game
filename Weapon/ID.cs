using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game
{
    [CreateAssetMenu(menuName = "SO/ID", fileName = "ID", order = 0)]
    public class ID : ScriptableObject, IEquatable<ID>
    {
        public int Value ;

        private void OnEnable()
        {
            Value = Random.Range(0, int.MaxValue);
        }

        public bool Equals(ID other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return base.Equals(other) && Value == other.Value;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != this.GetType())
            {
                return false;
            }

            return Equals((ID) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), Value);
        }

        public static bool operator ==(ID left, ID right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ID left, ID right)
        {
            return !Equals(left, right);
        }
    }
}