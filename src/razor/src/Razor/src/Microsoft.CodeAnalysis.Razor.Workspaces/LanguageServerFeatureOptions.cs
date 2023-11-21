// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace Microsoft.CodeAnalysis.Razor.Workspaces;

internal abstract class LanguageServerFeatureOptions
{
    public abstract bool SupportsFileManipulation { get; }

    public abstract string ProjectConfigurationFileName { get; }

    public abstract string CSharpVirtualDocumentSuffix { get; }

    public abstract string HtmlVirtualDocumentSuffix { get; }

    public abstract bool SingleServerCompletionSupport { get; }

    public abstract bool SingleServerSupport { get; }

    public abstract bool DelegateToCSharpOnDiagnosticPublish { get; }

    public abstract bool UsePreciseSemanticTokenRanges { get; }

    public abstract bool ShowAllCSharpCodeActions { get; }

    public abstract bool UpdateBuffersForClosedDocuments { get; }

    // Code action and rename paths in Windows VS Code need to be prefixed with '/':
    // https://github.com/dotnet/razor/issues/8131
    public abstract bool ReturnCodeActionAndRenamePathsWithPrefixedSlash { get; }

    /// <summary>
    /// Whether the file path for the generated C# and Html documents should utilize the project key to
    /// ensure a unique file path per project.
    /// </summary>
    public abstract bool IncludeProjectKeyInGeneratedFilePath { get; }

    /// <summary>
    /// Whether to monitor the entire workspace folder for any project.razor.bin files
    /// </summary>
    /// <remarks>
    /// When this is off, the language server won't have any project knowledge unless the
    /// razor/monitorProjectConfigurationFilePath notification is sent.
    /// </remarks>
    public abstract bool MonitorWorkspaceFolderForConfigurationFiles { get; }
}
