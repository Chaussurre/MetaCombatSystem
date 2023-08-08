using System;
using System.Collections.Generic;
using UnityEngine;
using OrbitResonance;

namespace MetaCombatSystem.ResourceManagement
{
    public interface IResourceModification<TResourceData> where TResourceData : struct
    {
        public void Initialize(TResourceData data, Resource resource, int delta, ICombatSystemSource source, List<string> tags, CombatTarget Self);

        public TResourceData GetResultData();
    }

    public class ResourcesManager<TResourceData, TResourceModification> : MonoBehaviour 
        where TResourceData : struct
        where TResourceModification : struct, IResourceModification<TResourceData>
    {

        [Serializable]
        public struct ResourceValue
        {
            public Resource Resource;
            public TResourceData Data;
            public DataWatcher<TResourceModification> DataWatcher;
        }

        public CombatTarget Self;
        public List<ResourceValue> Resources;

        public ResourceValue GetResourceValue(Resource resource)
        {
            var index = getResourceIndex(resource);
            if (index == -1)
                throw new ArgumentOutOfRangeException("resource");
            return Resources[index];
        }

        public bool HasResourceValue(Resource resource)
        {
            var index = getResourceIndex(resource);
            return index != -1;
        }

        public TResourceModification ChangeResource(Resource resource, int delta, ICombatSystemSource source, List<string> tags)
        {
            var index = getResourceIndex(resource);
            if (index == -1)
                throw new ArgumentOutOfRangeException("resource");
            var ResourceValue = Resources[index];

            var data = ResourceValue.Data;

            TResourceModification modification = default;
            modification.Initialize(data, resource, delta, source, tags, Self);
            modification = ResourceValue.DataWatcher.WatchData(modification);

            data = modification.GetResultData();

            Resources[index] = new()
            {
                Resource = ResourceValue.Resource,
                Data = data,
                DataWatcher = ResourceValue.DataWatcher,
            };

            return modification;
        }

        public TResourceModification PredictChangeResource(Resource resource, int delta, ICombatSystemSource source, List<string> tags)
        {
            var index = getResourceIndex(resource);
            if (index == -1)
                throw new ArgumentOutOfRangeException("resource");
            var ResourceValue = Resources[index];

            var data = ResourceValue.Data;

            TResourceModification modification = default;
            modification.Initialize(data, resource, delta, source, tags, Self);
            return ResourceValue.DataWatcher.WatchDataNoReaction(modification);
        }

        private int getResourceIndex(Resource resource)
        {
            return Resources.FindIndex(x => x.Resource == resource);
        }
    }
}