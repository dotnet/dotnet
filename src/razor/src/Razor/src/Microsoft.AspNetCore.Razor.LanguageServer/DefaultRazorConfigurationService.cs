// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.Hosting;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.Settings;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

internal class DefaultRazorConfigurationService : IConfigurationSyncService
{
    private readonly IClientConnection _clientConnection;
    private readonly ILogger _logger;

    public DefaultRazorConfigurationService(IClientConnection clientConnection, ILoggerFactory loggerFactory)
    {
        if (clientConnection is null)
        {
            throw new ArgumentNullException(nameof(clientConnection));
        }

        if (loggerFactory is null)
        {
            throw new ArgumentNullException(nameof(loggerFactory));
        }

        _clientConnection = clientConnection;
        _logger = loggerFactory.GetOrCreateLogger<DefaultRazorConfigurationService>();
    }

    public async Task<RazorLSPOptions?> GetLatestOptionsAsync(CancellationToken cancellationToken)
    {
        try
        {
            var request = GenerateConfigParams();

            var result = await _clientConnection.SendRequestAsync<ConfigurationParams, JsonObject[]>(Methods.WorkspaceConfigurationName, request, cancellationToken).ConfigureAwait(false);

            // LSP spec indicates result should be the same length as the number of ConfigurationItems we pass in.
            if (result?.Length != request.Items.Length || result[0] is null)
            {
                _logger.LogWarning($"Client failed to provide the expected configuration.");
                return null;
            }

            var instance = BuildOptions(result);
            return instance;
        }
        catch (Exception ex)
        {
            _logger.LogWarning($"Failed to sync client configuration on the server: {ex}");
            return null;
        }
    }

    private static ConfigurationParams GenerateConfigParams()
    {
        // NOTE: Do not change the ordering of sections without updating
        // the code in the BuildOptions method below.
        return new ConfigurationParams()
        {
            Items = new[]
            {
                new ConfigurationItem()
                {
                    Section = "razor"
                },
                new ConfigurationItem()
                {
                    Section = "html"
                },
                new ConfigurationItem()
                {
                    Section = "vs.editor.razor"
                }
            }
        };
    }

    // Internal for testing
    internal RazorLSPOptions BuildOptions(JsonObject[] result)
    {
        // VS Code will send back settings in the first two elements, VS will send back settings in the 3rd
        // so we can effectively detect which IDE we're in.
        if (result[0] is null or { Count: 0 } && result[1] is null or { Count: 0 })
        {
            var settings = ExtractVSOptions(result);
            return RazorLSPOptions.From(settings);
        }
        else
        {
            ExtractVSCodeOptions(result, out var formatting, out var autoClosingTags, out var commitElementsWithSpace, out var codeBlockBraceOnNextLine);
            return RazorLSPOptions.Default with
            {
                Formatting = formatting,
                AutoClosingTags = autoClosingTags,
                CommitElementsWithSpace = commitElementsWithSpace,
                CodeBlockBraceOnNextLine = codeBlockBraceOnNextLine
            };
        }
    }

    private void ExtractVSCodeOptions(
        JsonObject[] result,
        out FormattingFlags formatting,
        out bool autoClosingTags,
        out bool commitElementsWithSpace,
        out bool codeBlockBraceOnNextLine)
    {
        var razor = result[0];
        var html = result[1];

        formatting = RazorLSPOptions.Default.Formatting;
        autoClosingTags = RazorLSPOptions.Default.AutoClosingTags;
        codeBlockBraceOnNextLine = RazorLSPOptions.Default.CodeBlockBraceOnNextLine;
        // Deliberately not using the "default" here because we want a different default for VS Code, as
        // this matches VS Code's html servers commit behaviour
        commitElementsWithSpace = false;

        if (razor.TryGetPropertyValue("format", out var parsedFormatNode) &&
            parsedFormatNode?.AsObject() is { } parsedFormat)
        {
            if (parsedFormat.TryGetPropertyValue("enable", out var parsedEnableFormatting) &&
                parsedEnableFormatting is not null)
            {
                var formattingEnabled = GetObjectOrDefault(parsedEnableFormatting, formatting.IsEnabled());
                if (formattingEnabled)
                {
                    formatting |= FormattingFlags.Enabled;
                }
                else
                {
                    formatting = FormattingFlags.Disabled;
                }
            }

            if (parsedFormat.TryGetPropertyValue("codeBlockBraceOnNextLine", out var parsedCodeBlockBraceOnNextLine) &&
                parsedCodeBlockBraceOnNextLine is not null)
            {
                codeBlockBraceOnNextLine = GetObjectOrDefault(parsedCodeBlockBraceOnNextLine, codeBlockBraceOnNextLine);
            }
        }

        if (razor.TryGetPropertyValue("completion", out var parsedCompletionNode) &&
            parsedCompletionNode?.AsObject() is { } parsedCompletion)
        {
            if (parsedCompletion.TryGetPropertyValue("commitElementsWithSpace", out var parsedCommitElementsWithSpace) &&
                parsedCommitElementsWithSpace is not null)
            {
                commitElementsWithSpace = GetObjectOrDefault(parsedCommitElementsWithSpace, commitElementsWithSpace);
            }
        }

        if (html.TryGetPropertyValue("autoClosingTags", out var parsedAutoClosingTags) &&
            parsedAutoClosingTags is not null)
        {
            autoClosingTags = GetObjectOrDefault(parsedAutoClosingTags, autoClosingTags);
        }
    }

    private ClientSettings ExtractVSOptions(JsonObject[] result)
    {
        try
        {
            var settings = result[2].Deserialize<ClientSettings>();
            if (settings is null)
            {
                return ClientSettings.Default;
            }

            // Deserializing can result in null properties. Fill with default as needed
            if (settings.ClientSpaceSettings is null)
            {
                settings = settings with { ClientSpaceSettings = ClientSpaceSettings.Default };
            }

            if (settings.ClientCompletionSettings is null)
            {
                settings = settings with { ClientCompletionSettings = ClientCompletionSettings.Default };
            }

            if (settings.AdvancedSettings is null)
            {
                settings = settings with { AdvancedSettings = ClientAdvancedSettings.Default };
            }

            return settings;
        }
        catch (Exception)
        {
            return ClientSettings.Default;
        }
    }

    private T GetObjectOrDefault<T>(JsonNode token, T defaultValue, [CallerArgumentExpression(nameof(defaultValue))] string? expression = null)
    {
        try
        {
            // GetValue could potentially throw here if the user provides malformed options.
            // If this occurs, catch the exception and return the default value.
            return token.GetValue<T>() ?? defaultValue;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Malformed option: Token {token} cannot be converted to type {typeof(T)} for {expression}.");
            return defaultValue;
        }
    }
}
