// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace NuGet.VisualStudio
{
    /// <summary>
    /// Specifies methods for manipulating a key in the Windows registry.
    /// </summary>
    public interface IRegistryKey
    {
        /// <summary>
        /// Retrieves the specified subkey for read or read/write access.
        /// </summary>
        /// <param name="name">The name or path of the subkey to create or open.</param>
        /// <returns>The subkey requested, or null if the operation failed.</returns>
        IRegistryKey OpenSubKey(string name);

        /// <summary>
        /// Retrieves the value associated with the specified name.
        /// </summary>
        /// <param name="name">The name of the value to retrieve. This string is not case-sensitive.</param>
        /// <returns>The value associated with name, or null if name is not found.</returns>
        object GetValue(string name);

        /// <summary>
        /// Closes the key and flushes it to disk if its contents have been modified.
        /// </summary>
        void Close();
    }
}
