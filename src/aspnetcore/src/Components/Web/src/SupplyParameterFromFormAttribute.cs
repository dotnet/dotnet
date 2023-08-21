// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Microsoft.AspNetCore.Components;

/// <summary>
/// Indicates that the value of the associated property should be supplied from
/// the form data for the form with the specified name.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
public sealed class SupplyParameterFromFormAttribute : CascadingParameterAttributeBase
{
    /// <summary>
    /// Gets or sets the name for the parameter. The name is used to determine
    /// the prefix to use to match the form data and decide whether or not the
    /// value needs to be bound.
    /// </summary>
    public override string? Name { get; set; }

    /// <summary>
    /// Gets or sets the name for the handler. The name is used to match
    /// the form data and decide whether or not the value needs to be bound.
    /// </summary>
    public string? Handler { get; set; }

    /// <inheritdoc />
    internal override bool SingleDelivery => true;
}
