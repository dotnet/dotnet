// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;

namespace Microsoft.AspNetCore.Razor.Microbenchmarks;

public abstract partial class ProjectSnapshotManagerBenchmarkBase
{
    private class TestErrorReporter : ErrorReporter
    {
        public override void ReportError(Exception exception)
        {
        }

        public override void ReportError(Exception exception, ProjectSnapshot? project)
        {
        }

        public override void ReportError(Exception exception, Project workspaceProject)
        {
        }
    }
}
