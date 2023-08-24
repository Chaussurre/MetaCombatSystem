using System;
using System.Collections.Generic;

namespace MetaCombatSystem.Skills
{
    public struct SkillCast
    {
        public readonly Skill Skill;

        public readonly List<CombatTarget> Casters;

        private readonly List<CombatTarget> Targets;

        public SkillCast(Skill skill)
        {
            Skill = skill;
            Casters = new();
            Targets = new();
        }

        public int TargetCount {
            get
            {
                int count = 0;
                foreach (var target in Targets)
                    if (target)
                        count++;
                return count;
            }
        }

        public bool HasCasters => Casters.Count > 0;


        /// <summary>
        /// Return the target set for the given index.
        /// </summary>
        /// <param name="index">the index of the target</param>
        /// <returns>The target or null if the index is not correct</returns>
        public CombatTarget GetTarget(int index)
        {
            if (index >= Targets.Count || index < 0)
                return null;

            return Targets[index];
        }

        /// <summary>
        /// Set a target at a given index.
        /// </summary>
        /// <param name="target">The target to add to the list</param>
        /// <param name="index">The index</param>
        /// <returns>true iff the target was correctly added</returns>
        public bool SetTarget(CombatTarget target, int index)
        {
            if (index < 0 || index >= Skill.MinAndMaxNumberOfTargets.y)
                return false;

            if (!Skill.IsTargetValid(target, index))
                return false;

            for (int i = Targets.Count; i <= index; i++)
                Targets.Add(null);

            Targets[index] = target;
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        public void RemoveTarget(CombatTarget target)
        {
            int index = Targets.IndexOf(target);
            Targets[index] = null;
            cleanTargets();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        public void RemoveTarget(int index)
        {
            Targets[index] = null;
            cleanTargets();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public CombatTarget GetCaster(int index = 0)
        {
            return Casters[^(1 + index)];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool IsTargetListCorrect()
        {
            if (TargetCount < Skill.MinAndMaxNumberOfTargets.x)
                return false;

            foreach (var target in Targets)
                if (!target)
                    return false;

            return true;
        }

        /// <summary>
        /// Remove all the targets from the list.
        /// </summary>
        public void ClearTargets()
        {
            Targets.Clear();
        }

        private void cleanTargets()
        {
            for (int i = Targets.Count - 1; i >= 0 && !Targets[i]; i--)
                Targets.RemoveAt(i);
        }
    }
}