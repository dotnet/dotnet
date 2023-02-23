// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.CodeAnalysis.Host;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;

namespace Microsoft.CodeAnalysis.Razor;

internal interface IErrorReporter : IWorkspaceService
{
    void ReportError(Exception exception);
    void ReportError(Exception exception, IProjectSnapshot? project);
    void ReportError(Exception exception, Project workspaceProject);
}
