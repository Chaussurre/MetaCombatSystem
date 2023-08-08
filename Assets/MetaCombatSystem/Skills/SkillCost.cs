

namespace MetaCombatSystem.Skills
{
    public abstract class SkillCost : SkillComponent
    {
        sealed public override bool CanSkillBeTriggered(Skill skill)
        {
            return IsCostAffordable(skill.GetTarget(-1));
        }

        public abstract bool IsCostAffordable(CombatTarget selfTarget);
    }
}