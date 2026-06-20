using UnityEngine;

public class ShowMultiplyResultAttribute : PropertyAttribute
{
    public string[] siblingFieldNames;
    public string addToMultipliers;

    public ShowMultiplyResultAttribute(params string[] otherName)
    {
        this.siblingFieldNames = otherName;
    }
}
