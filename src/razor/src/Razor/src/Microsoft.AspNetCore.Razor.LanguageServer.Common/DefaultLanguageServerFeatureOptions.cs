// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.CodeAnalysis.Razor.Workspaces;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

internal class DefaultLanguageServerFeatureOptions : LanguageServerFeatureOptions
{
    public override bool SupportsFileManipulation => true;

    public override string ProjectConfigurationFileName => LanguageServerConstants.DefaultProjectConfigurationFile;

    public override string CSharpVirtualDocumentSuffix => ".ide.g.cs";

    public override string HtmlVirtualDocumentSuffix => "__virtual.html";

    public override bool SingleServerCompletionSupport => false;

    public override bool SingleServerSupport => false;

    public override bool SupportsDelegatedCodeActions => false;

    // Code action and rename paths in Windows VS Code need to be prefixed with '/':
    // https://github.com/dotnet/razor/issues/8131
    public override bool ReturnCodeActionAndRenamePathsWithPrefixedSlash
        => RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
}
