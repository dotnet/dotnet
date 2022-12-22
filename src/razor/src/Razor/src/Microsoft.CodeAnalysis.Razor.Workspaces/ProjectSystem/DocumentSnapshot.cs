// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis.Text;

namespace Microsoft.CodeAnalysis.Razor.ProjectSystem;

internal abstract class DocumentSnapshot
{
    public abstract string FileKind { get; }

    public abstract string FilePath { get; }

    public abstract string TargetPath { get; }

    public abstract ProjectSnapshot Project { get; }

    public abstract bool SupportsOutput { get; }

    public abstract IReadOnlyList<DocumentSnapshot> GetImports();

    public abstract Task<SourceText> GetTextAsync();

    public abstract Task<VersionStamp> GetTextVersionAsync();

    public abstract Task<RazorCodeDocument> GetGeneratedOutputAsync();

    public abstract bool TryGetText(out SourceText result);

    public abstract bool TryGetTextVersion(out VersionStamp result);

    public abstract bool TryGetGeneratedOutput(out RazorCodeDocument result);
}
