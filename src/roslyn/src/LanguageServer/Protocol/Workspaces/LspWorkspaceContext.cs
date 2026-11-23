// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

<<<<<<<< HEAD:src/roslyn/src/LanguageServer/Protocol/Workspaces/LspWorkspaceContext.cs
namespace Microsoft.CodeAnalysis.LanguageServer;
|||||||| 9da2930951f:src/roslyn/src/Features/ExternalAccess/OmniSharp/Options/IOmniSharpLineFormattingOptionsProvider.cs
namespace Microsoft.CodeAnalysis.ExternalAccess.OmniSharp.Options;
========
namespace Microsoft.VisualStudio.LanguageServices.ExternalAccess.ProjectSystem;
>>>>>>>> darc/forward/da9878a-07eb600:src/roslyn/src/VisualStudio/ExternalAccess/Core/ProjectSystem/Api/ProjectSystemUpdateAction.cs

<<<<<<<< HEAD:src/roslyn/src/LanguageServer/Protocol/Workspaces/LspWorkspaceContext.cs
internal readonly record struct LspWorkspaceContext(
    Workspace Workspace,
    Solution Solution,
    TextDocument? Document);
|||||||| 9da2930951f:src/roslyn/src/Features/ExternalAccess/OmniSharp/Options/IOmniSharpLineFormattingOptionsProvider.cs
internal interface IOmniSharpLineFormattingOptionsProvider
{
    OmniSharpLineFormattingOptions GetLineFormattingOptions();
}
========
internal enum ProjectSystemUpdateAction
{
    SetTreatAsUsed = 0,
    UnsetTreatAsUsed = 1,
    Remove = 2,
}
>>>>>>>> darc/forward/da9878a-07eb600:src/roslyn/src/VisualStudio/ExternalAccess/Core/ProjectSystem/Api/ProjectSystemUpdateAction.cs
