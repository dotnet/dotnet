// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Reflection;
using System.Runtime.Serialization;

using Microsoft.VisualStudio.TestPlatform.Utilities;

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel;

public delegate bool ValidateValueCallback(object? value);

[DataContract]
public class TestProperty : IEquatable<TestProperty>
{
    private static readonly ConcurrentDictionary<string, Type> TypeCache = new();

    private static bool DisableFastJson { get; set; } = FeatureFlag.Instance.IsSet(FeatureFlag.DISABLE_FASTER_JSON_SERIALIZATION);

    private Type _valueType;

    //public static Stopwatch

    /// <summary>
    /// Initializes a new instance of the <see cref="TestProperty"/> class.
    /// </summary>
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private TestProperty()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {
        // Default constructor for Serialization.
    }

    private TestProperty(string id, string label, string category, string description, Type valueType, ValidateValueCallback? validateValueCallback, TestPropertyAttributes attributes)
    {
        ValidateArg.NotNullOrEmpty(id, nameof(id));
        ValidateArg.NotNull(label, nameof(label));
        ValidateArg.NotNull(category, nameof(category));
        ValidateArg.NotNull(description, nameof(description));
        ValidateArg.NotNull(valueType, nameof(valueType));

        // If the type of property is unexpected, then fail as otherwise we will not be to serialize it over the wcf channel and serialize it in db.
        if (valueType == typeof(KeyValuePair<string, string>[]))
        {
            ValueType = "System.Collections.Generic.KeyValuePair`2[[System.String],[System.String]][]";
        }
        else if (valueType == typeof(string)
                 || valueType == typeof(Uri)
                 || valueType == typeof(string[])
                 || valueType.AssemblyQualifiedName!.Contains("System.Private")
                 || valueType.AssemblyQualifiedName.Contains("mscorlib"))
        {
            // This comparison is a check to ensure assembly information is not embedded in data.
            // Use type.FullName instead of type.AssemblyQualifiedName since the internal assemblies
            // are different in desktop and coreclr. Thus AQN in coreclr includes System.Private.CoreLib which
            // is not available on the desktop.
            // Note that this doesn't handle generic types. Such types will fail during serialization.
            ValueType = valueType.FullName!;
        }
        else if (valueType.GetTypeInfo().IsValueType)
        {
            // In case of custom types, let the assembly qualified name be available to help
            // deserialization on the client.
            ValueType = valueType.AssemblyQualifiedName;
        }
        else
        {
            throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.Resources.UnexpectedTypeOfProperty, valueType, id));
        }

        Id = id;
        Label = label;
        Category = category;
        Description = description;
        ValidateValueCallback = validateValueCallback;
        Attributes = attributes;
        _valueType = valueType;
    }

    /// <summary>
    /// Gets or sets the Id for the property.
    /// </summary>
    [DataMember]
    public string Id { get; set; }

    /// <summary>
    /// Gets or sets a label for the property.
    /// </summary>
    [DataMember]
    public string Label { get; set; }

    /// <summary>
    /// Gets or sets a category for the property.
    /// </summary>
    [DataMember]
    public string Category { get; set; }

    /// <summary>
    /// Gets or sets a description for the property.
    /// </summary>
    [DataMember]
    public string Description { get; set; }

    /// <summary>
    /// Gets the callback for validation of property value.
    /// </summary>
    /// <remarks>This property is not required at the client side.</remarks>
    [IgnoreDataMember]
    public ValidateValueCallback? ValidateValueCallback { get; }

    /// <summary>
    /// Gets or sets the attributes for this property.
    /// </summary>
    [DataMember]
    public TestPropertyAttributes Attributes { get; set; }

    /// <summary>
    /// Gets or sets a string representation of the type for value.
    /// </summary>
    [DataMember]
    public string ValueType { get; set; }

    #region IEquatable

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        return base.Equals(obj as TestProperty);
    }

    /// <inheritdoc/>
    public bool Equals(TestProperty? other)
    {
        return (other != null) && (Id == other.Id);
    }

    #endregion IEquatable

    /// <inheritdoc/>
    public override string ToString()
    {
        return Id;
    }

    /// <summary>
    /// Gets the valueType.
    /// </summary>
    /// <remarks>Only works for the valueType that is in the currently executing assembly or in Mscorlib.dll. The default valueType is of string valueType.</remarks>
    /// <returns>The valueType of the test property</returns>
    public Type GetValueType()
    {
        _valueType ??= GetType(ValueType);

        return _valueType;
    }

    private Type GetType(string typeName)
    {
        ValidateArg.NotNull(typeName, nameof(typeName));

        if (!DisableFastJson && TypeCache.TryGetValue(typeName, out var t))
        {
            return t;
        }

        Type? type = null;

        try
        {
            // This only works for the type is in the currently executing assembly or in Mscorlib.dll.
            type = Type.GetType(typeName);

            if (!DisableFastJson)
            {
                if (type != null)
                {
                    TypeCache.TryAdd(typeName, type);
                    return type;
                }
            }

            // Try 2.0 version as discovery returns version of 4.0 for all cases
            type ??= Type.GetType(typeName.Replace("Version=4.0.0.0", "Version=2.0.0.0"));

            // For UAP the type namespace for System.Uri,System.TimeSpan and System.DateTimeOffset differs from the desktop version.
            if (type == null && typeName.StartsWith("System.Uri"))
            {
                type = typeof(Uri);
            }
            else if (type == null && typeName.StartsWith("System.TimeSpan"))
            {
                type = typeof(TimeSpan);
            }
            else if (type == null && typeName.StartsWith("System.DateTimeOffset"))
            {
                type = typeof(DateTimeOffset);
            }
            else if (type == null && typeName.StartsWith("System.Int16"))
            {
                // For LineNumber property - Int is required
                type = typeof(Int16);
            }
            else if (type == null && typeName.StartsWith("System.Int32"))
            {
                type = typeof(Int32);
            }
            else if (type == null && typeName.StartsWith("System.Int64"))
            {
                type = typeof(Int64);
            }
        }
        catch (Exception)
        {
#if FullCLR
            // Try to see if the typeName contains Windows Phone PKT in that case load it from
            // desktop side
            if (typeName.Contains(s_windowsPhonePKT))
            {
                type = GetType(typeName.Replace(s_windowsPhonePKT, s_visualStudioPKT));
            }

            if (type == null)
            {
                System.Diagnostics.Debug.Fail("The test property type " + typeName + " of property " + Id + "is not supported.");
#else
            System.Diagnostics.Debug.WriteLine("The test property type " + typeName + " of property " + Id + "is not supported.");
#endif
#if FullCLR
            }
#endif
        }
        finally
        {
            // default is of string type.
            type ??= typeof(string);
        }

        if (!DisableFastJson)
        {
            TypeCache.TryAdd(typeName, type);
        }
        return type;
    }

    private static readonly Dictionary<string, KeyValuePair<TestProperty, HashSet<Type>>> Properties = new();

#if FullCLR
    private static string s_visualStudioPKT = "b03f5f7f11d50a3a";
    private static string s_windowsPhonePKT = "7cec85d7bea7798e";
#endif

    public static void ClearRegisteredProperties()
    {
        lock (Properties)
        {
            Properties.Clear();
        }
    }

    public static TestProperty? Find(string id)
    {
        ValidateArg.NotNull(id, nameof(id));

        TestProperty? result = null;

        lock (Properties)
        {
            if (Properties.TryGetValue(id, out var propertyTypePair))
            {
                result = propertyTypePair.Key;
            }
        }

        return result;
    }

    public static TestProperty Register(string id, string label, Type valueType, Type owner)
    {
        ValidateArg.NotNullOrEmpty(id, nameof(id));
        ValidateArg.NotNull(label, nameof(label));
        ValidateArg.NotNull(valueType, nameof(valueType));
        ValidateArg.NotNull(owner, nameof(owner));

        return Register(id, label, string.Empty, string.Empty, valueType, null, TestPropertyAttributes.None, owner);
    }

    public static TestProperty Register(string id, string label, Type valueType, TestPropertyAttributes attributes, Type owner)
    {
        ValidateArg.NotNullOrEmpty(id, nameof(id));
        ValidateArg.NotNull(label, nameof(label));
        ValidateArg.NotNull(valueType, nameof(valueType));
        ValidateArg.NotNull(owner, nameof(owner));

        return Register(id, label, string.Empty, string.Empty, valueType, null, attributes, owner);
    }

    public static TestProperty Register(string id, string label, string category, string description, Type valueType, ValidateValueCallback? validateValueCallback, TestPropertyAttributes attributes, Type owner)
    {
        ValidateArg.NotNullOrEmpty(id, nameof(id));
        ValidateArg.NotNull(label, nameof(label));
        ValidateArg.NotNull(category, nameof(category));
        ValidateArg.NotNull(description, nameof(description));
        ValidateArg.NotNull(valueType, nameof(valueType));
        ValidateArg.NotNull(owner, nameof(owner));

        TestProperty result;

        lock (Properties)
        {
            if (Properties.TryGetValue(id, out var propertyTypePair))
            {
                // verify the data valueType is valid
                if (propertyTypePair.Key.ValueType == valueType.AssemblyQualifiedName
                    || propertyTypePair.Key.ValueType == valueType.FullName
                    || propertyTypePair.Key._valueType == valueType)
                {
                    // add the owner to set of owners for this GraphProperty object
                    propertyTypePair.Value.Add(owner);
                    result = propertyTypePair.Key;
                }
                else
                {
                    // not the data valueType we expect, throw an exception
                    string message = string.Format(
                        CultureInfo.CurrentCulture,
                        Resources.Resources.Exception_RegisteredTestPropertyHasDifferentValueType,
                        id,
                        valueType.ToString(),
                        propertyTypePair.Key.ValueType);

                    throw new InvalidOperationException(message);
                }
            }
            else
            {
                // create a new TestProperty object
                result = new TestProperty(id, label, category, description, valueType, validateValueCallback, attributes);

                // setup the data pair used to track owners of this GraphProperty
                propertyTypePair = new KeyValuePair<TestProperty, HashSet<Type>>(result, new HashSet<Type>());
                propertyTypePair.Value.Add(owner);

                // add to the dictionary
                Properties[id] = propertyTypePair;
            }
        }

        return result;
    }

    public static bool TryUnregister(string id, out KeyValuePair<TestProperty, HashSet<Type>> propertyTypePair)
    {
        ValidateArg.NotNullOrEmpty(id, nameof(id));

        lock (Properties)
        {
            if (Properties.TryGetValue(id, out propertyTypePair))
            {
                return Properties.Remove(id);
            }
        }
        return false;
    }

    [SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Part of the public API")]
    public object GetRealObject(StreamingContext context)
    {
        var registeredProperty = Find(Id);
        registeredProperty ??= Register(
                Id,
                Label,
                Category,
                Description,
                GetValueType(),
                ValidateValueCallback,
                Attributes,
                typeof(TestObject));

        return registeredProperty;
    }
}
