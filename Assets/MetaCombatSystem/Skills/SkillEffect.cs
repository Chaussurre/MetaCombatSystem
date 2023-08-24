using System;
using System.Runtime.CompilerServices;
using UnityEngine;

[assembly: InternalsVisibleTo("MetaCombatSystem.Editor")]
namespace MetaCombatSystem.Skills
{
    /// <summary>
    /// Represent an effect of a skill.
    /// If this is your first time using this library, please consider creating your effects as childs of SkillEffectMonoTargets 
    /// or SkillEffectMultipleTargets instead.
    /// </summary>
    public abstract class SkillEffect : SkillComponent
    {
        internal Skill Skill;

        public bool ApplyToCaster;

        /// <summary>
        /// The first and last index of the targets concerned by this effect. Effects only work on continuous target indexes.
        /// </summary>
        public Vector2Int FirstAndLastTargets;

        /// <summary>
        /// If this is true, then if not all targets are set the effect cannot trigger.
        /// </summary>
        public bool requireAllTargets;

        public override bool CanSkillBeTriggered(SkillCast skillCast)
        {
            if (requireAllTargets && skillCast.TargetCount <= FirstAndLastTargets.y)
                return false;

            if (ApplyToCaster)
                IsTargetValid(skillCast.GetCaster());

            for (int i = FirstAndLastTargets.x; i < FirstAndLastTargets.y && i < skillCast.TargetCount; i++)
                if (!IsTargetValid(skillCast.GetTarget(i)))
                    return false;

            return true;
        }

        /// <summary>
        /// Returns wether this effect can be applied to this target.
        /// </summary>
        /// <param name="target">The tested target</param>
        /// <param name="index">The index of the target from the point of view of the effect.</param>
        /// <returns>true iff the target is valid</returns>
        public abstract bool IsTargetValid(CombatTarget target);

        public override void TriggerComponent(SkillCast skillCast)
        {
            for (int i = FirstAndLastTargets.x; i <= FirstAndLastTargets.y && i < skillCast.TargetCount; i++)
                TriggerEffect(skillCast.GetTarget(i));
        }

        public abstract void TriggerEffect(CombatTarget combatTarget);
    }
}