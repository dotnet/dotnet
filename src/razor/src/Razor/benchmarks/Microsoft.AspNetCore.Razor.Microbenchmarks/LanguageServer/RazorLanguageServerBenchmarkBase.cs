// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.AspNetCore.Razor.Telemetry;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CodeAnalysis.Text;
using Microsoft.CommonLanguageServerProtocol.Framework;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Nerdbank.Streams;

namespace Microsoft.AspNetCore.Razor.Microbenchmarks.LanguageServer;

public class RazorLanguageServerBenchmarkBase : ProjectSnapshotManagerBenchmarkBase
{
    public RazorLanguageServerBenchmarkBase()
    {
        var (_, serverStream) = FullDuplexStream.CreatePair();
        Logger = new NoopLogger();
        var razorLoggerFactory = new RazorLoggerFactory([new NoopLoggerProvider()]);
        RazorLanguageServer = RazorLanguageServerWrapper.Create(serverStream, serverStream, razorLoggerFactory, NoOpTelemetryReporter.Instance, configure: (collection) =>
        {
            collection.AddSingleton<IOnInitialized, NoopClientNotifierService>();
            collection.AddSingleton<IClientConnection, NoopClientNotifierService>();
            Builder(collection);
        }, featureOptions: BuildFeatureOptions());
    }

    protected internal virtual void Builder(IServiceCollection collection)
    {
    }

    private protected virtual LanguageServerFeatureOptions BuildFeatureOptions()
    {
        return null;
    }

    private protected RazorLanguageServerWrapper RazorLanguageServer { get; }

    private protected NoopLogger Logger { get; }

    internal IDocumentSnapshot GetDocumentSnapshot(string projectFilePath, string filePath, string targetPath)
    {
        var intermediateOutputPath = Path.Combine(Path.GetDirectoryName(projectFilePath), "obj");
        var hostProject = new HostProject(projectFilePath, intermediateOutputPath, RazorConfiguration.Default, rootNamespace: null);
        using var fileStream = new FileStream(filePath, FileMode.Open);
        var text = SourceText.From(fileStream);
        var textLoader = TextLoader.From(TextAndVersion.Create(text, VersionStamp.Create()));
        var hostDocument = new HostDocument(filePath, targetPath, FileKinds.Component);

        var projectSnapshotManager = CreateProjectSnapshotManager();
        projectSnapshotManager.ProjectAdded(hostProject);
        var tagHelpers = CommonResources.LegacyTagHelpers;
        var projectWorkspaceState = ProjectWorkspaceState.Create(tagHelpers, CodeAnalysis.CSharp.LanguageVersion.CSharp11);
        projectSnapshotManager.ProjectWorkspaceStateChanged(hostProject.Key, projectWorkspaceState);
        projectSnapshotManager.DocumentAdded(hostProject.Key, hostDocument, textLoader);
        var projectSnapshot = projectSnapshotManager.GetLoadedProject(hostProject.Key);

        var documentSnapshot = projectSnapshot.GetDocument(filePath);
        return documentSnapshot;
    }

    private class NoopClientNotifierService : IClientConnection, IOnInitialized
    {
        public Task OnInitializedAsync(VSInternalClientCapabilities clientCapabilities, CancellationToken cancellationToken)
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

    internal class NoopLoggerProvider : IRazorLoggerProvider
    {
        public ILogger CreateLogger(string categoryName)
        {
            return new NoopLogger();
        }

        public void Dispose()
        {
        }
    }

    internal class NoopLogger : ILogger, ILspLogger
    {
        public IDisposable BeginScope<TState>(TState state)
        {
            throw new NotImplementedException();
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
        }

        public void LogEndContext(string message, params object[] @params)
        {
        }

        public void LogError(string message, params object[] @params)
        {
        }

        public void LogException(Exception exception, string message = null, params object[] @params)
        {
        }

        public void LogInformation(string message, params object[] @params)
        {
        }

        public void LogStartContext(string message, params object[] @params)
        {
        }

        public void LogWarning(string message, params object[] @params)
        {
        }
    }
}
