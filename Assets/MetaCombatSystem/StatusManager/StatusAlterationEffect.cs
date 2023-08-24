using OrbitResonance;
using System.Collections.Generic;
using UnityEngine;

namespace MetaCombatSystem.StatusManagement
{

    public class StatusAlterationEffect : MonoBehaviour
    {
        public struct StatusStackChange
        {
            public int Stacks;
            public int? MaxStacks;
            public int Delta;
            public ICombatSystemSource Source;
            public List<CombatTag> Tags;
            public CombatTarget Self;

            public int ResultStacks { get
                {
                    var finalStack = Stacks + Delta;
                    if (MaxStacks.HasValue)
                        finalStack = Mathf.Clamp(finalStack, 0, MaxStacks.Value);
                    else
                        finalStack = Mathf.Max(finalStack, 0);
                    return finalStack;
                } }
        }

        public StatusAlteration StatusAlteration;

        [RemoteReadable]
        public int Stacks;
        public bool HasMax;
        [RemoteReadable]
        public int MaxStacks;

        public DataWatcher<StatusStackChange> DataWatcher;

        public bool IsActive => Stacks > 0;


        public StatusStackChange ChangeStacks(int delta, ICombatSystemSource source, CombatTarget Self, List<CombatTag> Tags)
        {
            StatusStackChange data = new()
            {
                Stacks = Stacks,
                MaxStacks = HasMax ? MaxStacks : null,
                Delta = delta,
                Source = source,
                Tags = Tags,
                Self = Self,
            };

            data = DataWatcher.WatchData(data);

            Stacks = data.ResultStacks;
            return data;
        }
    }
}
