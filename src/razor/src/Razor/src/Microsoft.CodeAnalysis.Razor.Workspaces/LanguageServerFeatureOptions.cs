// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace Microsoft.CodeAnalysis.Razor.Workspaces;

internal abstract class LanguageServerFeatureOptions
{
    public abstract bool SupportsFileManipulation { get; }

    public abstract string CSharpVirtualDocumentSuffix { get; }

    public abstract string HtmlVirtualDocumentSuffix { get; }

    public abstract bool SingleServerSupport { get; }

    public abstract bool DelegateToCSharpOnDiagnosticPublish { get; }

    public abstract bool UsePreciseSemanticTokenRanges { get; }

    public abstract bool ShowAllCSharpCodeActions { get; }

    public abstract bool UpdateBuffersForClosedDocuments { get; }

    // Code action and rename paths in Windows VS Code need to be prefixed with '/':
    // https://github.com/dotnet/razor/issues/8131
    public abstract bool ReturnCodeActionAndRenamePathsWithPrefixedSlash { get; }

    /// <summary>
    /// Whether the file path for the generated C# documents should utilize the project key to
    /// ensure a unique file path per project.
    /// </summary>
    public abstract bool IncludeProjectKeyInGeneratedFilePath { get; }

    public abstract bool UseRazorCohostServer { get; }

    /// <summary>
    /// When enabled, design time code will not be generated. All tooling, except formatting, will be using runtime code generation.
    /// </summary>
    public abstract bool ForceRuntimeCodeGeneration { get; }

    public abstract bool UseNewFormattingEngine { get; }

    /// <summary>
    /// Indicates that client supports soft selection in completion list, meaning that typing a commit 
    /// character with a soft-selected item will not commit that item.
    /// </summary>
    public abstract bool SupportsSoftSelectionInCompletion { get; }

    /// <summary>
    /// Indicates that VSCode-compatible completion trigger character set should be used
    /// </summary>
    public abstract bool UseVsCodeCompletionTriggerCharacters { get; }

    /// <summary>
    /// Indicates whether the language server's miscellanous files project will be initialized with
    /// all Razor files found under the workspace root path.
    /// </summary>
    public abstract bool DoNotInitializeMiscFilesProjectFromWorkspace { get; }
}
