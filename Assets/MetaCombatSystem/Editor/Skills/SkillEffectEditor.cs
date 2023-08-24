﻿using System;
using UnityEditor;

namespace MetaCombatSystem.Skills.Editor
{
    [CustomEditor(typeof(SkillEffect), true, isFallback = true), CanEditMultipleObjects]
    public class SkillEffectEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            var effect = target as SkillEffect;

            AreTargetsCorrect(effect);

            base.OnInspectorGUI();
        }

        protected void AreTargetsCorrect(SkillEffect effect)
        {
            if (effect.Skill == null)
                return;

            if (effect.FirstAndLastTargets.x < -1)
            {
                var message = "Targets must either be equal to -1, or be equal to a valid index.";
                EditorGUILayout.HelpBox(message, MessageType.Error);
                return;
            }

            if (effect.FirstAndLastTargets.x > effect.FirstAndLastTargets.y)
            {
                var message = "First target cannot be a higher index than last";
                EditorGUILayout.HelpBox(message, MessageType.Error);
                return;
            }

            if (effect.requireAllTargets && effect.Skill.MinAndMaxNumberOfTargets.x <= effect.FirstAndLastTargets.y)
            {
                var message = "This effect is set as requiring all targets yet some targets are set as optional.";
                EditorGUILayout.HelpBox(message, MessageType.Error);
                return;
            }

            if (effect.Skill && effect.FirstAndLastTargets.y >= effect.Skill.MinAndMaxNumberOfTargets.y)
            {
                var message = "Some target indexes do not exist in the corresponding skill";
                EditorGUILayout.HelpBox(message, MessageType.Error);
                return;
            }
        }
    }
}