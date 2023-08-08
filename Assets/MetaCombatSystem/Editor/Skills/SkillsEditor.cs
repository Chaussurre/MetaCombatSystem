using UnityEditor;

namespace MetaCombatSystem.Skills.Editor
{
    [CustomEditor(typeof(Skill)), CanEditMultipleObjects]
    public class SkillsEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            var skill = target as Skill;
            AreAllEffectsSet(skill);
            base.OnInspectorGUI();

            foreach (var component in skill.Components)
                if (component is SkillEffect effect)
                    effect.Skill = skill;
        }
        
        private void AreAllEffectsSet(Skill skill)
        {
            if (skill.Components.TrueForAll(x => x != null))
                return;

            var message = "Some components are null. This will throw an error at runtime.";
            EditorGUILayout.HelpBox(message, MessageType.Error);
            skill.Components.TrueForAll(x => x != null);
        }
    }
}
