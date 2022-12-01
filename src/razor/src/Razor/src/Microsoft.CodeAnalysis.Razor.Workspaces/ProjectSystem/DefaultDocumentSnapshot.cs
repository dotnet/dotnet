// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis.Text;

namespace Microsoft.CodeAnalysis.Razor.ProjectSystem;

internal class DefaultDocumentSnapshot : DocumentSnapshot
{
    public DefaultDocumentSnapshot(DefaultProjectSnapshot project, DocumentState state)
    {
        if (project is null)
        {
            throw new ArgumentNullException(nameof(project));
        }

        if (state is null)
        {
            throw new ArgumentNullException(nameof(state));
        }

        ProjectInternal = project;
        State = state;
    }

    public DefaultProjectSnapshot ProjectInternal { get; }

    public DocumentState State { get; }

    public override string FileKind => State.HostDocument.FileKind;

    public override string FilePath => State.HostDocument.FilePath;

    public override string TargetPath => State.HostDocument.TargetPath;

    public override ProjectSnapshot Project => ProjectInternal;

    public override bool SupportsOutput => true;

    public override IReadOnlyList<DocumentSnapshot> GetImports()
    {
        return State.GetImports(ProjectInternal);
    }

    public override Task<SourceText> GetTextAsync()
    {
        return State.GetTextAsync();
    }
    public override Task<VersionStamp> GetTextVersionAsync()
    {
        return State.GetTextVersionAsync();
    }

    public override async Task<RazorCodeDocument> GetGeneratedOutputAsync()
    {
        var (output, _) = await State.GetGeneratedOutputAndVersionAsync(ProjectInternal, this).ConfigureAwait(false);
        return output;
    }

    public override bool TryGetText(out SourceText result)
    {
        return State.TryGetText(out result);
    }

    public override bool TryGetTextVersion(out VersionStamp result)
    {
        return State.TryGetTextVersion(out result);
    }

    public override bool TryGetGeneratedOutput(out RazorCodeDocument result)
    {
        if (State.IsGeneratedOutputResultAvailable)
        {
#pragma warning disable VSTHRD002 // Avoid problematic synchronous waits
            result = State.GetGeneratedOutputAndVersionAsync(ProjectInternal, this).Result.output;
#pragma warning restore VSTHRD002 // Avoid problematic synchronous waits
            return true;
        }

        result = null;
        return false;
    }
}
