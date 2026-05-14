// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.TestPlatform.Extensions.TrxLogger.XML;

/// <summary>
/// Additional data needed to describe a field for automatic xml storage
/// </summary>
[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
internal abstract class StoreXmlAttribute : Attribute
{
    /// <summary>
    /// simple xpath location. only element and attribute names can be used.
    /// </summary>
    public string? Location;

    /// <summary>
    /// Initializes a new instance of the <see cref="StoreXmlAttribute"/> class.
    /// </summary>
    public StoreXmlAttribute()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="StoreXmlAttribute"/> class.
    /// </summary>
    /// <param name="location">
    /// The location.
    /// </param>
    public StoreXmlAttribute(string? location)
    {
        Location = location;
    }
}

/// <summary>
/// Additional info for storing simple fields with default value
/// </summary>
[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
internal sealed class StoreXmlSimpleFieldAttribute : StoreXmlAttribute
{
    public object? DefaultValue;

    /// <summary>
    /// Initializes a new instance of the <see cref="StoreXmlSimpleFieldAttribute"/> class.
    /// </summary>
    public StoreXmlSimpleFieldAttribute()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="StoreXmlSimpleFieldAttribute"/> class.
    /// </summary>
    /// <param name="location">
    /// The location.
    /// </param>
    public StoreXmlSimpleFieldAttribute(string location) : base(location)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="StoreXmlSimpleFieldAttribute"/> class.
    /// </summary>
    /// <param name="defaultValue">
    /// The default value.
    /// </param>
    public StoreXmlSimpleFieldAttribute(object defaultValue) : this(null, defaultValue)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="StoreXmlSimpleFieldAttribute"/> class.
    /// </summary>
    /// <param name="location">
    /// The location.
    /// </param>
    /// <param name="defaultValue">
    /// The default value.
    /// </param>
    public StoreXmlSimpleFieldAttribute(string? location, object defaultValue)
        : base(location)
    {
        DefaultValue = defaultValue;
    }
}

/// <summary>
/// Storing of fields that support IXmlTestStore
/// </summary>
[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
internal sealed class StoreXmlFieldAttribute : StoreXmlAttribute
{
    /// <summary>
    /// If there's no xml for the field a default instance is created or not.
    /// </summary>
    public bool CreateDefaultInstance = CreateDefaultInstanceDefault;

    /// <summary>
    /// Default value
    /// </summary>
    internal static readonly bool CreateDefaultInstanceDefault = true;

    /// <summary>
    /// Initializes a new instance of the <see cref="StoreXmlFieldAttribute"/> class.
    /// </summary>
    public StoreXmlFieldAttribute()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="StoreXmlFieldAttribute"/> class.
    /// </summary>
    /// <param name="location">
    /// The location.
    /// </param>
    /// <param name="createDefaultInstance">
    /// The create default instance.
    /// </param>
    public StoreXmlFieldAttribute(string location, bool createDefaultInstance) : base(location)
    {
        CreateDefaultInstance = createDefaultInstance;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="StoreXmlFieldAttribute"/> class.
    /// </summary>
    /// <param name="location">
    /// The location.
    /// </param>
    public StoreXmlFieldAttribute(string location) : this(location, true)
    {
    }
}
