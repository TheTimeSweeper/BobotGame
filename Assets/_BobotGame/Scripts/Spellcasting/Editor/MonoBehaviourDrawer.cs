using UnityEditor;
using UnityEngine;

namespace SpellCasting.Editor
{

    [CustomPropertyDrawer(typeof(Component), true)]
    public class FindComponentDrawer : PropertyDrawer
    {
        public const string labelFind = "Find";
        public const string labelNotFound = "Nope";

        private const float offset = 45;

        bool templabel;
        bool showing;
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            var refe = property.objectReferenceValue;
            if (refe == null)
            {
                Rect contentPosition = position;
                position.xMax -= offset;
                contentPosition.xMin = position.xMax;

                EditorGUI.PropertyField(position, property, label);

                var findLable = new GUIContent(templabel ? labelNotFound : labelFind);
                if (EditorGUI.ToggleLeft(contentPosition, findLable, false))
                {
                    if (FindComponent(property))
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

        private bool FindComponent(SerializedProperty property)
        {
            var obje = property.serializedObject.targetObject;

            Component[] components = ((MonoBehaviour)obje).GetComponentsInChildren<Component>(true);
            for (int i = 0; i < components.Length; i++)
            {
                var comp = components[i];
                property.objectReferenceValue = comp;

                if (property.objectReferenceValue == null)
                    continue;

                Undo.RecordObject(obje, "set holder");
                property.objectReferenceValue = comp;
                if (comp)
                {
                    return true;
                }
            }
            return false;
        }
    }
}