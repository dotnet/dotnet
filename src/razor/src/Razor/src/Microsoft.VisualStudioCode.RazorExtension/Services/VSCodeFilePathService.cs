// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Composition;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.CodeAnalysis.Razor.Workspaces;

namespace Microsoft.CodeAnalysis.Remote.Razor;

[Export(typeof(IFilePathService)), Shared]
[method: ImportingConstructor]
internal sealed class VSCodeFilePathService(LanguageServerFeatureOptions options) : AbstractFilePathService(options)
{
    private readonly LanguageServerFeatureOptions _options = options;

    public override Uri GetRazorDocumentUri(Uri virtualDocumentUri)
    {
        if (_options.UseRazorCohostServer && IsVirtualCSharpFile(virtualDocumentUri))
        {
            throw new InvalidOperationException("Can not get a Razor document from a generated document Uri in cohosting");
        }

        if (virtualDocumentUri.Scheme == "razor-html")
        {
            var builder = new UriBuilder(virtualDocumentUri);
            builder.Scheme = Uri.UriSchemeFile;
            return base.GetRazorDocumentUri(builder.Uri);
        }

        return base.GetRazorDocumentUri(virtualDocumentUri);
    }

    public override bool IsVirtualCSharpFile(Uri uri)
    {
        if (_options.UseRazorCohostServer)
        {
            return RazorUri.IsGeneratedDocumentUri(uri);
        }

        return base.IsVirtualCSharpFile(uri);
    }
}
