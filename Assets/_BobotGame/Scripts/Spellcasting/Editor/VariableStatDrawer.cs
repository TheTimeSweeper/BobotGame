using SpellCasting;
using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[CustomPropertyDrawer(typeof(VariableNumberStat))]
public class VariableStatDrawer : DoubleFloatPropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var stat = property.boxedValue;
        if (stat == null)
        {
            property.boxedValue = new VariableNumberStat(0);
        }

        base.OnGUI(position, property, label);
    }

    protected override float GetFirstValue(SerializedProperty property)
    {
        //get our value
        VariableNumberStat numberStat = (VariableNumberStat)property.boxedValue;

        return numberStat.DebugGetBaseValue();
    }

    protected override float GetSecondValue(SerializedProperty property)
    {
        //get our value
        VariableNumberStat numberStat = (VariableNumberStat)property.boxedValue;

        return numberStat.DebugGetModifiedValue();
    }

    public override float DrawFirstRect(Rect contentPosition, SerializedProperty property)
    {
        float drawnValue = base.DrawFirstRect(contentPosition, property);

        //get our value
        VariableNumberStat numberStat = (VariableNumberStat)property.boxedValue;

        //we have changed the value in editor
        if (drawnValue != numberStat.DebugGetBaseValue())
        {
            //set the base value
            numberStat.DebugOverrideBaseValue(drawnValue);
            numberStat.DebugSetDirty();
            numberStat.DebugOverrideModifiedValue(numberStat.UpdateValueWithModifiers());

            property.boxedValue = numberStat;
        }

        return drawnValue;
    }
}
