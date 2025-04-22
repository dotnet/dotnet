// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

using System.Collections.Generic;

namespace NuGet.Commands.Restore
{
    /// <summary>
    /// Represents all the items and properties for a target framework.
    /// The values provided must include any properties and items added after running the _CollectRestoreInput target.
    /// </summary>
    /// <remarks>See <see cref="IProject"/> for more context why this abstraction exists.</remarks>
    public interface ITargetFramework
    {
        /// <summary>Get the value for a property in the project.</summary>
        /// <param name="propertyName">The name of the property</param>
        /// <returns>The value of the requested property, or <see langword="null"/> if the property was not defined.</returns>
        string GetProperty(string propertyName);

        /// <summary>Get all items of a given type.</summary>
        /// <param name="itemType">The item type. For example, PackageReference, PackageVersion, etc.</param>
        /// <returns>The list of items of the requested type.</returns>
        IReadOnlyList<IItem> GetItems(string itemType);
    }
}
