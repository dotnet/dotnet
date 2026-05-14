// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace NuGet.Commands
{
    /// <summary>
    /// Interface that can be used to provide updates about projects that have been restored and actually something was written on disk.
    /// </summary>
    public interface IRestoreProgressReporter
    {
        /// <summary>
        /// Indicates the start of a project update.
        /// This method should only be called only if any of intermediaries have changed, and not when a project has no-oped.
        /// The interval denoted by this method and <see cref="EndProjectUpdate(string, IReadOnlyList{string})"/> is the update of the files on disk only.
        /// All the heavy work that can be done by restore, such as talking to sources, resolving the graph, installing packages is not included.
        /// </summary>
        /// <param name="projectPath">The project path. Never <see langword="null"/>.</param>
        /// <param name="updatedFiles">The relevant files. Ex. assets file. Never <see langword="null"/>.</param>
        void StartProjectUpdate(string projectPath, IReadOnlyList<string> updatedFiles);

        /// <summary>
        /// Indicates the end of a project update.
        /// This method should only be called only if any of intermediaries have changed, and not when a project has no-oped.
        /// The interval denoted by this method and <see cref="StartProjectUpdate(string, IReadOnlyList{string})(string, IReadOnlyList{string})"/> is the update of the files on disk only.
        /// All the heavy work that can be done by restore, such as talking to sources, resolving the graph, installing packages is not included.
        /// </summary>
        /// <param name="projectPath">The project path. Never <see langword="null"/>.</param>
        /// <param name="updatedFiles">The relevant files. Ex. assets file. Never <see langword="null"/>.</param>
        void EndProjectUpdate(string projectPath, IReadOnlyList<string> updatedFiles);
    }
}
