using System.Collections;
using UnityEngine;

namespace MetaCombatSystem.Skills
{
    /// <summary>
    /// This class represent a skillEffect that apply to a predefined number of targets and whose effect can be different on each target.
    /// </summary>
    public abstract class SkillEffectMultipleTargets : SkillEffect
    {
        /// <summary>
        /// Set it to true if this effect accept to trigger without all the possible targets
        /// </summary>
        public bool CanBeCastOnPartialTargets;

        /// <summary>
        /// The number of target this specific skill can handle max. Should always be constant
        /// </summary>
        /// <returns>The max number of targets</returns>
        public abstract int GetNumberOfTargets();


        sealed public override void TriggerComponent(Skill Skill)
        {
            int targetsNumber = GetNumberOfTargets();
            for (int i = 0; i < targetsNumber && i < Skill.TargetCount; i++)
            {
                var target = Skill.GetTarget(i + FirstAndLastTargets.x);
                EffectTrigger(target, i);
            }
        }

        /// <summary>
        /// Apply the effect on a target given the target's index.
        /// </summary>
        /// <param name="target">The target to apply the effect to</param>
        /// <param name="index">The index, from the point of view of the skill. the first target will always be 0, then 1, etc</param>
        protected abstract void EffectTrigger(CombatTarget target, int index);
    }
}