using UnityEditor;

[CustomPropertyDrawer(typeof(ShowMultiplyResultAttribute))]
public class ShowMultiplyResultDrawer : DoubleFloatPropertyDrawer
{
    protected override float GetSecondValue(SerializedProperty property)
    {
        // First get the attribute since it contains the range for the slider
        ShowMultiplyResultAttribute resultAttribute = attribute as ShowMultiplyResultAttribute;

        float value = property.floatValue;

        for (int i = 0; i < resultAttribute.siblingFieldNames.Length; i++)
        {
            var siblingProperty = FindSiblingProperty(property, resultAttribute.siblingFieldNames[i]);
            if (siblingProperty != null && siblingProperty.propertyType == SerializedPropertyType.Float)
            {
                value *= siblingProperty.floatValue;
            }
        }

        var siblingProperty2 = FindSiblingProperty(property, resultAttribute.addToMultipliers);
        if (siblingProperty2 != null && siblingProperty2.propertyType == SerializedPropertyType.Float)
        {
            value *= (1 + siblingProperty2.floatValue);
        }

        return value;
    }

    //ai
    //I think this is my first full vibe code...
    public SerializedProperty FindSiblingProperty(SerializedProperty property, string siblingName)
    {
        if (property == null) return null;

        // Get the full path of the current property (e.g., "myStruct.nestedClass.myField")
        string path = property.propertyPath;

        // Find the index of the last dot
        int lastDot = path.LastIndexOf('.');

        if (lastDot == -1)
        {
            // Root property: Find directly on the main SerializedObject
            return property.serializedObject.FindProperty(siblingName);
        }
        else
        {
            // Nested property: Extract parent path and append the sibling name
            string parentPath = path.Substring(0, lastDot);
            return property.serializedObject.FindProperty($"{parentPath}.{siblingName}");
        }
    }
}
