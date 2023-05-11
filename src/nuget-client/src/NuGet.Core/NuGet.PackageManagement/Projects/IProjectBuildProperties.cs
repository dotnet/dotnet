// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading.Tasks;

namespace NuGet.ProjectManagement
{
    /// <summary>
    /// Represents an API providing read-only access to 
    /// project's build properties.
    /// </summary>
    [Obsolete("This was only used by NuGet's VS extension, so has been moved there")]
    public interface IProjectBuildProperties
    {
        /// <summary>
        /// Returns a property value.
        /// Must be called on the UI thread.
        /// </summary>
        /// <param name="propertyName">A property name</param>
        /// <returns>Property value or <code>null</code> if not found.</returns>
        /// <remarks>Often times when retrieving properties we are already on the UI thread. In those cases, prefer calling this method over the async one to avoid the extra state machine allocations.</remarks>
        string GetPropertyValue(string propertyName);

        /// <summary>
        /// Asynchronous method to retrieve a property value.
        /// </summary>
        /// <param name="propertyName">A property name</param>
        /// <returns>Property value or <code>null</code> if not found.</returns>
        /// <remarks>Often times when retrieving properties we are already on the UI thread. In those cases, prefer calling the synchronous version instead to avoid the extra state machine allocations.</remarks>
        Task<string> GetPropertyValueAsync(string propertyName);
    }
}
