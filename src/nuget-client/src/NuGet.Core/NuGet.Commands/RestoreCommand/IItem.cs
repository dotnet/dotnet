// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable enable

namespace NuGet.Commands.Restore
{
    /// <summary>
    /// Represents an item and its metadata.
    /// </summary>
    /// <remarks>See <see cref="IProject"/> for context on why this abstraction exists.</remarks>
    public interface IItem
    {
        /// <summary>The item identity, which is the value provided in the Include XML attribute in MSBuild files.</summary>
        /// <remarks>For NuGet, this is typically the package id, but if the item is referencing a file, it will be the absolute file path.</remarks>
        string Identity { get; }

        /// <summary>Get the evaluated value of a metadata on this item.</summary>
        /// <param name="name">The name of the metadata whose value is retrieved.</param>
        /// <returns>The evaluated value of the given metadata for this item, or <see langword="null"/> if no metadata exists with the given name.</returns>
        string GetMetadata(string name);
    }
}
