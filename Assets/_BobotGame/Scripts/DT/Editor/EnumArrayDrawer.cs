using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(EnumArray))]
public class EnumArrayDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EnumArray enumNames = attribute as EnumArray;

        int index = System.Convert.ToInt32(property.propertyPath.Substring(property.propertyPath.IndexOf("[")).Replace("[", "").Replace("]", ""));

        //change the label
        label.text = enumNames.names[index];

        //draw field
        EditorGUI.PropertyField(position, property, label, true);
    }
}