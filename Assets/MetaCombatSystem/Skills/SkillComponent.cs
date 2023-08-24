using UnityEngine;

namespace MetaCombatSystem.Skills
{
    public abstract class SkillComponent : MonoBehaviour
    {
        public abstract bool CanSkillBeTriggered(SkillCast skillCast);

        public abstract void TriggerComponent(SkillCast skillCast);

        /// <summary>
        /// This method is called everytime before the skill is cast.
        /// </summary>
        public virtual void SetUpEffect() { }

        /// <summary>
        /// This method is called everytime after the skill is cast.
        /// </summary>
        public virtual void FinishEffect() { }
    }
}