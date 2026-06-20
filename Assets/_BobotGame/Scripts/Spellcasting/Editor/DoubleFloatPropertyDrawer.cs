using UnityEditor;
using UnityEngine;

public abstract class DoubleFloatPropertyDrawer : PropertyDrawer
{
    protected float getFraction => 0.5f;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        //EditorGUI.indentLevel = 0;
        //add a label and get the position of where the field would normally be
        Rect contentPosition = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Keyboard), label);

        //draws a rect on the left half of the usual field location
        DrawFirstRect(contentPosition, property);
        DrawSecondRect(contentPosition, property);

        EditorGUI.EndProperty();
    }

    protected virtual float GetFirstValue(SerializedProperty property)
    {
        return property.floatValue;
    }
    protected abstract float GetSecondValue(SerializedProperty property);

    public virtual float DrawFirstRect(Rect contentPosition, SerializedProperty property)
    {
        Rect firstHalfRect = contentPosition;
        firstHalfRect.width *= getFraction;
        float value = EditorGUI.FloatField(firstHalfRect, GetFirstValue(property));
        if (property.propertyType == SerializedPropertyType.Float)
        {
            property.floatValue = value;
        }
        return value;
    }

    public virtual float DrawSecondRect(Rect contentPosition, SerializedProperty property)
    {
        Rect secondHalfRect = contentPosition;
        secondHalfRect.width *= getFraction;
        secondHalfRect.x += contentPosition.width * (1 - getFraction);

        return EditorGUI.FloatField(secondHalfRect, GetSecondValue(property));
    }
}