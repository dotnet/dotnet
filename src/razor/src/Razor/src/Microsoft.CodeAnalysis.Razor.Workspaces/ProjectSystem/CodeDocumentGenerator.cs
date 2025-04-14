// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

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
