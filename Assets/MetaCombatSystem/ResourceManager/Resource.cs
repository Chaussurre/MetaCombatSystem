using UnityEngine;

namespace MetaCombatSystem.ResourceManagement
{
    [CreateAssetMenu(fileName = "Resource", menuName = "MetaCombat System/Create Resource")]
    public class Resource : ScriptableObject
    {
        public string Name;
        public Color Color;
    }
}
