// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Composition;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.CodeAnalysis.ExternalAccess.Razor.Cohost;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.Workspaces.Settings;
using Microsoft.VisualStudio.Razor.LanguageClient.Cohost;

namespace Microsoft.VisualStudioCode.RazorExtension.Endpoints;

[Shared]
[Export(typeof(ICohostConfigurationChangedService))]
[Export(typeof(IRazorCohostStartupService))]
[method: ImportingConstructor]
internal sealed class CohostConfigurationChangedService(
    IClientSettingsManager clientSettingsManager,
    ILoggerFactory loggerFactory) : ICohostConfigurationChangedService, IRazorCohostStartupService
{
    private readonly IClientSettingsManager _clientSettingsManager = clientSettingsManager;
    private readonly ILogger _logger = loggerFactory.GetOrCreateLogger<CohostConfigurationChangedService>();

    public int Order => WellKnownStartupOrder.Default;

    public Task StartupAsync(VSInternalClientCapabilities clientCapabilities, RazorCohostRequestContext requestContext, CancellationToken cancellationToken)
    {
        return RefreshOptionsAsync(requestContext, cancellationToken);
    }

    public Task OnConfigurationChangedAsync(RazorCohostRequestContext requestContext, CancellationToken cancellationToken)
    {
        return RefreshOptionsAsync(requestContext, cancellationToken);
    }

    private async Task RefreshOptionsAsync(RazorCohostRequestContext requestContext, CancellationToken cancellationToken)
    {
        _logger.LogDebug($"Refreshing options from client.");

        var razorClientLanguageServerManager = requestContext.GetRequiredService<IRazorClientLanguageServerManager>();

        var configurationParams = new ConfigurationParams()
        {
            Items = [
                //TODO: new ConfigurationItem { Section = "razor.format.enable" },
                new ConfigurationItem { Section = "razor.format.code_block_brace_on_next_line" },
                new ConfigurationItem { Section = "razor.completion.commit_elements_with_space" },
            ]
        };

        var options = await razorClientLanguageServerManager.SendRequestAsync<ConfigurationParams, JsonArray>(
            Methods.WorkspaceConfigurationName,
            configurationParams,
            cancellationToken).ConfigureAwait(false);

        var current = _clientSettingsManager.GetClientSettings().AdvancedSettings;
        var settings = current with
        {
            CodeBlockBraceOnNextLine = GetBooleanOptionValue(options[0], current.CodeBlockBraceOnNextLine),
            CommitElementsWithSpace = GetBooleanOptionValue(options[1], current.CommitElementsWithSpace),
        };

        _clientSettingsManager.Update(settings);
    }

    private static bool GetBooleanOptionValue(JsonNode? jsonNode, bool defaultValue)
    {
        if (jsonNode is null)
        {
            return defaultValue;
        }

        return jsonNode.ToString() == "true";
    }
}
