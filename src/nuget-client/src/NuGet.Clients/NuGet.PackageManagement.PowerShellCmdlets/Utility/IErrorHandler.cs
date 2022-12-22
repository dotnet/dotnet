// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Management.Automation;

namespace NuGet.PackageManagement.PowerShellCmdlets
{
    /// <summary>
    /// Interface defining common NuGet Cmdlet error handling and generation operations.
    /// </summary>
    public interface IErrorHandler
    {
        /// <summary>
        /// Handles a native PowerShell ErrorRecord. If terminating is set to true, this method does not return.
        /// </summary>
        /// <param name="errorRecord">The record representing the error condition.</param>
        /// <param name="terminating">If true, write a terminating error else write to error stream.</param>
        void HandleError(ErrorRecord errorRecord, bool terminating);

        /// <summary>
        /// Handles a regular BCL Exception. If terminating is set to true, this method does not return.
        /// </summary>
        /// <param name="exception">The exception representing the error condition.</param>
        /// <param name="terminating">If true, write a terminating error else write to error stream.</param>
        /// <param name="errorId">
        /// The local-agnostic error id to use. Well-known error ids are defined in
        /// <see cref="NuGet.PowerShell.Commands.NuGetErrorId" />.
        /// </param>
        /// <param name="category">The PowerShell ErrorCategory to use.</param>
        /// <param name="target">The context object associated with this error condition. This may be null.</param>
        void HandleException(Exception exception, bool terminating, string errorId = NuGetErrorId.CmdletUnhandledException, ErrorCategory category = ErrorCategory.NotSpecified, object target = null);

        /// <summary>
        /// Generates an error to signify the specified project was not found.
        /// If terminating is set to true, this method does not return.
        /// </summary>
        /// <param name="projectName">The name of the project that was not found.</param>
        /// <param name="terminating">If true, write a terminating error else write to error stream.</param>
        void WriteProjectNotFoundError(string projectName, bool terminating);

        /// <summary>
        /// Generates a terminating error to signify there is no open solution. This method does not return.
        /// </summary>
        void ThrowSolutionNotOpenTerminatingError();

        /// <summary>
        /// Generates a terminating error to signify there is are no compatible projects or the solution is empty.
        /// This method does not return.
        /// </summary>
        void ThrowNoCompatibleProjectsTerminatingError();
    }
}
