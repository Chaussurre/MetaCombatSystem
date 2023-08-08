using System;
using System.Collections.Generic;
using UnityEngine;

namespace MetaCombatSystem.Skills
{
    /// <summary>
    /// This class handle skills for turn by turn style combats and other similar combat styles.
    /// To use this class, add it to a gameobject representing your skill.
    /// Then, choose how many targets will the skill concern (with a min and a max)
    /// Add effects and filters to decide which targets are valids, and what will the skill do to them.
    /// Then during runtime, add each target to the Skill's list of targets.
    /// Once all the targets are set, call the trigger method and the skill will be cast on those targets.
    /// </summary>
    public sealed class Skill : MonoBehaviour
    {
        /// <summary>
        /// The minimum number of targets that the skill require to function and the maximum number of targets the skill can handle
        /// </summary>
        public Vector2Int MinAndMaxNumberOfTargets = Vector2Int.one;

        /// <summary>
        /// The list of all components of the skill.
        /// </summary>
        public List<SkillComponent> Components = new();

        /// <summary>
        /// When true, user must reinput targets after triggering the spell.
        /// When false, triggering again the spell will reuse the last targets sets
        /// </summary>
        public bool ResetTargetsOnCast;

        /// <summary>
        /// If the caster is targetable, this field must contain the caster's target.
        /// </summary>
        public CombatTarget SelfTarget;

        private List<CombatTarget> Targets;

        /// <summary>
        /// The number of target currently set for the skill
        /// </summary>
        public int TargetCount { get; private set; }

        private void Awake()
        {
            Targets = new(MinAndMaxNumberOfTargets.y);
            for (int i = 0; i < MinAndMaxNumberOfTargets.y; i++)
                Targets.Add(null);
        }

        /// <summary>
        /// Return the target set for the given index. If index is -1, returns the self target instead.
        /// </summary>
        /// <param name="index">the index of the target or -1 for the self target</param>
        /// <returns>The target or the self target</returns>
        public CombatTarget GetTarget(int index)
        {
            if (index == -1)
                return SelfTarget;

            return Targets[index];
        }

        /// <summary>
        /// Set a target to the first position without one.
        /// </summary>
        /// <param name="target">The target to add to the list</param>
        /// <returns>true iff the target was correctly added</returns>
        public bool AddTarget(CombatTarget target)
        {
            return SetTarget(target, firstFreeIndex());
        }

        /// <summary>
        /// Set a target at a given index.
        /// </summary>
        /// <param name="target">The target to add to the list</param>
        /// <param name="index">The index</param>
        /// <returns>true iff the target was correctly added</returns>
        public bool SetTarget(CombatTarget target, int index)
        {
            if (index < 0 || index >= MinAndMaxNumberOfTargets.y)
                return false;

            if (!IsTargetValid(target, index))
                return false;

            if (!Targets[index] && target)
                TargetCount++;
            if (Targets[index] && !target)
                TargetCount--;

            Targets[index] = target;
            return true;

        }

        /// <summary>
        /// Remove all the targets from the list.
        /// </summary>
        public void ClearTargets()
        {
            for (int i = 0; i < Targets.Count; i++)
                Targets[i] = null;

            TargetCount = 0;
        }

        /// <summary>
        /// Check wether or not a target can be added to the first free space.
        /// </summary>
        /// <param name="target">The target to test</param>
        /// <returns>whether or not the target can be added</returns>
        public bool IsTargetValid(CombatTarget target)
        {
            return IsTargetValid(target, firstFreeIndex());
        }

        /// <summary>
        /// Check wether or not a target can be added at the given index
        /// </summary>
        /// <param name="target">the target to test</param>
        /// <param name="index">the index to test</param>
        /// <returns>wether or not the target can be added</returns>
        public bool IsTargetValid(CombatTarget target, int index)
        {
            foreach (var component in Components)
                if (component is SkillEffect effect &&
                    effect.FirstAndLastTargets.x <= index &&
                    effect.FirstAndLastTargets.y >= index &&
                    !effect.IsTargetValid(target, index - effect.FirstAndLastTargets.x))
                    return false;

            return true;
        }

        /// <summary>
        /// Check wether or not the skill can be triggered with the targets of the list.
        /// </summary>
        /// <returns>true if the skill can be triggered</returns>
        public bool ReadyToTrigger()
        {
            if (Targets.Count < MinAndMaxNumberOfTargets.x || Targets.Count > MinAndMaxNumberOfTargets.y)
                return false;

            if (!AreTargetsWellOrdered())
                return false;

            foreach (var component in Components)
                if (!component.CanSkillBeTriggered(this))
                    return false;
            
            return true;
        }

        /// <summary>
        /// Wether or not this skill can afford its non targeted costs. Require either a selfTarget or no costs.
        /// Will send an error if there are costs but no self targets.
        /// </summary>
        /// <returns>True if there are no unaffordable costs.</returns>
        public bool CanAffordCosts()
        {
            foreach (var component in Components)
            {
                if (component is not SkillCost cost)
                    continue;

                if (SelfTarget == null)
                    throw new Exception("Skills with no self targets cannot have costs");

                if (!cost.IsCostAffordable(SelfTarget))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Cast the skill on the targets of the list
        /// </summary>
        public void Trigger()
        {
            if (!ReadyToTrigger())
                return;

            foreach (var effect in Components)
                effect.TriggerComponent(this);

            if (ResetTargetsOnCast)
                ClearTargets();
        }

        private int firstFreeIndex()
        {
            int index = 0;
            while (Targets[index]) index++;
            return index;
        }

        private bool AreTargetsWellOrdered()
        {
            int i = 0;
            for (; i < Targets.Count; i++)
                if (!Targets[i])
                    break;

            for (; i < Targets.Count; i++)
                if (Targets[i])
                    return false;
            
            return true;
        }
    }
}