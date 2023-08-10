using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MetaCombatSystem.StatusManagement
{
    [CreateAssetMenu(fileName = "StatusAlteration", menuName = "MetaCombat System/Status Alteration")]
    public class StatusAlteration : ScriptableObject, ICombatSystemSource
    {
        public StatusAlterationEffect EffectPrefab;
        public string StatusName;
    }
}
