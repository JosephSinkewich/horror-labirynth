#if UNITY_EDITOR
using Game;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(TagSelectorAttribute))]
public class TagSelectorPropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (property.propertyType != SerializedPropertyType.String)
        {
            EditorGUI.LabelField(position, label.text, "Use TagSelector with string fields only.");
            return;
        }

        property.stringValue = EditorGUI.TagField(position, label, property.stringValue);
    }
}
#endif
