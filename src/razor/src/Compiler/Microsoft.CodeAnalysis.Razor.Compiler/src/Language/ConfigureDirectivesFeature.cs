// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Microsoft.AspNetCore.Razor.Language;

internal sealed class ConfigureDirectivesFeature : RazorEngineFeatureBase, IConfigureRazorParserOptionsFeature
{
    private readonly Dictionary<string, ImmutableArray<DirectiveDescriptor>.Builder> _fileKindToDirectivesMap = new(StringComparer.OrdinalIgnoreCase);

    public void AddDirective(DirectiveDescriptor directive, params ReadOnlySpan<string> fileKinds)
    {
        lock (_fileKindToDirectivesMap)
        {
            // To maintain backwards compatibility, FileKinds.Legacy is assumed when a file kind is not specified.
            if (fileKinds.IsEmpty)
            {
                fileKinds = [FileKinds.Legacy];
            }

            foreach (var fileKind in fileKinds)
            {
                var directives = _fileKindToDirectivesMap.GetOrAdd(fileKind, _ => ImmutableArray.CreateBuilder<DirectiveDescriptor>());
                directives.Add(directive);
            }
        }
    }

    public ImmutableArray<DirectiveDescriptor> GetDirectives(string? fileKind = null)
    {
        // To maintain backwards compatibility, FileKinds.Legacy is assumed when a file kind is not specified.
        fileKind ??= FileKinds.Legacy;

        lock (_fileKindToDirectivesMap)
        {
            return _fileKindToDirectivesMap.TryGetValue(fileKind, out var directives)
                ? directives.ToImmutable()
                : [];
        }
    }

    public int Order => 100;

    void IConfigureRazorParserOptionsFeature.Configure(RazorParserOptions.Builder builder)
    {
        builder.Directives = GetDirectives(builder.FileKind);
    }
}
