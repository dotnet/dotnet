// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Razor.Language.CodeGeneration;

namespace Microsoft.AspNetCore.Razor.Language.Components;

internal class ComponentCodeTarget : CodeTarget
{
    private readonly RazorCodeGenerationOptions _options;
    private readonly RazorLanguageVersion _version;

    public ComponentCodeTarget(RazorCodeGenerationOptions options, RazorLanguageVersion version, IEnumerable<ICodeTargetExtension> extensions)
    {
        _options = options;
        _version = version;

        // Components provide some built-in target extensions that don't apply to
        // legacy documents.
        Extensions = new[] { new ComponentTemplateTargetExtension(), }.Concat(extensions).ToArray();
    }

    public ICodeTargetExtension[] Extensions { get; }

    public override IntermediateNodeWriter CreateNodeWriter()
    {
        return _options.DesignTime
            ? new ComponentDesignTimeNodeWriter(_version)
            : new ComponentRuntimeNodeWriter(_version);
    }

    public override TExtension GetExtension<TExtension>()
    {
        for (var i = 0; i < Extensions.Length; i++)
        {
            var match = Extensions[i] as TExtension;
            if (match != null)
            {
                return match;
            }
        }

        return null;
    }

    public override bool HasExtension<TExtension>()
    {
        for (var i = 0; i < Extensions.Length; i++)
        {
            var match = Extensions[i] as TExtension;
            if (match != null)
            {
                return true;
            }
        }

        return false;
    }
}
