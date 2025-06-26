// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Razor;
using System.Collections.Immutable;
using Microsoft.AspNetCore.Razor.Language;
using System.Threading;

namespace Microsoft.CodeAnalysis.Razor.ProjectSystem;

internal readonly struct CodeDocumentGenerator(RazorProjectEngine projectEngine, RazorCompilerOptions compilerOptions)
{
    public RazorCodeDocument Generate(
        RazorSourceDocument source,
        RazorFileKind fileKind,
        ImmutableArray<RazorSourceDocument> importSources,
        ImmutableArray<TagHelperDescriptor> tagHelpers,
        CancellationToken cancellationToken)
    {
        var forceRuntimeCodeGeneration = compilerOptions.IsFlagSet(RazorCompilerOptions.ForceRuntimeCodeGeneration);

        return forceRuntimeCodeGeneration
            ? projectEngine.Process(source, fileKind, importSources, tagHelpers, cancellationToken)
            : projectEngine.ProcessDesignTime(source, fileKind, importSources, tagHelpers, cancellationToken);
    }

    public RazorCodeDocument GenerateDesignTime(
        RazorSourceDocument source,
        RazorFileKind fileKind,
        ImmutableArray<RazorSourceDocument> importSources,
        ImmutableArray<TagHelperDescriptor> tagHelpers,
        CancellationToken cancellationToken)
    {
        return projectEngine.ProcessDesignTime(source, fileKind, importSources, tagHelpers, cancellationToken);
    }
}
