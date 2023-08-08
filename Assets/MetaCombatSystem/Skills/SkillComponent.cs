using UnityEngine;

namespace MetaCombatSystem.Skills
{
    public abstract class SkillComponent : MonoBehaviour
    {
        public abstract bool CanSkillBeTriggered(Skill skill);

        public abstract void TriggerComponent(Skill skill);
    }
}