// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
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
        var current = new DirectoryInfo(AppContext.BaseDirectory);
        while (current != null && !File.Exists(Path.Combine(current.FullName, "Razor.sln")))
        {
            current = current.Parent;
        }

        RepoRoot = current.FullName;

        var (_, serverStream) = FullDuplexStream.CreatePair();
        var logger = new NoopLogger();
        RazorLanguageServer = RazorLanguageServerWrapper.Create(serverStream, serverStream, logger, configure: (collection) => {
            collection.AddSingleton<ClientNotifierServiceBase, NoopClientNotifierService>();
            Builder(collection);
        });
    }

    protected internal virtual void Builder(IServiceCollection collection)
    {
    }

    protected string RepoRoot { get; }

    private protected RazorLanguageServerWrapper RazorLanguageServer { get; }

    internal DocumentSnapshot GetDocumentSnapshot(string projectFilePath, string filePath, string targetPath)
    {
        var hostProject = new HostProject(projectFilePath, RazorConfiguration.Default, rootNamespace: null);
        using var fileStream = new FileStream(filePath, FileMode.Open);
        var text = SourceText.From(fileStream);
        var textLoader = TextLoader.From(TextAndVersion.Create(text, VersionStamp.Create()));
        var hostDocument = new HostDocument(filePath, targetPath, FileKinds.Component);

        var projectSnapshotManager = CreateProjectSnapshotManager();
        projectSnapshotManager.ProjectAdded(hostProject);
        projectSnapshotManager.DocumentAdded(hostProject, hostDocument, textLoader);
        var projectSnapshot = projectSnapshotManager.GetOrCreateProject(projectFilePath);

        var documentSnapshot = projectSnapshot.GetDocument(filePath);
        return documentSnapshot;
    }

    private class NoopClientNotifierService : ClientNotifierServiceBase
    {
        public override Task OnInitializedAsync(VSInternalClientCapabilities clientCapabilities, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public override Task SendNotificationAsync<TParams>(string method, TParams @params, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public override Task SendNotificationAsync(string method, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public override Task<TResponse> SendRequestAsync<TParams, TResponse>(string method, TParams @params, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }

    private class NoopLogger : IRazorLogger
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
