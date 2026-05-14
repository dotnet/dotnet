// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.ComponentModel;
using System.Reflection;

internal static class EnumExtensions
{
    /// <summary>
    /// Returns the value in the Description attribute of an enum value if it exists.
    /// Otherwise returns null.
    /// </summary>
    public static string GetDescription(this Enum value)
    {
        Type type = value.GetType();
        string name = Enum.GetName(type, value);
        if (name != null)
        {
            FieldInfo field = type.GetField(name);
            if (field != null)
            {
                DescriptionAttribute attr = 
                    Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
                    
                if (attr != null)
                {
                    return attr.Description;
                }
            }
        }
        return null;
    }
}