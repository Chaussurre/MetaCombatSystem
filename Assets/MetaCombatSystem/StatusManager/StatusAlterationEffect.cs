using OrbitResonance;
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


        public void ChangeStacks(int delta, ICombatSystemSource source, CombatTarget Self)
        {
            StatusStackChange data = new()
            {
                Stacks = Stacks,
                MaxStacks = HasMax ? MaxStacks : null,
                Delta = delta,
                Source = source,
                Self = Self,
            };

            data = DataWatcher.WatchData(data);

            Stacks = data.ResultStacks;
        }
    }
}
