// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Razor.ExternalAccess.OmniSharp.Document;
using Microsoft.AspNetCore.Razor.Language;

namespace Microsoft.AspNetCore.Razor.OmniSharpPlugin;

public sealed class ProjectConfiguration
{
    internal ProjectConfiguration(RazorConfiguration configuration, IReadOnlyList<OmniSharpHostDocument> documents, string rootNamespace)
    {
        if (configuration is null)
        {
            throw new ArgumentNullException(nameof(configuration));
        }

        if (documents is null)
        {
            throw new ArgumentNullException(nameof(documents));
        }

        Configuration = configuration;
        Documents = documents;
        RootNamespace = rootNamespace;
    }

    public RazorConfiguration Configuration { get; }

    internal IReadOnlyList<OmniSharpHostDocument> Documents { get; }

    public string RootNamespace { get; }
}
