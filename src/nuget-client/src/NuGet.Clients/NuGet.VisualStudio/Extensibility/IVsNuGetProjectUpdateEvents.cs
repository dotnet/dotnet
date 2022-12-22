// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace NuGet.VisualStudio
{
    /// <summary>
    /// NuGet project update events.
    /// This API provides means of tracking project updates by NuGet.
    /// In particular, for PackageReference projects, updates to the assets file and nuget generated props/targets.
    /// For packages.config projects, package installations will be tracked.
    /// All events are fired from a threadpool thread.
    /// </summary>
    public interface IVsNuGetProjectUpdateEvents
    {
        /// <summary>
        /// Raised when solution restore starts with the list of projects that will be restored.
        /// The list will not include all projects. Some projects may have been skipped in earlier up to date check, and other projects may no-op.
        /// </summary>
        /// <remarks>
        /// Just because a project is being restored that doesn't necessarily mean any actual updates will happen.
        /// No heavy computation should happen in any of these methods as it'll block the NuGet progress.
        /// </remarks>
        event SolutionRestoreEventHandler SolutionRestoreStarted;

        /// <summary>
        /// Raised when solution restore finishes with the list of projects that were restored.
        /// The list will not include all projects. Some projects may have been skipped in earlier up to date check, and other projects may no-op.
        /// </summary>
        /// <remarks>
        /// Just because a project is being restored that doesn't necessarily mean any actual updates will happen.
        /// No heavy computation should happen in any of these methods as it'll block the NuGet progress.
        /// </remarks>
        event SolutionRestoreEventHandler SolutionRestoreFinished;

        /// <summary>
        /// Raised when particular project is about to be updated.
        /// For PackageReference projects, this means an assets file or a nuget temp msbuild file write (nuget.g.props or nuget.g.targets). The list of updated files will include the aforementioned.
        /// If a project was restored, but no file updates happen, this event will not be fired.
        /// For packages.config projects, this means that the project file was changed.
        /// </summary>
        /// <remarks>
        /// No heavy computation should happen in any of these methods as it'll block the NuGet progress.
        /// </remarks>
        event ProjectUpdateEventHandler ProjectUpdateStarted;

        /// <summary>
        /// Raised when particular project update has been completed.
        /// For PackageReference projects, this means an assets file or a nuget temp msbuild file write (nuget.g.props or nuget.g.targets). The list of updated files will include the aforementioned.
        /// If a project was restored, but no file updates happen, this event will not be fired.
        /// For packages.config projects, this means that the project file was changed.
        /// </summary>
        /// <remarks>
        /// No heavy computation should happen in any of these methods as it'll block the NuGet progress.
        /// </remarks>
        event ProjectUpdateEventHandler ProjectUpdateFinished;
    }

    /// <summary>
    /// Defines an event handler delegate for solution restore start and end.
    /// </summary>
    /// <param name="projects">List of projects that will run restore. Never <see langword="null"/>.</param>
    public delegate void SolutionRestoreEventHandler(IReadOnlyList<string> projects);

    /// <summary>
    /// Defines an event handler delegate for project updates.
    /// </summary>
    /// <param name="projectUniqueName">Project full path. Never <see langword="null"/>. </param>
    /// <param name="updatedFiles">NuGet output files that may be updated. Never <see langword="null"/>.</param>
    public delegate void ProjectUpdateEventHandler(string projectUniqueName, IReadOnlyList<string> updatedFiles);
}
