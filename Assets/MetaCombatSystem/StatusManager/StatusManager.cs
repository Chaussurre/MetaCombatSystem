using OrbitResonance;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MetaCombatSystem.StatusManagement
{
    public class StatusManager : MonoBehaviour
    {
        public CombatTarget Self;
        public List<StatusAlterationEffect> StatusAlterations = new();

        public StatusAlterationEffect GetStatus(StatusAlteration status)
        {
            var result = StatusAlterations.FirstOrDefault(x => x.StatusAlteration == status);

            if (result == null)
            {
                var statusEffect = Instantiate(status.EffectPrefab, transform);
                statusEffect.StatusAlteration = status;
                StatusAlterations.Add(statusEffect);
                return StatusAlterations[^1];
            }

            return result;
        }

        public StatusAlterationEffect.StatusStackChange ChangeStacks(StatusAlteration status, 
            int delta, ICombatSystemSource source, List<CombatTag> Tags = null)
        {
            var statusEffect = GetStatus(status);
            return statusEffect.ChangeStacks(delta, source, Self, Tags);
        }

        public DataWatcher<StatusAlterationEffect.StatusStackChange> GetDataWatcher(StatusAlteration status)
        {
            var statusEffect = GetStatus(status);

            return statusEffect.DataWatcher;
        }
    }
}
