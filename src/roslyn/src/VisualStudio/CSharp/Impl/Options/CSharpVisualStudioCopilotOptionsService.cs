﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Composition;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Copilot;
using Microsoft.CodeAnalysis.Editor.Shared.Utilities;
using Microsoft.CodeAnalysis.Host.Mef;
using Microsoft.Internal.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Settings;
using Microsoft.VisualStudio.Shell;

namespace Microsoft.VisualStudio.LanguageServices.CSharp.Options;

[ExportLanguageService(typeof(ICopilotOptionsService), LanguageNames.CSharp), Shared]
internal sealed class CSharpVisualStudioCopilotOptionsService : ICopilotOptionsService
{
    /// <summary>
    /// Guid for UI context that is set from Copilot when the package is initialized
    /// </summary>
    private const string CopilotHasLoadedGuid = "871c3e1c-e58c-4ce9-b6a7-26600555739a";

    /// <summary>
    /// Guid for UI Context that is set from Copilot when sign in related UI contexts have been set properly. Used to determine when UI context status is final
    /// for a set of operations. When this UI context is not active, the signed in and entitled contexts values may not be correct.
    /// </summary>
    private const string GitHubAccountStatusDetermined = "3049be7e-71ee-4045-a510-f8ee1a967723";

    /// <summary>
    /// Guid for UI context that is set from Copilot when we detect a GitHub account is signed in.
    /// </summary>
    private const string GitHubAccountStatusSignedIn = "ef3ebbb7-511d-472c-ae4b-6af1bb44f378";

    /// <summary>
    /// Guid for UI context that is set from VS Identity Service when we detect that a signed in GitHub account is entitled to access Copilot.
    /// </summary>
    private const string GitHubAccountStatusIsCopilotEntitled = "3DE3FA6E-91B2-46C1-9E9E-DD04975BB890";

    private const string CopilotOptionNamePrefix = "Microsoft.VisualStudio.Conversations";

    // Default value must reflect their default values in ConversationsOptions in Copilot repo.
    private readonly CopilotOption _copilotCodeAnalysisOption = new("EnableCSharpCodeAnalysis", false);
    private readonly CopilotOption _copilotRefineOption = new("EnableCSharpRefineQuickActionSuggestion", false);
    private readonly CopilotOption _copilotOnTheFlyDocsOption = new("EnableOnTheFlyDocs", true);
    private readonly CopilotOption _copilotGenerateDocumentationCommentOption = new("EnableCSharpGenerateDocumentationComment", true);
    private readonly CopilotOption _copilotGenerateMethodImplementationOption = new("EnableCSharpGenerateMethodImplementation", true);

    private static readonly UIContext s_copilotHasLoadedUIContext = UIContext.FromUIContextGuid(new Guid(CopilotHasLoadedGuid));
    private static readonly UIContext s_gitHubAccountStatusDeterminedContext = UIContext.FromUIContextGuid(new Guid(GitHubAccountStatusDetermined));
    private static readonly UIContext s_gitHubAccountStatusIsCopilotEntitledUIContext = UIContext.FromUIContextGuid(new Guid(GitHubAccountStatusIsCopilotEntitled));
    private static readonly UIContext s_gitHubAccountStatusSignedInUIContext = UIContext.FromUIContextGuid(new Guid(GitHubAccountStatusSignedIn));

    private readonly Task<ISettingsManager> _settingsManagerTask;

    /// <summary>
    /// Determines if Copilot is active and the user is signed in and entitled to use Copilot.
    /// </summary>
    private static bool IsGithubCopilotLoadedAndSignedIn
        => s_copilotHasLoadedUIContext.IsActive
        && s_gitHubAccountStatusDeterminedContext.IsActive
        && s_gitHubAccountStatusSignedInUIContext.IsActive
        && s_gitHubAccountStatusIsCopilotEntitledUIContext.IsActive;

    [method: ImportingConstructor]
    [method: Obsolete(MefConstruction.ImportingConstructorMessage, error: true)]
    public CSharpVisualStudioCopilotOptionsService(
        IVsService<SVsSettingsPersistenceManager, ISettingsManager> settingsManagerService,
        IThreadingContext threadingContext)
    {
        _settingsManagerTask = settingsManagerService.GetValueAsync(threadingContext.DisposalToken);
    }

    private async Task<bool> IsCopilotOptionEnabledAsync(CopilotOption option)
    {
        if (!IsGithubCopilotLoadedAndSignedIn)
            return false;

        var settingManager = await _settingsManagerTask.ConfigureAwait(false);
        // The bool setting is persisted as 0=None, 1=True, 2=False, so it needs to be retrieved as an int.
        // If isEnabled is 0 or the value is not persisted, we should return the default value for the option.
        var isEnabled = settingManager.GetValueOrDefault($"{CopilotOptionNamePrefix}.{option.Name}", 0);
        return isEnabled == 1 || (isEnabled == 0 && option.DefaultValue);
    }

    public Task<bool> IsCodeAnalysisOptionEnabledAsync()
        => IsCopilotOptionEnabledAsync(_copilotCodeAnalysisOption);

    public Task<bool> IsRefineOptionEnabledAsync()
        => IsCopilotOptionEnabledAsync(_copilotRefineOption);

    public Task<bool> IsOnTheFlyDocsOptionEnabledAsync()
        => IsCopilotOptionEnabledAsync(_copilotOnTheFlyDocsOption);

    public Task<bool> IsGenerateDocumentationCommentOptionEnabledAsync()
        => IsCopilotOptionEnabledAsync(_copilotGenerateDocumentationCommentOption);

    public Task<bool> IsImplementNotImplementedExceptionEnabledAsync()
        => IsCopilotOptionEnabledAsync(_copilotGenerateMethodImplementationOption);

    private record struct CopilotOption(string Name, bool DefaultValue);
}
