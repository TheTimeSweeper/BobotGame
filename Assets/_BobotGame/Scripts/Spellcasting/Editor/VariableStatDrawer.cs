using SpellCasting;
using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[CustomPropertyDrawer(typeof(VariableNumberStat))]
public class VariableStatDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        //EditorGUI.indentLevel = 0;
        var stat = property.boxedValue;
        if(stat == null)
        {
            property.boxedValue = new VariableNumberStat(0);
        }
        VariableNumberStat numberStat = (VariableNumberStat)property.boxedValue;

        //add a label and get the position to start the grid
        Rect contentposition = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Keyboard), label);
        Rect firstHalfRect = contentposition;
        firstHalfRect.width *= 0.5f;
        //firstHalfRect.x -= firstHalfRect.width * 0.5f;
        Rect secondHalfRect = firstHalfRect;
        secondHalfRect.x += secondHalfRect.width;

        var value = EditorGUI.FloatField(firstHalfRect, numberStat.DebugGetBaseValue());
        if (value != numberStat.DebugGetBaseValue())
        {
            numberStat.DebugOverrideBaseValue(value);
        }
        EditorGUI.FloatField(secondHalfRect, numberStat.Value);
        property.boxedValue = numberStat;
        EditorGUI.EndProperty();
    }
}
