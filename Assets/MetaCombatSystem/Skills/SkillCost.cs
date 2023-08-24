using System;

namespace MetaCombatSystem.Skills
{
    public abstract class SkillCost : SkillComponent
    {
        public sealed override bool CanSkillBeTriggered(SkillCast skillCast)
        {
            return IsCostAffordable(skillCast.GetCaster());
        }

        protected abstract bool IsCostAffordable(CombatTarget selfTarget);

        public sealed override void TriggerComponent(SkillCast skillCast)
        {
            PayCosts(skillCast.GetCaster());
        }

        protected abstract void PayCosts(CombatTarget combatTarget);
    }
}