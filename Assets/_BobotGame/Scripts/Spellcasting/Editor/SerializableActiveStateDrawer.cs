using ActiveStates;
using SpellCasting;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
[CustomPropertyDrawer(typeof(SerializableActiveState))]
public class SerializableActiveStateDrawer : PropertyDrawer
{
    string field;

    public static IEnumerable<System.Type> allStates;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        EditorGUI.indentLevel = 0;

        SerializableActiveState state = (SerializableActiveState)property.boxedValue;

        allStates = ActiveStateCatalog.FindAllStateTypes();

        //todo bobot probably don't use reflection and two giant foreach loops every single ongui
        bool found = false;
        string foundName = "";
        foreach (System.Type t in allStates)
        {
            if (state.activeStateName == t.FullName)
            {
                foundName = t.Name;
                found = true; 
                break;
            }
        }

        label.text = found ? $"{label.text} ({foundName})" : $"{label.text} (Invalid)";
        
        var guiStyle = new GUIStyle(EditorStyles.label);
        if (!found)
        {
            guiStyle.normal.textColor = Color.red;
        }

        //add a label and get the position to start the grid
        Rect contentposition = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Keyboard), label, guiStyle);

        field = EditorGUI.TextField(contentposition, state.activeStateName);

        state.activeStateName = field;
        found = false;
        foreach (System.Type t in allStates)
        {
            if (GetPartialMatch(t.FullName, field))
            {
                state.activeStateName = t.FullName;
                found = true;
                break;
            }
        }
        property.boxedValue = state;

        //var newLabel = new GUIContent(templabel ? labelNotFound : labelFind);
        //if (EditorGUI.ToggleLeft(contentposition, newLabel, false))
        //{
        //    if (FindLabel(property))
        //    {
        //        templabel = true;
        //        EditorApplication.delayCall += () => { templabel = false; };
        //    }
        //}
    }

    private static bool GetPartialMatch(string fullName, string field)
    {
        var fullName2 = fullName.ToLowerInvariant();
        var field2 = field.ToLowerInvariant();
        if (fullName2.Contains(field2))
        {
            return true;
        }
        return false;
    }
}
