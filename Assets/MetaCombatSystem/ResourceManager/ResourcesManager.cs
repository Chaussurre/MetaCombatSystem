using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace MetaCombatSystem.ResourceManagement
{

    public class ResourcesManager : ResourcesManager<
        ResourcesManager.ResourceData, 
        ResourcesManager.ResourceModification>
    {
        [Serializable]
        public struct ResourceData
        {
            public int MaxValue;
            public int MinValue;
            public int Value;
        }

        public struct ResourceModification : IResourceModification<ResourceData>
        {
            public int originalValue { get; private set; }
            public int originalMax { get; private set; }
            public int max;
            public int originalMin { get; private set; }
            public int min;
            public int delta;
            public float multiplier;
            public ICombatSystemSource source;
            public CombatTarget Self;
            public List<string> tags;

            public void Initialize(ResourceData data, Resource resource, int delta, ICombatSystemSource source, List<string> tags, CombatTarget Self)
            {
                originalValue = data.Value;
                originalMax = data.MaxValue;
                max = data.MaxValue;
                originalMin = data.MinValue;
                min = data.MinValue;
                this.delta = delta;
                this.source = source;
                this.tags = tags;
                multiplier = 1f;
                this.Self = Self;
            }

            public ResourceData GetResultData()
            {
                return new()
                {
                    MaxValue = max,
                    MinValue = Mathf.Min(max, min),
                    Value = Mathf.Clamp(originalValue + Mathf.RoundToInt(delta * multiplier), min, max),
                };
            }
        }
    }
}
