using UnityEngine;
using System;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
public class MonoScriptAttribute : PropertyAttribute
{
    public System.Type type;
    public MonoScriptAttribute() { }
    public MonoScriptAttribute(System.Type aType)
    {
        type = aType;
    }
}