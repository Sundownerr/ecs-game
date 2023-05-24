using System;
using Sirenix.OdinInspector;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

namespace Game
{
    public class ParticleSystemEmitter : MonoBehaviour
    {
        [ListDrawerSettings(Expanded = true, DraggableItems = false, ShowIndexLabels = false)] [SerializeField]
        private Entry[] _entries;

        public void EmitAt(Vector3 position)
        {
            for (var i = 0; i < _entries.Length; i++)
            {
                _entries[i].ParticleSystem.transform.position = position;
                _entries[i].ParticleSystem.Emit(_entries[i].EmitCount);
            }
        }

        public void EmitAt(ref NativeArray<float3> positions)
        {
            for (var i = 0; i < _entries.Length; i++)
            {
                var particleSystemTransform = _entries[i].ParticleSystem.transform;

                for (var j = 0; j < positions.Length; j++)
                {
                    particleSystemTransform.position = positions[j];
                    _entries[i].ParticleSystem.Emit(_entries[i].EmitCount);
                }
            }
        }

        public void EmitAt2(ref NativeArray<float3> positionsAndDirections)
        {
            var startSpeedCurve = new ParticleSystem.MinMaxCurve(0, 0);
            var defaultSpeedCurve = new ParticleSystem.MinMaxCurve(0, 0);

            for (var entryIndex = 0; entryIndex < _entries.Length; entryIndex++)
            {
                var particleSystemTransform = _entries[entryIndex].ParticleSystem.transform;
                var main = _entries[entryIndex].ParticleSystem.main;
                var startSpeed = main.startSpeed;
                var defaultSpeed = startSpeed.constant;
                defaultSpeedCurve.constantMin = defaultSpeed;
                defaultSpeedCurve.constantMax = defaultSpeed;

                startSpeedCurve.constantMin = defaultSpeed;

                for (var i = 0; i < positionsAndDirections.Length; i+=2)
                {
                    startSpeedCurve.constantMax = defaultSpeed + math.length(positionsAndDirections[i + 1]);
                    main.startSpeed = startSpeedCurve;

                    particleSystemTransform.SetPositionAndRotation(
                        positionsAndDirections[i],
                        Quaternion.LookRotation(positionsAndDirections[i + 1]));

                    _entries[entryIndex].ParticleSystem.Emit(_entries[entryIndex].EmitCount);
                    main.startSpeed = defaultSpeedCurve;
                }
            }
        }

        [Serializable]
        public class Entry
        {
            [HideLabel] [HorizontalGroup("1")] public ParticleSystem ParticleSystem;
            [HorizontalGroup("1")] [LabelWidth(90)]
            public int EmitCount = 1;
        }
    }
}