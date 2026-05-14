// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using OpenTelemetry.Internal;

namespace OpenTelemetry.OpAmp.Client.Settings;

/// <summary>
/// Configuration settings for identifying the client to the OpAMP server.
/// </summary>
public sealed class IdentificationSettings
{
    /// <summary>
    /// Gets the collection of resources associated with the current instance.
    /// </summary>
    public Dictionary<string, AnyValueUnion> IdentifyingResources { get; } = [];

    /// <summary>
    /// Gets the collection of resources associated with the current instance.
    /// </summary>
    public Dictionary<string, AnyValueUnion> NonIdentifyingResources { get; } = [];

    /// <summary>
    /// Adds an itentifying attribute to the resources collection.
    /// </summary>
    /// <param name="key">The unique key associated with the resource.</param>
    /// <param name="value">The value of the resource to be added.</param>
    public void AddIdentifyingAttribute(string key, string value)
    {
        Guard.ThrowIfNullOrEmpty(key, nameof(key));

        this.IdentifyingResources[key] = AnyValueUnion.From(value);
    }

    /// <summary>
    /// Adds an itentifying attribute to the resources collection.
    /// </summary>
    /// <param name="key">The unique key associated with the resource.</param>
    /// <param name="value">The value of the resource to be added.</param>
    public void AddIdentifyingAttribute(string key, int value)
    {
        Guard.ThrowIfNullOrEmpty(key, nameof(key));

        this.IdentifyingResources[key] = AnyValueUnion.From(value);
    }

    /// <summary>
    /// Adds an itentifying attribute to the resources collection.
    /// </summary>
    /// <param name="key">The unique key associated with the resource.</param>
    /// <param name="value">The value of the resource to be added.</param>
    public void AddIdentifyingAttribute(string key, double value)
    {
        Guard.ThrowIfNullOrEmpty(key, nameof(key));

        this.IdentifyingResources[key] = AnyValueUnion.From(value);
    }

    /// <summary>
    /// Adds an itentifying attribute to the resources collection.
    /// </summary>
    /// <param name="key">The unique key associated with the resource.</param>
    /// <param name="value">The value of the resource to be added.</param>
    public void AddIdentifyingAttribute(string key, bool value)
    {
        Guard.ThrowIfNullOrEmpty(key, nameof(key));

        this.IdentifyingResources[key] = AnyValueUnion.From(value);
    }

    /// <summary>
    /// Adds a non itentifying attribute to the resources collection.
    /// </summary>
    /// <param name="key">The unique key associated with the resource.</param>
    /// <param name="value">The value of the resource to be added.</param>
    public void AddNonIdentifyingAttribute(string key, string value)
    {
        Guard.ThrowIfNullOrEmpty(key, nameof(key));

        this.NonIdentifyingResources[key] = AnyValueUnion.From(value);
    }

    /// <summary>
    /// Adds a non itentifying attribute to the resources collection.
    /// </summary>
    /// <param name="key">The unique key associated with the resource.</param>
    /// <param name="value">The value of the resource to be added.</param>
    public void AddNonIdentifyingAttribute(string key, int value)
    {
        Guard.ThrowIfNullOrEmpty(key, nameof(key));

        this.NonIdentifyingResources[key] = AnyValueUnion.From(value);
    }

    /// <summary>
    /// Adds a non itentifying attribute to the resources collection.
    /// </summary>
    /// <param name="key">The unique key associated with the resource.</param>
    /// <param name="value">The value of the resource to be added.</param>
    public void AddNonIdentifyingAttribute(string key, double value)
    {
        Guard.ThrowIfNullOrEmpty(key, nameof(key));

        this.NonIdentifyingResources[key] = AnyValueUnion.From(value);
    }

    /// <summary>
    /// Adds a non itentifying attribute to the resources collection.
    /// </summary>
    /// <param name="key">The unique key associated with the resource.</param>
    /// <param name="value">The value of the resource to be added.</param>
    public void AddNonIdentifyingAttribute(string key, bool value)
    {
        Guard.ThrowIfNullOrEmpty(key, nameof(key));

        this.NonIdentifyingResources[key] = AnyValueUnion.From(value);
    }
}
