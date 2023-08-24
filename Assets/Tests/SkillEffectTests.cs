using NUnit.Framework;
using UnityEngine;
using System.Reflection;

namespace MetaCombatSystem.Skills.Tests
{
    public class SkillEffectTests
    {
        Skill skill;
        MockSkillEffect skillEffect;
        MockSkillEffectNoValidTarget noValidTargetsSkillEffect;
        SkillCast skillCast;
        CombatTarget skillTarget1;
        CombatTarget skillTarget2;

        [SetUp]
        public void SetUp()
        {
            skill = new GameObject("skill").AddComponent<Skill>();
            skill.MinAndMaxNumberOfTargets = new(1, 2);

            skillCast = new(skill);

            skillEffect = new GameObject("skill effect").AddComponent<MockSkillEffect>();
            skillEffect.FirstAndLastTargets = new(0, 1);
            skill.Components = new() { skillEffect };
            
            noValidTargetsSkillEffect = new GameObject("no valid Targers").AddComponent<MockSkillEffectNoValidTarget>();
            noValidTargetsSkillEffect.FirstAndLastTargets = new(0, 1);

            skillTarget1 = new GameObject("target 1").AddComponent<CombatTarget>();
            skillTarget2 = new GameObject("target 2").AddComponent<CombatTarget>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(skill.gameObject);
            Object.DestroyImmediate(skillEffect.gameObject);
            Object.DestroyImmediate(noValidTargetsSkillEffect.gameObject);
            Object.DestroyImmediate(skillTarget1.gameObject);
            Object.DestroyImmediate(skillTarget2.gameObject);
        }

        [Test]
        public void EffectTriggerOnTargets()
        {
            skillEffect.TriggerEffect(skillTarget1);
            Assert.AreEqual(1, skillEffect.TriggerCount, "EffectTrigger should be called once");
            Assert.AreEqual(skillTarget1, skillEffect.LastTriggeredTarget, "Last target should be the first target");

            skillEffect.TriggerEffect(skillTarget2);
            Assert.AreEqual(2, skillEffect.TriggerCount, "EffectTrigger should be called twice");
            Assert.AreEqual(skillTarget2, skillEffect.LastTriggeredTarget, "Last target should be the second target");
        }

        [Test]
        public void TriggerSkill()
        {
            skillCast.SetTarget(skillTarget1, 0);
            skillCast.SetTarget(skillTarget2, 1);

            Assert.IsTrue(skill.ReadyToTrigger(skillCast), "Skill should be ready to trigger on two targets");

            skill.Trigger(skillCast);

            Assert.AreEqual(2, skillEffect.TriggerCount, "EffectTrigger should be called twice");
            Assert.AreSame(skillTarget2, skillEffect.LastTriggeredTarget, "Last target should be the second target");
        }

        [Test]
        public void IsTargetValid()
        {
            Assert.IsTrue(skillEffect.IsTargetValid(skillTarget1), "IsTargetValid should return true by default.");
        }

        [Test]
        public void InvalidTarget()
        {
            Assert.IsFalse(noValidTargetsSkillEffect.IsTargetValid(skillTarget2), "IsTargetValid should be overriden to return false");
        }


        [Test]
        public void SkillInvalidTargets()
        {
            skill.Components.Add(noValidTargetsSkillEffect);

            skillCast.SetTarget(skillTarget1, 0);
            skillCast.SetTarget(skillTarget2, 1);

            Assert.IsFalse(skill.ReadyToTrigger(skillCast), "Skill should not be ready to trigger on two targets");
        }

        [Test]
        public void SkillSeveralCasts()
        {
            skillCast.SetTarget(skillTarget1, 0);

            //first cast
            Assert.IsTrue(skill.ReadyToTrigger(skillCast), "Skill should be ready to trigger on one target");
            Assert.IsFalse(skillEffect.finishedEffect);
            skill.Trigger(skillCast);
            Assert.IsTrue(skillEffect.finishedEffect);
            Assert.AreEqual(1, skillEffect.TriggerCount, "EffectTrigger should be called once");

            //second cast
            Assert.IsTrue(skill.ReadyToTrigger(skillCast), "Skill should be ready to trigger on one target");
            skill.Trigger(skillCast);
            Assert.IsTrue(skillEffect.finishedEffect);
            Assert.AreEqual(1, skillEffect.TriggerCount, "EffectTrigger should be called once");
        }

        // Define a mock SkillEffectMonoTarget class for testing
        private class MockSkillEffect : SkillEffect
        {
            public int TriggerCount = 0;
            public CombatTarget LastTriggeredTarget = null;
            public bool finishedEffect = false;

            public override void SetUpEffect()
            {
                TriggerCount = 0;
                LastTriggeredTarget = null;
                finishedEffect = false;
            }

            public override void FinishEffect()
            {
                finishedEffect = true;
            }

            public override bool IsTargetValid(CombatTarget target)
            {
                return true;
            }

            public override void TriggerEffect(CombatTarget combatTarget)
            {
                TriggerCount++;
                LastTriggeredTarget = combatTarget;
            }
        }

        private class MockSkillEffectNoValidTarget : SkillEffect
        {
            public bool called = false;

            public override void TriggerEffect(CombatTarget target)
            {
                called = true;
            }

            public override bool IsTargetValid(CombatTarget target)
            {
                return false;
            }
        }
    }
}
