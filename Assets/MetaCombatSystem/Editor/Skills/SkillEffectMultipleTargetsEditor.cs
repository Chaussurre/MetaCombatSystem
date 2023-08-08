using UnityEngine;
using UnityEditor;

namespace MetaCombatSystem.Skills.Editor
{
    [CustomEditor(typeof(SkillEffectMultipleTargets)), CanEditMultipleObjects]
    public class SkillEffectMultipleTargetsEditor : SkillEffectEditor
    {
        public override void OnInspectorGUI()
        {
            var skillEffect = target as SkillEffectMultipleTargets;

            IsSetToASkill(skillEffect);
            AreTargetsCorrect(skillEffect);

            var numTargets = skillEffect.GetNumberOfTargets();
            EditorGUILayout.LabelField($"This effect requires {numTargets} targets");
            var first = EditorGUILayout.IntField("First Target", skillEffect.FirstAndLastTargets.x);
            skillEffect.FirstAndLastTargets = new(first, first + numTargets);

            DrawPropertiesExcluding(serializedObject, "FirstAndLastTargets");
        }
    }
}