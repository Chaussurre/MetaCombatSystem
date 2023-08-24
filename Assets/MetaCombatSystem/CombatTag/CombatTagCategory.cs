using System.Collections.Generic;
using UnityEngine;

namespace MetaCombatSystem
{
    [CreateAssetMenu(fileName = "Combat Tag Category", menuName = "MetaCombat System/Combat Tag Category")]
    public class CombatTagCategory : ScriptableObject
    {
        public List<string> Tags = new();
    }
}