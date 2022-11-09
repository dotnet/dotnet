// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

using Microsoft.VisualStudio.TestPlatform.CoreUtilities;

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollection;

/// <summary>
/// Session Start event arguments
/// </summary>
[DataContract]
public sealed class SessionStartEventArgs : DataCollectionEventArgs
{
    private readonly IDictionary<string, object?> _properties;

    /// <summary>
    /// Initializes a new instance of the <see cref="SessionStartEventArgs"/> class.
    /// </summary>
    /// <remarks>
    /// Default constructor with empty properties and default DataCollectionContext.
    /// DataCollectionContext with empty session signifies that is it irrelevant in the current context.
    /// </remarks>
    public SessionStartEventArgs()
        : this(new DataCollectionContext(new SessionId(Guid.Empty)), new Dictionary<string, object?>())
    {

    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SessionStartEventArgs"/> class.
    /// </summary>
    /// <remarks>
    /// constructor with properties and default DataCollectionContext.
    /// DataCollectionContext with empty session signifies that is it irrelevant in the current context.
    /// </remarks>
    public SessionStartEventArgs(IDictionary<string, object?> properties)
        : this(new DataCollectionContext(new SessionId(Guid.Empty)), properties)
    {

    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SessionStartEventArgs"/> class.
    /// </summary>
    /// <param name="context">
    /// Context information for the session
    /// </param>
    public SessionStartEventArgs(DataCollectionContext context, IDictionary<string, object?> properties)
        : base(context)
    {
        _properties = properties;
        TPDebug.Assert(!context.HasTestCase, "Session event has test a case context");
    }

    /// <summary>
    /// Gets session start properties enumerator
    /// </summary>
    public IEnumerator<KeyValuePair<string, object?>> GetProperties()
    {
        return _properties.GetEnumerator();
    }

    /// <summary>
    /// Gets property value
    /// </summary>
    /// <param name="property">
    /// Property name
    /// </param>
    public T? GetPropertyValue<T>(string property)
    {
        ValidateArg.NotNullOrEmpty(property, nameof(property));

        return _properties.ContainsKey(property) ? (T?)_properties[property] : default;
    }

    /// <summary>
    /// Gets property value
    /// </summary>
    /// <param name="property">
    /// Property name
    /// </param>
    public object? GetPropertyValue(string property)
    {
        ValidateArg.NotNullOrEmpty(property, nameof(property));

        _properties.TryGetValue(property, out var propertyValue);

        return propertyValue;
    }

}

/// <summary>
/// Session End event arguments
/// </summary>
[DataContract]
public sealed class SessionEndEventArgs : DataCollectionEventArgs
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SessionEndEventArgs"/> class.
    /// </summary>
    /// <remarks>
    /// Default constructor with default DataCollectionContext.
    /// DataCollectionContext with empty session signifies that is it irrelevant in the current context.
    /// </remarks>
    public SessionEndEventArgs() : this(new DataCollectionContext(new SessionId(Guid.Empty)))
    {

    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SessionEndEventArgs"/> class.
    /// </summary>
    /// <param name="context">
    /// Context information for the session
    /// </param>
    public SessionEndEventArgs(DataCollectionContext context)
        : base(context)
    {
        TPDebug.Assert(!context.HasTestCase, "Session event has test a case context");
    }

}
