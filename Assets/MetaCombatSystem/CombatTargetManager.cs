using System.Collections.Generic;
using UnityEngine;

namespace MetaCombatSystem
{
    /// <summary>
    /// A singleton class containing all the targets. Usefull for tracking targets.
    /// </summary>
    public class CombatTargetManager : MonoBehaviour
    {
        /// <summary>
        /// The list of all targets. Should not be modified by the library's user.
        /// </summary>
        [HideInInspector]
        public List<CombatTarget> Targets = new();

        /// <summary>
        /// The instance to access the singleton
        /// </summary>
        public static CombatTargetManager Instance;

        private void Awake()
        {
            if (!Instance)
                Instance = this;
            else
                Destroy(this);
        }

        private void OnDestroy()
        {
            if (Instance == this)
                Instance = null;
        }

        public void RegisterTarget(CombatTarget skillTarget)
        {
            Targets.Add(skillTarget);
        }

        public void RemoveTarget(CombatTarget skillTarget)
        {
            Targets.Remove(skillTarget);
        }
    }
}