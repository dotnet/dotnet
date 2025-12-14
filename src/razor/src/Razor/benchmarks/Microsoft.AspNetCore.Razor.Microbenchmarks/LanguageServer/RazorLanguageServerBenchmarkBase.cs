// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System;
using System.Collections.Immutable;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer;
using Microsoft.AspNetCore.Razor.LanguageServer.Hosting;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Telemetry;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CodeAnalysis.Text;
using Microsoft.Extensions.DependencyInjection;
using Nerdbank.Streams;

namespace Microsoft.AspNetCore.Razor.Microbenchmarks.LanguageServer;

public class RazorLanguageServerBenchmarkBase : ProjectSnapshotManagerBenchmarkBase
{
    public RazorLanguageServerBenchmarkBase()
    {
        var (_, serverStream) = FullDuplexStream.CreatePair();
        var razorLoggerFactory = EmptyLoggerFactory.Instance;
        Logger = razorLoggerFactory.GetOrCreateLogger(GetType());
        RazorLanguageServerHost = RazorLanguageServerHost.Create(
            serverStream,
            serverStream,
            razorLoggerFactory,
            NoOpTelemetryReporter.Instance,
            configureServices: (collection) =>
            {
                collection.AddSingleton<IOnInitialized, NoOpClientNotifierService>();
                collection.AddSingleton<IClientConnection, NoOpClientNotifierService>();
                collection.AddSingleton<IRazorProjectInfoDriver, NoOpRazorProjectInfoDriver>();
                Builder(collection);
            },
            featureOptions: BuildFeatureOptions());
    }

    protected internal virtual void Builder(IServiceCollection collection)
    {
    }

    private protected virtual LanguageServerFeatureOptions BuildFeatureOptions()
    {
        return null;
    }

    private protected RazorLanguageServerHost RazorLanguageServerHost { get; }

    private protected ILogger Logger { get; }

    internal async Task<IDocumentSnapshot> GetDocumentSnapshotAsync(string projectFilePath, string filePath, string targetPath, string rootNamespace = null)
    {
        var intermediateOutputPath = Path.Combine(Path.GetDirectoryName(projectFilePath), "obj");
        var hostProject = new HostProject(projectFilePath, intermediateOutputPath, RazorConfiguration.Default, rootNamespace);
        using var fileStream = new FileStream(filePath, FileMode.Open);
        var text = SourceText.From(fileStream);
        var hostDocument = new HostDocument(filePath, targetPath, RazorFileKind.Component);

        var projectManager = CreateProjectSnapshotManager();

        await projectManager.UpdateAsync(
            updater =>
            {
                updater.AddProject(hostProject);
                var projectWorkspaceState = ProjectWorkspaceState.Create([.. CommonResources.LegacyTagHelpers]);
                updater.UpdateProjectWorkspaceState(hostProject.Key, projectWorkspaceState);
                updater.AddDocument(hostProject.Key, hostDocument, text);
            },
            CancellationToken.None);

        return projectManager.GetRequiredDocument(hostProject.Key, filePath);
    }

    private sealed class NoOpClientNotifierService : IClientConnection, IOnInitialized
    {
        public Task OnInitializedAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task SendNotificationAsync<TParams>(string method, TParams @params, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SendNotificationAsync(string method, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<TResponse> SendRequestAsync<TParams, TResponse>(string method, TParams @params, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }

    private sealed class NoOpRazorProjectInfoDriver : IRazorProjectInfoDriver
    {
        public void AddListener(IRazorProjectInfoListener listener)
        {
        }

        public ImmutableArray<RazorProjectInfo> GetLatestProjectInfo()
            => [];

        public Task WaitForInitializationAsync()
            => Task.CompletedTask;
    }
}
