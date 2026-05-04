using SpellCasting;
using System;
using UnityEditor;
using UnityEngine;

namespace SpellCasting.Editor
{

    [CustomPropertyDrawer(typeof(CommonComponentsHolder), true)]
    class CommonComponentsHolderDrawer : PropertyDrawer
    {
        const string labelFind = "Find Holder";
        const string labelNotFound = "Find Holder (Not Found)";
        bool templabel;
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            var refe = property.objectReferenceValue;
            if (refe == null)
            {
                EditorGUI.indentLevel = 0;

                //add a label and get the position to start the grid
                Rect contentposition = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Keyboard), label);

                var newLabel = new GUIContent(templabel ? labelNotFound : labelFind);
                if (EditorGUI.ToggleLeft(contentposition, newLabel, false))
                {
                    if (FindLabel(property))
                    {
                        templabel = true;
                        EditorApplication.delayCall += () => { templabel = false; };
                    }
                }
            }
            else
            {
                EditorGUI.PropertyField(position, property, label);
            }
            EditorGUI.EndProperty();
        }

        private bool FindLabel(SerializedProperty property)
        {
            var obje = property.serializedObject.targetObject;
            if(((MonoBehaviour)obje).TryGetComponent<CommonComponentsHolder>(out var holder)){
                Undo.RecordObject(obje, "set holder");
                property.objectReferenceValue = holder;
                if(holder)
                {
                    return true;
                }
            }
            return false;
        }
    }
}