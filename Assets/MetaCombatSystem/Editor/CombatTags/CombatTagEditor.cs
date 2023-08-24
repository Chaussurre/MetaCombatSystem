using UnityEditor;
using UnityEngine;

namespace MetaCombatSystem.Editor
{
    [CustomPropertyDrawer(typeof(CombatTag))]
    public class CombatTagEditor : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return 2 * EditorGUIUtility.singleLineHeight;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var categoryProp = property.FindPropertyRelative("Category");
            
            var rect1 = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            EditorGUI.ObjectField(rect1, categoryProp, typeof(CombatTagCategory));

            if (categoryProp.objectReferenceValue != null)
            {
                var stringProp = property.FindPropertyRelative("Tag");

                var options = (categoryProp.objectReferenceValue as CombatTagCategory).Tags;

                int index = 0;
                for (; index < options.Count; index++)
                    if (options[index] == stringProp.stringValue)
                        break;
                if (index == options.Count) index = 0;

                var rect2 = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight,
                    position.width, EditorGUIUtility.singleLineHeight);
                index = EditorGUI.Popup(rect2, label.text, index, options.ToArray());

                stringProp.stringValue = options[index];
            }
        }

    }
}