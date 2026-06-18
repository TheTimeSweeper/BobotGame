using ActiveStates;
using SpellCasting;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.UIElements;
[CustomPropertyDrawer(typeof(SerializableActiveState))]
public class SerializableActiveStateDrawer : PropertyDrawer
{
    public static IEnumerable<System.Type> allStates;
    
    string field;
    bool wasFocused;
    
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        EditorGUI.indentLevel = 0;

        SerializableActiveState state = (SerializableActiveState)property.boxedValue;

        allStates = ActiveStateCatalog.FindAllStateTypes();

        //todo bobot probably don't use reflection and two giant foreach loops every single ongui
        bool preFind = false;
        string foundName = "";
        foreach (System.Type t in allStates)
        {
            if (state.activeStateName == t.FullName)
            {
                foundName = t.Name;
                preFind = true; 
                break;
            }
        }

        label.text = preFind ? $"{label.text} ({foundName})" : $"{label.text} (Invalid)";
        
        var guiStyle = new GUIStyle(EditorStyles.label);
        if (!preFind)
        {
            guiStyle.normal.textColor = Color.red;
        }

        //add a label and get the position to start the grid
        Rect contentposition = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Keyboard), label, guiStyle);

        //ai 3
        string myControlName = "MyTextField_" + property.propertyPath;
        GUI.SetNextControlName(myControlName);

        if (wasFocused)
        {
            field = EditorGUI.TextField(contentposition, field);
        } 
        else
        {
            field = EditorGUI.TextField(contentposition, state.activeStateName);
        }

        ////ai -- didn't work
        //// Generate a unique ID for this property field
        //int controlID = GUIUtility.GetControlID(FocusType.Keyboard);

        //// Check if this property is currently focused/highlighted
        //bool isFocused = (GUIUtility.keyboardControl == controlID-1);
        ////ai 2 -- didn't work
        //bool isFocused = EditorGUIUtility.editingTextField;
        //ai 3
        bool isFocused = GUI.GetNameOfFocusedControl() == myControlName;
        wasFocused = isFocused;


        bool found = false;
        
        string newField = "";
        string matchList = "";
        int matches = 0;
        foreach (System.Type t in allStates)
        {
            if (GetPartialMatch(t.FullName, field))
            {
                if (!found)
                {
                    found = true;
                    newField = t.FullName;
                    matchList = $"-->{t.FullName.Replace("ActiveStates.", "")}";
                    matches = 1;
                }
                else
                {
                    if (matches < 3)
                    {
                        matchList += "\n" + t.FullName.Replace("ActiveStates.", "");
                        matches++;
                    }
                    if (matches >= 3)
                    {
                        break;
                    }
                }
            }
        }
        if (!found)
        {
            state.activeStateName = newField;
            property.boxedValue = state;
        }

        if (matches > 0 && isFocused)
        {
            Rect window = position;
            window.y -= 20 * matches;
            window.height *= matches;
            //window.width *= 0.7f;
            EditorGUI.DrawRect(window, Color.gray);

            var guiStyle2 = new GUIStyle(EditorStyles.label);
            guiStyle2.normal.textColor = Color.black;
            EditorGUI.LabelField(window, new GUIContent(matchList), guiStyle2);
        }
        if (!isFocused)
        {
            state.activeStateName = newField;
            property.boxedValue = state;
        }
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
