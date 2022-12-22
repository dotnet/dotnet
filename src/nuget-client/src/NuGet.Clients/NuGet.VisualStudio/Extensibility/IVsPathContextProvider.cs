// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Runtime.InteropServices;

namespace NuGet.VisualStudio
{
    /// <summary>
    /// A factory to initialize <see cref="IVsPathContext"/> instances.
    /// </summary>
    [ComImport]
    [Guid("5BAC7095-F674-4778-8788-E15FFF77F96B")]
    public interface IVsPathContextProvider
    {
        /// <summary>
        /// Attempts to create an instance of <see cref="IVsPathContext"/>.
        /// </summary>
        /// <remarks>Can be called from a background thread.</remarks>
        /// <param name="projectUniqueName">
        /// Unique identificator of the project. Should be a full path to project file.
        /// </param>
        /// <param name="context">The path context associated with given project.</param>
        /// <returns>
        /// <code>True</code> if operation has succeeded and context was created.
        /// False, otherwise, e.g. when provided project is not managed by NuGet.
        /// </returns>
        /// <throws>
        /// <code>ArgumentNullException</code> if projectUniqueName is passed as null.
        /// <code>InvalidOperationException</code> when it fails to create a context and return appropriate error message.
        /// </throws>
        bool TryCreateContext(string projectUniqueName, out IVsPathContext context);
    }
}
