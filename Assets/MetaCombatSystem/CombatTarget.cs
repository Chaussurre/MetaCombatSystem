using UnityEngine;
using System.Collections.Generic;

namespace MetaCombatSystem
{
    /// <summary>
    /// Describes anything targetable. 
    /// </summary>
    public class CombatTarget : MonoBehaviour
    {
        /// <summary>
        /// The list of behaviors accessibles by skills targeting this target.
        /// </summary>
        public List<Behaviour> TargetTypes;
        
        /// <summary>
        /// Get a behavior given its type.
        /// </summary>
        /// <typeparam name="TTarget">The type of the searched behavior</typeparam>
        /// <param name="name">If several behavior have the same type, you can specify the gameobject's name.</param>
        /// <returns>The searched behavior</returns>
        public TTarget GetTarget<TTarget>(string name = null) where TTarget : Behaviour
        {
            foreach (var behaviour in TargetTypes)
                if (behaviour is TTarget target && (name == null || behaviour.name == name))
                    return target;

            return null;
        }

        private void Start() => CombatTargetManager.Instance?.RegisterTarget(this);
        private void OnEnable() => CombatTargetManager.Instance?.RegisterTarget(this);
        private void OnDisable() => CombatTargetManager.Instance?.RemoveTarget(this);
    }
}
