namespace MetaCombatSystem.Skills
{
    /// <summary>
    /// This class represent an effect that only affect one target at a time and does the same thing to all targets.
    /// </summary>
    public abstract class SkillEffectMonoTarget : SkillEffect
    {
        sealed public override void TriggerComponent(Skill Skill)
        {
            for (int i = FirstAndLastTargets.x; i <= FirstAndLastTargets.y && i < Skill.TargetCount; i++)
                EffectTrigger(Skill.GetTarget(i));
        }

        /// <summary>
        /// The effect to apply to each targets
        /// </summary>
        /// <param name="target">The current target</param>
        public abstract void EffectTrigger(CombatTarget target);

        sealed public override bool IsTargetValid(CombatTarget target, int index)
        {
            return IsTargetValid(target);
        }

        /// <summary>
        /// Wehter or not the given target is a valid one. True by default.
        /// </summary>
        /// <param name="target">The tested target</param>
        /// <returns>true iff the target is valid</returns>
        public virtual bool IsTargetValid(CombatTarget target)
        {
            return true;
        }
    }
}