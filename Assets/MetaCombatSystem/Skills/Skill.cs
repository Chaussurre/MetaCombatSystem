using System;
using System.Collections.Generic;
using UnityEngine;

namespace MetaCombatSystem.Skills
{
    /// <summary>
    /// This class handle skills for turn by turn style combats and other similar combat styles.
    /// To use this class, add it to a gameobject representing your skill.
    /// Then, choose how many targets will the skill concern (with a min and a max)
    /// Add effects and filters to decide which targets are valids, and what will the skill do to them.
    /// Then during runtime, add each target to the Skill's list of targets.
    /// Once all the targets are set, call the trigger method and the skill will be cast on those targets.
    /// </summary>
    public sealed class Skill : MonoBehaviour
    {
        /// <summary>
        /// The minimum number of targets that the skill require to function and the maximum number of targets the skill can handle
        /// </summary>
        public Vector2Int MinAndMaxNumberOfTargets = Vector2Int.one;

        /// <summary>
        /// The list of all components of the skill.
        /// </summary>
        public List<SkillComponent> Components = new();

        /// <summary>
        /// Check wether or not a target can be added at the given index
        /// </summary>
        /// <param name="target">the target to test</param>
        /// <param name="index">the index to test</param>
        /// <returns>wether or not the target can be added</returns>
        public bool IsTargetValid(CombatTarget target, int index)
        {
            foreach (var component in Components)
                if (component is SkillEffect effect &&
                    effect.FirstAndLastTargets.x <= index &&
                    effect.FirstAndLastTargets.y >= index &&
                    !effect.IsTargetValid(target))
                    return false;

            return true;
        }

        /// <summary>
        /// Cast the skill on the targets of the list
        /// </summary>
        public void Trigger(SkillCast skillCast)
        {
            if (!ReadyToTrigger(skillCast))
                return;

            foreach (var effect in Components)
                effect.TriggerComponent(skillCast);
        }


        /// <summary>
        /// Check wether or not the skill can be triggered with the targets of the list.
        /// </summary>
        /// <returns>true if the skill can be triggered</returns>
        public bool ReadyToTrigger(SkillCast skillCast)
        {
            if (!skillCast.IsTargetListCorrect())
                return false;

            foreach (var component in Components)
                if (!component.CanSkillBeTriggered(skillCast))
                    return false;

            return true;
        }
    }
}