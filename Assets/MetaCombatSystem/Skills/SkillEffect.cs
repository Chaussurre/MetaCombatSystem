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

        /// <summary>
        /// The first and last index of the targets concerned by this effect. Effects only work on continuous target indexes.
        /// </summary>
        public Vector2Int FirstAndLastTargets;

        /// <summary>
        /// If this is true, then if not all targets are set the effect cannot trigger.
        /// </summary>
        public bool requireAllTargets;

        /// <summary>
        /// this method is called everytime before the skill trigger any skilleffects.
        /// </summary>
        public virtual void SetUpEffect() { }

        public override bool CanSkillBeTriggered(Skill skill)
        {
            if (requireAllTargets && skill.TargetCount <= FirstAndLastTargets.y)
                return false;

            for (int i = FirstAndLastTargets.x; i < FirstAndLastTargets.y && i < skill.TargetCount; i++)
                if (!IsTargetValid(skill.GetTarget(i), i))
                    return false;

            return true;
        }

        /// <summary>
        /// Returns wether this effect can be applied to this target. True by default.
        /// </summary>
        /// <param name="target">The tested target</param>
        /// <param name="index">The index of the target from the point of view of the effect.</param>
        /// <returns>true iff the target is valid</returns>
        public virtual bool IsTargetValid(CombatTarget target, int index)
        {
            return true;
        }
    }
}