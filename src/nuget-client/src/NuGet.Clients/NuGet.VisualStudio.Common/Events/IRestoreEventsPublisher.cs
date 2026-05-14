// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace NuGet.VisualStudio
{
    /// <summary>
    /// The interface for publishing restore-related events. The event consumers use <see cref="IRestoreEvents"/> to
    /// listen to events.
    /// </summary>
    public interface IRestoreEventsPublisher
    {
        /// <summary>
        /// Publishes the <see cref="IRestoreEvents.SolutionRestoreCompleted" /> asynchronously such that the caller
        /// need not be concerned about slow consumers.
        /// </summary>
        void OnSolutionRestoreCompleted(SolutionRestoredEventArgs args);
    }
}
