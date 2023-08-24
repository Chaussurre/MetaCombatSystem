using UnityEngine;

namespace MetaCombatSystem.Skills
{
    public abstract class SkillComponent : MonoBehaviour
    {
        public abstract bool CanSkillBeTriggered(SkillCast skillCast);

        public abstract void TriggerComponent(SkillCast skillCast);
    }
}