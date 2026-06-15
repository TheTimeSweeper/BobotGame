using System;
#if UNITY_EDITOR
using UnityEngine;
#endif

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
#if UNITY_EDITOR
public class EnumArray : PropertyAttribute
#else
public class EnumArray : Attribute
#endif
{
    public Type EnumType;
    public string[] names;

    public EnumArray(System.Type enumType)
    {
        this.names = System.Enum.GetNames(enumType);
        this.EnumType = enumType;
    }

}