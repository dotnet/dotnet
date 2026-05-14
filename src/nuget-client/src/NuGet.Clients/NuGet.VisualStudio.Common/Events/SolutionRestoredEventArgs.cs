// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft;
using NuGet.Common;

namespace NuGet.VisualStudio
{
    /// <summary>
    /// The data available when the <see cref="IRestoreEvents.SolutionRestoreCompleted"/> event is raised.
    /// </summary>
    public class SolutionRestoredEventArgs : EventArgs
    {
        public NuGetOperationStatus RestoreStatus { get; }
        public string SolutionDirectory { get; }

        public SolutionRestoredEventArgs(
            NuGetOperationStatus restoreStatus,
            string solutionDirectory)
        {
            Assumes.NotNullOrEmpty(solutionDirectory);

            RestoreStatus = restoreStatus;
            SolutionDirectory = solutionDirectory;
        }
    }
}
