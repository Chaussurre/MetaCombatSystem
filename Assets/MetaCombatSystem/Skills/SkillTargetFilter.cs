using System.Collections;
using UnityEngine;

namespace MetaCombatSystem.Skills
{
    /// <summary>
    /// This class specifies extra rules for a skill's targets without being associated with a specific effect.
    /// </summary>
    public abstract class SkillTargetFilter : SkillEffect
    {
        sealed public override void TriggerComponent(Skill Skill) { }

        public abstract override bool IsTargetValid(CombatTarget target, int index);
    }
}